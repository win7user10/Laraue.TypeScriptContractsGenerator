﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Laraue.TypeScriptContractsGenerator.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }
    }
}
