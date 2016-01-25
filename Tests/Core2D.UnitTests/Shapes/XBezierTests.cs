﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace Core2D.UnitTests
{
    public class XBezierTests
    {
        [Fact]
        [Trait("Core2D", "Shapes")]
        public void Inherits_From_BaseShape()
        {
            var target = new XBezier();
            Assert.True(target is BaseShape);
        }
    }
}
