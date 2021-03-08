﻿using System.Collections.Generic;
using Laraue.CodeTranslation.Abstractions.Output;

namespace Laraue.TypeScriptContractsGenerator.Architecture.Types
{
	public class Array : ReferenceOutputType
	{
		/// <inheritdoc />
		public override OutputTypeName Name { get; }

		/// <inheritdoc />
		public override IEnumerable<OutputType> UsedTypes { get; }

		public Array(OutputTypeName name, IEnumerable<OutputType> usedTypes)
		{
			Name = GetName(name);
			UsedTypes = usedTypes;
		}

		public OutputTypeName GetName(OutputTypeName sourceType)
		{
			return new (sourceType, sourceType.GenericNames, true);
		}
	}
}
