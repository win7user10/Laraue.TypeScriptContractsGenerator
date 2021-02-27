﻿using Laraue.CodeTranslation.Abstractions.Metadata;

namespace Laraue.CodeTranslation.Typed.Abstractions.Code
{
	public interface ITypeCodeGenerator : CodeTranslation.Abstractions.Code.ITypeCodeGenerator
	{
		/// <summary>
		/// Get translated type of a <see cref="ITypeMetadata">type</see>.
		/// </summary>
		/// <param name="metadata"></param>
		/// <returns></returns>
		string GetType(ITypeMetadata metadata);
	}
}