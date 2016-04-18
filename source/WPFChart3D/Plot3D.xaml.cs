namespace WPFChart3D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using WPFTools3D;

    /// <summary>
    /// Interaction logic for Plot3D.xaml
    /// </summary>
    public partial class Plot3D : UserControl
    {
        #region Constants

        static readonly Double SQRT_TWO = Math.Sqrt(2);

        #endregion

        #region Fields

        readonly TransformMatrix _transformMatrix;
        readonly ViewportRect _selectRect;
        readonly ScreenSpaceLines3D _wireframes;
        readonly ScreenSpaceLines3D _axis;
        readonly Model3D _model3d;
        readonly ScaleTransform3D _scale;
        readonly AxisAngleRotation3D _rotate;

        Vector3D _previousPosition3D;
        Point _origin;
        Chart3D _3dChart;
        Double _xMin;
        Double _xMax;
        Double _yMin;
        Double _yMax;
        Double _zMin;
        Double _zMax;

        #endregion

        #region ctor

        public Plot3D()
        {
            _wireframes = new ScreenSpaceLines3D
            {
                Color = Colors.SteelBlue
            };

            _axis = new ScreenSpaceLines3D
            {
                Color = Colors.Black,
                Thickness = 0.5
            };

            _model3d = new Model3D();
            _transformMatrix = new TransformMatrix();
            _selectRect = new ViewportRect();

            InitializeComponent();
            var tg = TrackBall.Transform as Transform3DGroup;

            if (tg != null && tg.Children.Count == 2)
            {
                _scale = (ScaleTransform3D)tg.Children[0];
                _rotate = (AxisAngleRotation3D)((RotateTransform3D)tg.Children[1]).Rotation;

                if (TrackBall.Viewport3D != null && TrackBall.Viewport3D.Camera != null)
                {
                    if (TrackBall.Viewport3D.Camera.IsFrozen)
                    {
                        TrackBall.Viewport3D.Camera = TrackBall.Viewport3D.Camera.Clone();
                    }

                    if (TrackBall.Viewport3D.Camera.Transform != tg)
                    {
                        TrackBall.Viewport3D.Camera.Transform = tg;
                    }
                }
            }

            MainViewport.Children.Add(_wireframes);
            MainViewport.Children.Add(_axis);

            TrackBall.IsManipulationEnabled = true;
            TrackBall.ManipulationDelta += TouchDelta;
            TrackBall.ManipulationStarted += TouchStarted;
            TrackBall.PreviewMouseWheel += MouseWheelRotated;
        }

        #endregion

        #region Mouse Wheel

        void MouseWheelRotated(Object sender, MouseWheelEventArgs e)
        {
            //Am I using some magic number here or is "120" for the
            //standard delta for the mouse wheel too high for usual
            //mice? My awesome razer salmosa has 120...
            if (e.Delta > 0)
            {
                Zoom(e.Delta * 0.01);
            }
            else if (e.Delta < 0)
            {
                Zoom(-100.0 / e.Delta);
            }
        }

        #endregion

        #region Touch Events

        void TouchStarted(Object sender, ManipulationStartedEventArgs e)
        {
            _origin = e.ManipulationOrigin;
            _previousPosition3D = ProjectToTrackball(MainViewport.ActualWidth, MainViewport.ActualHeight, _origin);
        }

        void TouchDelta(Object sender, ManipulationDeltaEventArgs e)
        {
            Zoom(SQRT_TWO / e.DeltaManipulation.Scale.Length);
            Rotate(e.DeltaManipulation.Translation);
        }

        #endregion

        #region Dependency Properties

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(Plot3D), new PropertyMetadata(OnTitleChanged));

        static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            control.TitleBlock.Text = (String)e.NewValue;
        }

        public Boolean IsAxisShown
        {
            get { return (Boolean)GetValue(IsAxisShownProperty); }
            set { SetValue(IsAxisShownProperty, value); }
        }

        public static readonly DependencyProperty IsAxisShownProperty =
            DependencyProperty.Register("IsAxisShown", typeof(Boolean), typeof(Plot3D), new PropertyMetadata(OnAxisShownChanged));

        static void OnAxisShownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var shown = (Boolean)e.NewValue;
            control._axis.Points.Clear();

            if (shown)
            {
                control.ShowAxis();
            }
        }

        public Double ScaleX
        {
            get { return (Double)GetValue(ScaleXProperty); }
            set { SetValue(ScaleXProperty, value); }
        }

        public static readonly DependencyProperty ScaleXProperty =
            DependencyProperty.Register("ScaleX", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnScaleXChanged));

        static void OnScaleXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            control._scale.ScaleX = value;
        }

        public Double ScaleY
        {
            get { return (Double)GetValue(ScaleYProperty); }
            set { SetValue(ScaleYProperty, value); }
        }

        public static readonly DependencyProperty ScaleYProperty =
            DependencyProperty.Register("ScaleY", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnScaleYChanged));

        static void OnScaleYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            control._scale.ScaleY = value;
        }

        public Double ScaleZ
        {
            get { return (Double)GetValue(ScaleZProperty); }
            set { SetValue(ScaleZProperty, value); }
        }

        public static readonly DependencyProperty ScaleZProperty =
            DependencyProperty.Register("ScaleZ", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnScaleZChanged));

        static void OnScaleZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            control._scale.ScaleZ = value;
        }

        public Double RotateX
        {
            get { return (Double)GetValue(RotateXProperty); }
            set { SetValue(RotateXProperty, value); }
        }

        public static readonly DependencyProperty RotateXProperty =
            DependencyProperty.Register("RotateX", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnRotateXChanged));

        static void OnRotateXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            var axis = control._rotate.Axis;
            control._rotate.Axis = new Vector3D(value, axis.Y, axis.Z);
        }

        public Double RotateY
        {
            get { return (Double)GetValue(RotateYProperty); }
            set { SetValue(RotateYProperty, value); }
        }

        public static readonly DependencyProperty RotateYProperty =
            DependencyProperty.Register("RotateY", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnRotateYChanged));

        static void OnRotateYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            var axis = control._rotate.Axis;
            control._rotate.Axis = new Vector3D(axis.X, value, axis.Z);
        }

        public Double RotateZ
        {
            get { return (Double)GetValue(RotateZProperty); }
            set { SetValue(RotateZProperty, value); }
        }

        public static readonly DependencyProperty RotateZProperty =
            DependencyProperty.Register("RotateZ", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnRotateZChanged));

        static void OnRotateZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            var axis = control._rotate.Axis;
            control._rotate.Axis = new Vector3D(axis.X, axis.Y, value);
        }

        public Double RotateAngle
        {
            get { return (Double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }

        public static readonly DependencyProperty RotateAngleProperty =
            DependencyProperty.Register("RotateAngle", typeof(Double), typeof(Plot3D), new PropertyMetadata(OnRotateAngleChanged));

        static void OnRotateAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            var value = (Double)e.NewValue;
            control._rotate.Angle = value;
        }

        public Object Model
        {
            get { return (Object)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(Object), typeof(Plot3D), new PropertyMetadata(OnModelChanged));

        static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //TODO see TODO region
            throw new NotImplementedException();
        }

        #endregion

        #region TODO

        static void OnWireframeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            control._wireframes.Thickness = (Double)e.NewValue;
        }

        static void OnWireframeColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Plot3D)d;
            control._wireframes.Color = (Color)e.NewValue;
        }

        //_control.CreateSurface(plot.Nx, plot.Ny);
        //LOOP {
        //_control.SetVertex(i, (Single)_plot[0, i].X, (Single)_plot[0, i].Y, (Single)_plot[0, i].Z);
        //}
        //_control.SetView((Single)_plot.MinX, (Single)_plot.MaxX, (Single)_plot.MinY, (Single)_plot.MaxY, (Single)_plot.MinZ, (Single)_plot.MaxZ);
        //_control.SetColors((Single)_plot.MinZ, (Single)_plot.MaxZ, GenerateColors(_plot.ColorPalette, 20));
        //_control.Publish();
        //_control.SetWireframe(_plot.IsMesh);
        //_control.SetSurface(_plot.IsSurf);

        //If Model NAME == LinePlotModel
        //foreach (var series in Series)
        //{
        //for (var j = 2; j < series.Count; j += 2)
        //{
        //    var x1 = x[j - 2];
        //    var x2 = x[j - 1];
        //    var x3 = x[j - 0];
        //    var y1 = y[j - 2];
        //    var y2 = y[j - 1];
        //    var y3 = y[j - 0];
        //    var z1 = z[j - 2];
        //    var z2 = z[j - 1];
        //    var z3 = z[j - 0];
        //    _control.AddWireframeVertex(x1, y1, z1, x2, y2, z2, x3, y3, z3);
        //}
        //}
        //_control.SetTransformWireframe();

        #endregion

        #region Plotting

        public void CreateSurface(Int32 nx, Int32 ny)
        {
            var surface = new UniformSurfaceChart3D();
            _3dChart = surface;
            _3dChart.SetDataNo(nx * ny);
            surface.SetGrid(nx, ny);
        }

        public void SetVertex(Int32 i, Single x, Single y, Single z)
        {
            _3dChart[i] = new Vertex3D
            {
                X = x,
                Y = y,
                Z = z
            };
        }

        public void SetView(Single xmin, Single xmax, Single ymin, Single ymax, Single zmin, Single zmax)
        {
            _3dChart.SetDataRange(xmin, xmax, ymin, ymax, zmin, zmax);
        }

        public void SetColors(Single zmin, Single zmax)
        {
            var nv = _3dChart.GetDataNo();

            for (var i = 0; i < nv; i++)
            {
                var vert = _3dChart[i];
                var h = (vert.Z - zmin) / (zmax - zmin);
                var color = TextureMapping.PseudoColor(h);
                _3dChart[i].Color = color;
            }
        }

        public void SetColors(Single zmin, Single zmax, Color[] colors)
        {
            var nv = _3dChart.GetDataNo();

            for (var i = 0; i < nv; i++)
            {
                var vert = _3dChart[i];
                var h = (vert.Z - zmin) / (zmax - zmin);
                var index = (Int32)Math.Min(Math.Floor(h * colors.Length), colors.Length - 1);
                _3dChart[i].Color = colors[index];
            }
        }

        public void Publish()
        {
            var meshs = ((UniformSurfaceChart3D)_3dChart).GetMeshes();

            var backMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            _model3d.UpdateModel(meshs, backMaterial);

            var xMin = _3dChart.XMin();
            var xMax = _3dChart.XMax();
            var yMin = _3dChart.YMin();
            var yMax = _3dChart.YMax();
            var zMin = _3dChart.ZMin();
            var zMax = _3dChart.ZMax();

            Project(xMin, xMax, yMin, yMax, zMin, zMax);
            TransformChart();
        }

        public void AddWireframeVertex(Double x1, Double y1, Double z1, Double x2, Double y2, Double z2, Double x3, Double y3, Double z3)
        {
            var p1 = new Point3D(x1, y1, z1);
            var p2 = new Point3D(x2, y2, z2);
            var p3 = new Point3D(x3, y3, z3);
            var space = _wireframes;

            space.Points.Add(p1);
            space.Points.Add(p2);
            space.Points.Add(p2);
            space.Points.Add(p3);
            space.Points.Add(p3);
            space.Points.Add(p1);
        }

        void Project(Double xmin, Double xmax, Double ymin, Double ymax, Double zmin, Double zmax)
        {
            _xMin = xmin;
            _xMax = xmax;
            _yMin = ymin;
            _yMax = ymax;
            _zMin = zmin;
            _zMax = zmax;

            _transformMatrix.CalculateProjectionMatrix(xmin, xmax, ymin, ymax, zmin, zmax, 0.5);
        }

        public void SetTransformWireframe(Double xmin, Double xmax, Double ymin, Double ymax, Double zmin, Double zmax)
        {
            Project(xmin, xmax, ymin, ymax, zmin, zmax);
            _wireframes.Transform = new MatrixTransform3D(_transformMatrix.TotalMatrix);
        }

        public void SetWireframe(Boolean status)
        {
            if (status)
            {
                _wireframes.MakeWireframe(_model3d.Content);
            }
            else
            {
                _wireframes.Points.Clear();
            }
        }

        public void SetSurface(Boolean status)
        {
            if (status)
            {
                if (!MainViewport.Children.Contains(_model3d))
                {
                    MainViewport.Children.Add(_model3d);
                }
            }
            else
            {
                MainViewport.Children.Remove(_model3d);
            }
        }

        #endregion

        #region Helper

        void ShowAxis()
        {
            var geo = new GeometryModel3D();
            var cube = new MeshGeometry3D();

            var p1 = new Point3D(_xMin, _yMin, _zMin);
            var p2 = new Point3D(_xMax, _yMin, _zMin);
            var p5 = new Point3D(_xMax, _yMax, _zMin);
            var p3 = new Point3D(_xMin, _yMax, _zMin);

            var p4 = new Point3D(_xMin, _yMin, _zMax);
            var p6 = new Point3D(_xMax, _yMin, _zMax);
            var p8 = new Point3D(_xMin, _yMax, _zMax);
            var p7 = new Point3D(_xMax, _yMax, _zMax);

            cube.Positions.Add(p1);//0 - 0 0 0
            cube.Positions.Add(p2);//1 - 1 0 0
            cube.Positions.Add(p3);//2 - 0 1 0
            cube.Positions.Add(p5);//3 - 1 1 0

            cube.Positions.Add(p4);//4 - 0 0 1
            cube.Positions.Add(p6);//5 - 1 0 1
            cube.Positions.Add(p8);//6 - 0 1 1
            cube.Positions.Add(p7);//7 - 1 1 1

            var zh = (_zMax - _zMin) * 0.5;
            var xh = (_xMax - _xMin) * 0.5;
            var yh = (_yMax - _yMin) * 0.5;

            var pc1 = new Point3D(_xMin, _yMin, zh);
            var pc2 = new Point3D(_xMax, _yMin, zh);
            var pc5 = new Point3D(_xMax, _yMax, zh);
            var pc3 = new Point3D(_xMin, _yMax, zh);

            var pc4 = new Point3D(xh, _yMin, _zMax);
            var pc6 = new Point3D(_xMax, yh, _zMax);
            var pc8 = new Point3D(_xMin, yh, _zMax);
            var pc7 = new Point3D(xh, _yMax, _zMax);

            var pc9 = new Point3D(xh, _yMin, _zMin);
            var pc10 = new Point3D(_xMax, yh, _zMin);
            var pc11 = new Point3D(_xMin, yh, _zMin);
            var pc12 = new Point3D(xh, _yMax, _zMin);

            cube.Positions.Add(pc1);//8
            cube.Positions.Add(pc2);//9
            cube.Positions.Add(pc3);//10
            cube.Positions.Add(pc5);//11

            cube.Positions.Add(pc4);//12
            cube.Positions.Add(pc6);//13
            cube.Positions.Add(pc8);//14
            cube.Positions.Add(pc7);//15

            cube.Positions.Add(pc9);//16
            cube.Positions.Add(pc10);//17
            cube.Positions.Add(pc11);//18
            cube.Positions.Add(pc12);//19

            cube.TriangleIndices.Add(0);
            cube.TriangleIndices.Add(4);
            cube.TriangleIndices.Add(8);

            cube.TriangleIndices.Add(1);
            cube.TriangleIndices.Add(5);
            cube.TriangleIndices.Add(9);

            cube.TriangleIndices.Add(2);
            cube.TriangleIndices.Add(6);
            cube.TriangleIndices.Add(10);

            cube.TriangleIndices.Add(3);
            cube.TriangleIndices.Add(7);
            cube.TriangleIndices.Add(11);

            cube.TriangleIndices.Add(4);
            cube.TriangleIndices.Add(5);
            cube.TriangleIndices.Add(12);

            cube.TriangleIndices.Add(4);
            cube.TriangleIndices.Add(6);
            cube.TriangleIndices.Add(14);

            cube.TriangleIndices.Add(5);
            cube.TriangleIndices.Add(7);
            cube.TriangleIndices.Add(13);

            cube.TriangleIndices.Add(6);
            cube.TriangleIndices.Add(7);
            cube.TriangleIndices.Add(15);

            cube.TriangleIndices.Add(0);
            cube.TriangleIndices.Add(1);
            cube.TriangleIndices.Add(16);

            cube.TriangleIndices.Add(0);
            cube.TriangleIndices.Add(2);
            cube.TriangleIndices.Add(18);

            cube.TriangleIndices.Add(1);
            cube.TriangleIndices.Add(3);
            cube.TriangleIndices.Add(17);

            cube.TriangleIndices.Add(2);
            cube.TriangleIndices.Add(3);
            cube.TriangleIndices.Add(19);

            geo.Geometry = cube;
            _axis.MakeWireframe(geo);
            _axis.Transform = new MatrixTransform3D(_transformMatrix.TotalMatrix);
        }

        void Zoom(Double scale)
        {
            _scale.ScaleX *= scale;
            _scale.ScaleY *= scale;
            _scale.ScaleZ *= scale;
        }

        void Rotate(Vector translation)
        {
            _origin += translation;
            var pos = ProjectToTrackball(MainViewport.ActualWidth, MainViewport.ActualHeight, _origin);
            var axis = Vector3D.CrossProduct(_previousPosition3D, pos);

            if (axis.Length != 0)
            {
                var angle = Vector3D.AngleBetween(_previousPosition3D, pos);
                var delta = new Quaternion(axis, -angle);
                var q = new Quaternion(_rotate.Axis, _rotate.Angle) * delta;
                _rotate.Axis = q.Axis;
                _rotate.Angle = q.Angle;
                _previousPosition3D = pos;
            }
        }

        Vector3D ProjectToTrackball(Double width, Double height, Point point)
        {
            // Scale so bounds map to [0,0] - [2,2]
            var x = 2.0 * point.X / width;
            var y = 2.0 * point.Y / height;
            x = x - 1.0;
            y = 1.0 - y;
            var z2 = 1.0 - x * x - y * y;
            var z = z2 > 0.0 ? Math.Sqrt(z2) : 0.0;
            return new Vector3D(x, y, z);
        }

        void TransformChart()
        {
            if (_model3d.Content != null)
            {
                var group = _model3d.Content.Transform as Transform3DGroup;

                if (group != null)
                {
                    group.Children.Clear();
                    group.Children.Add(new MatrixTransform3D(_transformMatrix.TotalMatrix));
                }
            }
        }

        #endregion
    }
}
