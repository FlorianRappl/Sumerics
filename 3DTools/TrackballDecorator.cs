//---------------------------------------------------------------------------
//
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Limited Permissive License.
// See http://www.microsoft.com/resources/sharedsource/licensingbasics/limitedpermissivelicense.mspx
// All other rights reserved.
//
// This file is part of the 3D Tools for Windows Presentation Foundation
// project.  For more information, see:
// 
// http://CodePlex.com/Wiki/View.aspx?ProjectName=3DTools
//
//---------------------------------------------------------------------------

namespace WPFTools3D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class TrackballDecorator : Viewport3DDecorator
    {
        readonly Transform3DGroup _transform = new Transform3DGroup();
        readonly ScaleTransform3D _scale = new ScaleTransform3D();
        readonly AxisAngleRotation3D _rotation = new AxisAngleRotation3D();

        Vector3D _previousPosition3D;
        Point _previousPosition2D;
        Border _eventSource;        

        public TrackballDecorator()
        {
            _previousPosition3D = new Vector3D(0, 0, 1);
            // the transform that will be applied to the viewport 3d's camera
            _transform.Children.Add(_scale);
            _transform.Children.Add(new RotateTransform3D(_rotation));

            // used so that we always get events while activity occurs within
            // the viewport3D
            _eventSource = new Border();
            _eventSource.Background = Brushes.Transparent;
            
            PreViewportChildren.Add(_eventSource);
        }

        /// <summary>
        ///     A transform to move the camera or scene to the trackball's
        ///     current orientation and scale.
        /// </summary>
        public Transform3D Transform
        {
            get { return _transform; }
        }

        #region Event Handling

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            _previousPosition2D = e.GetPosition(this);
            _previousPosition3D = ProjectToTrackball(ActualWidth,
                                                     ActualHeight,
                                                     _previousPosition2D);
            if (Mouse.Captured == null)
            {
                Mouse.Capture(this, CaptureMode.Element);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (IsMouseCaptured)
            {
                Mouse.Capture(this, CaptureMode.None);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (IsMouseCaptured)
            {
                Point currentPosition = e.GetPosition(this);

                // avoid any zero axis conditions
                if (currentPosition == _previousPosition2D) return;

                // Prefer tracking to zooming if both buttons are pressed.
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Track(currentPosition);
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    Zoom(currentPosition);
                }

                _previousPosition2D = currentPosition;

                Viewport3D viewport3D = this.Viewport3D;
                if (viewport3D != null)
                {
                    if (viewport3D.Camera != null)
                    {
                        if (viewport3D.Camera.IsFrozen)
                        {
                            viewport3D.Camera = viewport3D.Camera.Clone();
                        }

                        if (viewport3D.Camera.Transform != _transform)
                        {
                            viewport3D.Camera.Transform = _transform;
                        }
                    }
                }
            }
        }

        #endregion Event Handling

        private void Track(Point currentPosition)
        {
            var currentPosition3D = ProjectToTrackball(
                ActualWidth, ActualHeight, currentPosition);

            var axis = Vector3D.CrossProduct(_previousPosition3D, currentPosition3D);
            var angle = Vector3D.AngleBetween(_previousPosition3D, currentPosition3D);

            // quaterion will throw if this happens - sometimes we can get 3D positions that
            // are very similar, so we avoid the throw by doing this check and just ignoring
            // the event 
            if (axis.Length != 0)
            {
                var delta = new Quaternion(axis, -angle);

                // Get the current orientantion from the RotateTransform3D
                var r = _rotation;
                var q = new Quaternion(_rotation.Axis, _rotation.Angle);

                // Compose the delta with the previous orientation
                q *= delta;

                // Write the new orientation back to the Rotation3D
                _rotation.Axis = q.Axis;
                _rotation.Angle = q.Angle;

                _previousPosition3D = currentPosition3D;
            }
        }

        private Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            var x = point.X / (width * 0.5);    // Scale so bounds map to [0,0] - [2,2]
            var y = point.Y / (height * 0.5);

            x = x - 1;                           // Translate 0,0 to the center
            y = 1 - y;                           // Flip so +Y is up instead of down

            var z2 = 1 - x * x - y * y;       // z^2 = 1 - x^2 - y^2
            var z = z2 > 0 ? Math.Sqrt(z2) : 0.0;

            return new Vector3D(x, y, z);
        }

        private void Zoom(Point currentPosition)
        {
            var yDelta = currentPosition.Y - _previousPosition2D.Y;
            var scale = Math.Exp(yDelta / 100);    // e^(yDelta/100) is fairly arbitrary.

            _scale.ScaleX *= scale;
            _scale.ScaleY *= scale;
            _scale.ScaleZ *= scale;
        }
    }
}
