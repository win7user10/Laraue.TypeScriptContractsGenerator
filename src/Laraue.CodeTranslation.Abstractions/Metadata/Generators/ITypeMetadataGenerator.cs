﻿using System;
using JetBrains.Annotations;

namespace Laraue.CodeTranslation.Abstractions.Metadata.Generators
{
	/// <summary>
	/// Class can generates <see cref="TypeMetadata"/> for <see cref="Type" />.
	/// </summary>
	public interface ITypeMetadataGenerator : IMetadataGenerator
	{
		/// <summary>
		/// Generates <see cref="TypeMetadata">metadata</see> for some <see cref="Type">Clr type</see>.
		/// </summary>
		/// <returns></returns>
		[NotNull]
		TypeMetadata GetMetadata(Type type);
	}
}