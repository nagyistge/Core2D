﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Core2D.Style;
using Portable.Xaml.ComponentModel;
using System;
using System.Globalization;

namespace Serializer.Xaml.Converters
{
    /// <summary>
    /// Defines <see cref="ArgbColor"/> type converter.
    /// </summary>
    internal class ArgbColorTypeConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return ArgbColor.Parse((string)value);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var color = value as ArgbColor;
            if (color != null)
            {
                return ArgbColor.ToHtml(color);
            }
            throw new NotSupportedException();
        }
    }
}
