﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Core2D
{
    /// <summary>
    /// Helper class for <see cref="Tool.Text"/> editor.
    /// </summary>
    public class ToolText : ToolBase
    {
        private Editor _editor;
        private State _currentState = State.None;
        private XText _shape;
        private XPoint _topLeftHelperPoint;
        private XPoint _bottomRightHelperPoint;

        /// <summary>
        /// Initialize new instance of <see cref="ToolText"/> class.
        /// </summary>
        /// <param name="editor">The current <see cref="Editor"/> object.</param>
        public ToolText(Editor editor)
            : base()
        {
            _editor = editor;
        }

        /// <summary>
        /// Try to connect <see cref="XText.TopLeft"/> point at specified location.
        /// </summary>
        /// <param name="text">The text object.</param>
        /// <param name="x">The X coordinate of point.</param>
        /// <param name="y">The Y coordinate of point.</param>
        public void TryToConnectTopLeft(XText text, double x, double y)
        {
            var result = ShapeBounds.HitTest(_editor.Project.CurrentContainer, new Vector2(x, y), _editor.Project.Options.HitTreshold);
            if (result != null && result is XPoint)
            {
                text.TopLeft = result as XPoint;
            }
        }

        /// <summary>
        /// Try to connect <see cref="XText.BottomRight"/> point at specified location.
        /// </summary>
        /// <param name="text">The text object.</param>
        /// <param name="x">The X coordinate of point.</param>
        /// <param name="y">The Y coordinate of point.</param>
        public void TryToConnectBottomRight(XText text, double x, double y)
        {
            var result = ShapeBounds.HitTest(_editor.Project.CurrentContainer, new Vector2(x, y), _editor.Project.Options.HitTreshold);
            if (result != null && result is XPoint)
            {
                text.BottomRight = result as XPoint;
            }
        }

        /// <inheritdoc/>
        public override void LeftDown(double x, double y)
        {
            base.LeftDown(x, y);

            double sx = _editor.Project.Options.SnapToGrid ? Editor.Snap(x, _editor.Project.Options.SnapX) : x;
            double sy = _editor.Project.Options.SnapToGrid ? Editor.Snap(y, _editor.Project.Options.SnapY) : y;
            switch (_currentState)
            {
                case State.None:
                    {
                        _shape = XText.Create(
                            sx, sy,
                            _editor.Project.CurrentStyleLibrary.Selected,
                            _editor.Project.Options.PointShape,
                            "Text",
                            _editor.Project.Options.DefaultIsStroked);
                        if (_editor.Project.Options.TryToConnect)
                        {
                            TryToConnectTopLeft(_shape as XText, sx, sy);
                        }
                        _editor.Project.CurrentContainer.WorkingLayer.Shapes = _editor.Project.CurrentContainer.WorkingLayer.Shapes.Add(_shape);
                        _editor.Project.CurrentContainer.WorkingLayer.Invalidate();
                        ToStateOne();
                        Move(_shape);
                        _editor.Project.CurrentContainer.HelperLayer.Invalidate();
                        _currentState = State.One;
                        _editor.CancelAvailable = true;
                    }
                    break;
                case State.One:
                    {
                        var text = _shape as XText;
                        if (text != null)
                        {
                            text.BottomRight.X = sx;
                            text.BottomRight.Y = sy;
                            if (_editor.Project.Options.TryToConnect)
                            {
                                TryToConnectBottomRight(_shape as XText, sx, sy);
                            }
                            _editor.Project.CurrentContainer.WorkingLayer.Shapes = _editor.Project.CurrentContainer.WorkingLayer.Shapes.Remove(_shape);
                            Remove();
                            Finalize(_shape);
                            _editor.Project.AddShape(_shape);
                            _currentState = State.None;
                            _editor.CancelAvailable = false;
                        }
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        public override void RightDown(double x, double y)
        {
            base.RightDown(x, y);

            switch (_currentState)
            {
                case State.None:
                    break;
                case State.One:
                    {
                        _editor.Project.CurrentContainer.WorkingLayer.Shapes = _editor.Project.CurrentContainer.WorkingLayer.Shapes.Remove(_shape);
                        _editor.Project.CurrentContainer.WorkingLayer.Invalidate();
                        Remove();
                        _editor.Project.CurrentContainer.HelperLayer.Invalidate();
                        _currentState = State.None;
                        _editor.CancelAvailable = false;
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        public override void Move(double x, double y)
        {
            base.Move(x, y);

            double sx = _editor.Project.Options.SnapToGrid ? Editor.Snap(x, _editor.Project.Options.SnapX) : x;
            double sy = _editor.Project.Options.SnapToGrid ? Editor.Snap(y, _editor.Project.Options.SnapY) : y;
            switch (_currentState)
            {
                case State.None:
                    if (_editor.Project.Options.TryToConnect)
                    {
                        _editor.TryToHoverShape(sx, sy);
                    }
                    break;
                case State.One:
                    {
                        var text = _shape as XText;
                        if (text != null)
                        {
                            if (_editor.Project.Options.TryToConnect)
                            {
                                _editor.TryToHoverShape(sx, sy);
                            }
                            text.BottomRight.X = sx;
                            text.BottomRight.Y = sy;
                            _editor.Project.CurrentContainer.WorkingLayer.Invalidate();
                            Move(_shape);
                            _editor.Project.CurrentContainer.HelperLayer.Invalidate();
                        }
                    }
                    break;
            }
        }

        /// <inheritdoc/>
        public override void ToStateOne()
        {
            base.ToStateOne();

            _topLeftHelperPoint = XPoint.Create(0, 0, _editor.Project.Options.PointShape);
            _editor.Project.CurrentContainer.HelperLayer.Shapes = _editor.Project.CurrentContainer.HelperLayer.Shapes.Add(_topLeftHelperPoint);
            _bottomRightHelperPoint = XPoint.Create(0, 0, _editor.Project.Options.PointShape);
            _editor.Project.CurrentContainer.HelperLayer.Shapes = _editor.Project.CurrentContainer.HelperLayer.Shapes.Add(_bottomRightHelperPoint);
        }

        /// <inheritdoc/>
        public override void Move(BaseShape shape)
        {
            base.Move(shape);

            if (_topLeftHelperPoint != null)
            {
                _topLeftHelperPoint.X = _shape.TopLeft.X;
                _topLeftHelperPoint.Y = _shape.TopLeft.Y;
            }

            if (_bottomRightHelperPoint != null)
            {
                _bottomRightHelperPoint.X = _shape.BottomRight.X;
                _bottomRightHelperPoint.Y = _shape.BottomRight.Y;
            }
        }

        /// <inheritdoc/>
        public override void Remove()
        {
            base.Remove();

            if (_topLeftHelperPoint != null)
            {
                _editor.Project.CurrentContainer.HelperLayer.Shapes = _editor.Project.CurrentContainer.HelperLayer.Shapes.Remove(_topLeftHelperPoint);
                _topLeftHelperPoint = null;
            }

            if (_bottomRightHelperPoint != null)
            {
                _editor.Project.CurrentContainer.HelperLayer.Shapes = _editor.Project.CurrentContainer.HelperLayer.Shapes.Remove(_bottomRightHelperPoint);
                _bottomRightHelperPoint = null;
            }
        }
    }
}