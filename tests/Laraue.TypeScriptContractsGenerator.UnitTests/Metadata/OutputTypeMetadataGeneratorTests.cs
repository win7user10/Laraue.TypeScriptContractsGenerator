﻿using System;
using Laraue.CodeTranslation.Abstractions.Metadata;
using Laraue.CodeTranslation.Abstractions.Output;
using Laraue.CodeTranslation.Abstractions.Output.Metadata;
using Laraue.TypeScriptContractsGenerator.Architecture;
using Laraue.TypeScriptContractsGenerator.Architecture.Types;
using Xunit;
using String = Laraue.TypeScriptContractsGenerator.Architecture.Types.String;

namespace Laraue.TypeScriptContractsGenerator.UnitTests.Metadata
{
	public class OutputTypeMetadataGeneratorTests
	{
		private readonly IOutputTypeMetadataGenerator _generator = new TypeScriptOutputTypeMetadataGenerator();

		[Theory]
		[InlineData(typeof(int), typeof(Number))]
		[InlineData(typeof(double), typeof(Number))]
		[InlineData(typeof(decimal), typeof(Number))]
		[InlineData(typeof(float), typeof(Number))]
		[InlineData(typeof(long), typeof(Number))]
		[InlineData(typeof(short), typeof(Number))]
		[InlineData(typeof(string), typeof(String))]
		[InlineData(typeof(Guid), typeof(String))]
		[InlineData(typeof(MainClass), typeof(Class))]
		public void GenerateCommonTypes(Type clrType, Type tsType)
		{
			var metadata = _generator.Generate(new TypeMetadata { ClrType = clrType });
			Assert.Equal(tsType, metadata.OutputType.GetType());
		}
	}
}