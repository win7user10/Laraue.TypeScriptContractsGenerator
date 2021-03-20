﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Laraue.CodeTranslation.Abstractions.Code;
using Laraue.CodeTranslation.Abstractions.Output;
using Laraue.TypeScriptContractsGenerator.Extensions;
using Laraue.TypeScriptContractsGenerator.Types;
using Array = Laraue.TypeScriptContractsGenerator.Types.Array;
using Enum = Laraue.TypeScriptContractsGenerator.Types.Enum;
using String = Laraue.TypeScriptContractsGenerator.Types.String;

namespace Laraue.TypeScriptContractsGenerator
{
    public class TypePartsGenerator : ITypePartsCodeGenerator
    {
        /// <inheritdoc />
        public string[] GenerateImportStrings(OutputType type)
        {
            var strings = type.UsedTypes.Select(usedType => GetImportString(type, usedType));
            return strings.ToArray();
        }

        public virtual string GenerateName(OutputType type) => type.Name.Name.ToPascalCase();

        [CanBeNull]
        public virtual string[] GetFilePathParts(OutputType type) => type.TypeMetadata?.ClrType?.Namespace?.Split('.');

        [CanBeNull]
        public virtual string GetFileName(OutputType type) => type.Name.Name.ToCamelCase();

        public virtual string GenerateName(OutputPropertyType property) => property.PropertyName.ToCamelCase();

        public virtual string GenerateDefaultValue(OutputPropertyType property)
        {
            if (IsNullableType(property))
            {
                return "null";
            }

            return property.OutputType switch
            {
                Number => "0",
                String => "''",
                Enum => GenerateDefaultEnumValue(property),
                _ => throw new NotImplementedException($"{property.OutputType.GetType()} default value is unknown")
            };
        }

        public virtual string GeneratePropertyType(OutputPropertyType property)
        {
            var codeBuilder = new StringBuilder(property.OutputType.Name);
            if (IsNullableType(property))
            {
                codeBuilder.Append(" | null");
            }

            return codeBuilder.ToString();
        }

        public virtual bool ShouldBeUsedTypingInPropertyDefinition(OutputPropertyType property)
        {
            switch (property.OutputType)
            {
                case Number:
                case String when !IsNullableType(property):
                    return false;
                default:
                    return true;
            }
        }

        protected virtual bool IsNullableType(OutputPropertyType property)
        {
            var propertyType = property.PropertyMetadata.PropertyType;
            return propertyType.IsNullable
                   || property.OutputType is Array
                   || propertyType.ClrType.IsClass;
        }

        protected virtual string GenerateDefaultEnumValue(OutputPropertyType property)
        {
            if (property.OutputType is not Enum enumType)
            {
                throw new InvalidOperationException($"Impossible to get enum value from the type {property.OutputType.GetType()}");
            }

            var enumName = GenerateName(property.OutputType);
            var enumValues = enumType.EnumValues;
            var firstEnumValue = enumValues.OrderBy(x => x.Value).First().Key;
            return $"{enumName}.{firstEnumValue}";
        }

        public string GetImportString(OutputType importerType, OutputType importingType)
        {
            if (importerType == importingType)
            {
                throw new InvalidOperationException($"Importer type {importerType} attempts to import self");
            }

            if (importerType?.TypeMetadata == null)
            {
                throw new InvalidOperationException($"{importerType} does not contain a data to generate import string.");
            }

            if (importingType?.TypeMetadata == null)
            {
                throw new InvalidOperationException($"{importingType} does not contain a data to generate import string.");
            }

            var importerTypeParts = GetFilePathParts(importerType);
            using var importerTypePartsEnumerator = importerTypeParts!.Take(importerTypeParts.Length - 1).GetEnumerator();
            var importingTypeParts = GetFilePathParts(importingType);
            using var importingTypePartsEnumerator = importingTypeParts!.Take(importingTypeParts.Length - 1).GetEnumerator();

            var pathSegmentsToImport = new List<string>(5);
            var upperLevelPartsCount = 0;
            var isImportFromThisFolder = false;

            // TODO - simplify this algorithm.
            while (true)
            {
                var importerHasSegment = importerTypePartsEnumerator.MoveNext();
                var importingHasSegment = importingTypePartsEnumerator.MoveNext();

                switch (importingHasSegment)
                {
                    case true when importerHasSegment:
                    {
                        if (importerTypePartsEnumerator.Current == importingTypePartsEnumerator.Current)
                            continue;
                        pathSegmentsToImport.Add(importingTypePartsEnumerator.Current);
                        upperLevelPartsCount++;
                        while (importingTypePartsEnumerator.MoveNext())
                            pathSegmentsToImport.Add(importingTypePartsEnumerator.Current);
                        while (importerTypePartsEnumerator.MoveNext())
                            upperLevelPartsCount++;
                        break;
                    }
                    case true:
                    {
                        isImportFromThisFolder = true;
                        pathSegmentsToImport.Add(importingTypePartsEnumerator.Current);
                        while (importingTypePartsEnumerator.MoveNext())
                            pathSegmentsToImport.Add(importingTypePartsEnumerator.Current);
                        break;
                    }
                    default:
                    {
                        if (importerHasSegment)
                        {
                            upperLevelPartsCount++;
                            while (importerTypePartsEnumerator.MoveNext())
                                upperLevelPartsCount++;
                        }
                        else
                        {
                            isImportFromThisFolder = true;
                        }

                        break;
                    }
                }

                break;
            }

            if (upperLevelPartsCount > 0)
            {
                pathSegmentsToImport = Enumerable.Range(1, upperLevelPartsCount)
                    .Select(x => "..")
                    .Concat(pathSegmentsToImport)
                    .ToList();
            }

            pathSegmentsToImport.Add(GetFileName(importingType));

            var path = isImportFromThisFolder ? "./" : string.Empty;
            path += string.Join("/", pathSegmentsToImport.ToArray());

            return $"import {{ {GenerateName(importingType)} }} from '{path}'";
        }
    }
}