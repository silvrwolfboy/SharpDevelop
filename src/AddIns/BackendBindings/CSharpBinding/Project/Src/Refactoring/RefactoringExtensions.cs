﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.Core;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.SharpDevelop;
using CSharpBinding.Parser;

namespace CSharpBinding.Refactoring
{
	/// <summary>
	/// Some extensions related to refactoring features.
	/// </summary>
	public static class RefactoringExtensions
	{
		/// <summary>
		/// Checks whether a property is auto-implemented (has only "get; set;" definitions).
		/// </summary>
		/// <param name="property">Property to check</param>
		/// <returns>True if auto-implemented, false otherwise.</returns>
		public static bool IsAutoImplemented(this IProperty property)
		{
			if (property == null)
				throw new ArgumentNullException("property");
			
			if (property.IsAbstract)
				return false;
			
			return property.CanGet && !property.Getter.HasBody && property.CanSet && !property.Setter.HasBody;
		}
		
		/// <summary>
		/// Retrieves the declaration for the specified entity.
		/// Returns null if the entity is not defined in C# source code.
		/// </summary>
		public static EntityDeclaration GetDeclaration(this IEntity entity, out CSharpFullParseInformation parseInfo)
		{
			if (entity == null || string.IsNullOrEmpty(entity.Region.FileName)) {
				parseInfo = null;
				return null;
			}
			parseInfo = SD.ParserService.Parse(FileName.Create(entity.Region.FileName),
			                                   parentProject: entity.ParentAssembly.GetProject())
				as CSharpFullParseInformation;
			if (parseInfo == null)
				return null;
			return parseInfo.SyntaxTree.GetNodeAt<EntityDeclaration>(entity.Region.Begin);
		}
	}
}
