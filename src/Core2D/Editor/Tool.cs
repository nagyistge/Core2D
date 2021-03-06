﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Core2D.Editor
{
    /// <summary>
    /// Specifies editor tools.
    /// </summary>
    public enum Tool
    {
        /// <summary>
        /// None tool.
        /// </summary>
        None,

        /// <summary>
        /// Selection tool.
        /// </summary>
        Selection,

        /// <summary>
        /// Point tool.
        /// </summary>
        Point,

        /// <summary>
        /// Line tool.
        /// </summary>
        Line,

        /// <summary>
        /// Arc tool.
        /// </summary>
        Arc,

        /// <summary>
        /// Cubic bezier tool.
        /// </summary>
        CubicBezier,

        /// <summary>
        /// Quadratic bezier tool.
        /// </summary>
        QuadraticBezier,

        /// <summary>
        /// Path tool.
        /// </summary>
        Path,

        /// <summary>
        /// Rectangle tool.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Ellipse tool.
        /// </summary>
        Ellipse,

        /// <summary>
        /// Text tool
        /// </summary>
        Text,

        /// <summary>
        /// Image tool.
        /// </summary>
        Image
    }
}
