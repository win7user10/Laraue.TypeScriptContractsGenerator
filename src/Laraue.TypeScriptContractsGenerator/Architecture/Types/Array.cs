﻿using System.Collections.Generic;
using Laraue.CodeTranslation.Abstractions.Metadata;
using Laraue.CodeTranslation.Abstractions.Output;

namespace Laraue.TypeScriptContractsGenerator.Architecture.Types
{
	public class Array : ReferenceOutputType
	{
		public Array(OutputTypeName name, IEnumerable<TypeMetadata> usedTypes)
			: base(name, usedTypes)
		{
		}
	}
}
