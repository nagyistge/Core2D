﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Core2D.Style;
using Xunit;

namespace Core2D.UnitTests
{
    public class ArrowStyleTests
    {
        [Fact]
        [Trait("Core2D.Style", "Style")]
        public void Inherits_From_ObservableObject()
        {
            var target = new ArrowStyle();
            Assert.True(target is BaseStyle);
        }
    }
}
