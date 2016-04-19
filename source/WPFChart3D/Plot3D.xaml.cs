namespace WPFChart3D
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;
    using WPFChart3D.Data;
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
                TrackBall.Viewport3D.Camera.Transform = tg;
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
            var control = (Plot3D)d;
            var value = e.NewValue;

            if (value is SurfacePlotModel)
            {
                control.Apply((SurfacePlotModel)value);
            }
            else if (value is LinePlotModel)
            {
                control.Apply((LinePlotModel)value);
            }
        }

        #endregion

        #region Apply Model

        void Apply(SurfacePlotModel model)
        {
            var nx = model.Nx;
            var ny = model.Ny;
            var surface = new UniformSurfaceChart3D();
            _3dChart = surface;
            _3dChart.SetDataNo(nx * ny);
            surface.SetGrid(nx, ny);
            _wireframes.Thickness = model.Data.Thickness;
            _wireframes.Color = model.Data.Color.Convert();

            var x = model.Data.Xs;
            var y = model.Data.Ys;
            var z = model.Data.Zs;
            var length = x.Length;

            for (int i = 0; i < length; i++)
            {
                _3dChart[i] = new Vertex3D
                {
                    X = (Single)x[i],
                    Y = (Single)y[i], 
                    Z = (Single)z[i]
                };
            }

            SetView(model.XAxis, model.YAxis, model.ZAxis);
            SetColors(model.ZAxis, model.Colors);

            var meshs = ((UniformSurfaceChart3D)_3dChart).GetMeshes();

            var backMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            _model3d.UpdateModel(meshs, backMaterial);

            var xMin = _3dChart.XMin();
            var xMax = _3dChart.XMax();
            var yMin = _3dChart.YMin();
            var yMax = _3dChart.YMax();
            var zMin = _3dChart.ZMin();
            var zMax = _3dChart.ZMax();

            Project(model.XAxis, model.YAxis, model.ZAxis);
            TransformChart();

            if (model.IsWireframeShown)
            {
                _wireframes.MakeWireframe(_model3d.Content);
            }
            else
            {
                _wireframes.Points.Clear();
            }

            if (!model.IsSurfaceShown)
            {
                MainViewport.Children.Remove(_model3d);
            }
            else if (!MainViewport.Children.Contains(_model3d))
            {
                MainViewport.Children.Add(_model3d);
            }
        }

        void Apply(LinePlotModel model)
        {
            foreach (var series in model.Series)
            {
                var x = series.Xs;
                var y = series.Ys;
                var z = series.Zs;
                var length = x.Length;
                _wireframes.Thickness = series.Thickness;
                _wireframes.Color = series.Color.Convert();

                for (var j = 2; j < length; j += 2)
                {
                    var p1 = new Point3D(x[j - 2], y[j - 2], z[j - 2]);
                    var p2 = new Point3D(x[j - 1], y[j - 1], z[j - 1]);
                    var p3 = new Point3D(x[j - 0], y[j - 0], z[j - 0]);

                    _wireframes.Points.Add(p1);
                    _wireframes.Points.Add(p2);
                    _wireframes.Points.Add(p2);
                    _wireframes.Points.Add(p3);
                    _wireframes.Points.Add(p3);
                    _wireframes.Points.Add(p1);
                }
            }

            Project(model.XAxes, model.YAxes, model.ZAxes);
            _wireframes.Transform = new MatrixTransform3D(_transformMatrix.TotalMatrix);
        }

        #endregion

        #region Helper

        void SetColors(Plot3dAxis axes, WpfColor[] colors)
        {
            var nv = _3dChart.GetDataNo();
            var offset = axes.Minimum;
            var norm = axes.Maximum - offset;

            for (var i = 0; i < nv; i++)
            {
                var vert = _3dChart[i];
                var h = (vert.Z - offset) / norm;
                var index = Math.Max(Math.Min((Int32)Math.Floor(h * colors.Length), colors.Length - 1), 0);
                _3dChart[i].Color = colors[index].Convert();
            }
        }

        void Project(Plot3dAxis xAxes, Plot3dAxis yAxes, Plot3dAxis zAxes)
        {
            _xMin = xAxes.Minimum;
            _xMax = xAxes.Maximum;
            _yMin = yAxes.Minimum;
            _yMax = yAxes.Maximum;
            _zMin = zAxes.Minimum;
            _zMax = zAxes.Maximum;

            _transformMatrix.CalculateProjectionMatrix(_xMin, _xMax, _yMin, _yMax, _zMin, _zMax, 0.5);
        }

        void SetView(Plot3dAxis xAxes, Plot3dAxis yAxes, Plot3dAxis zAxes)
        {
            _3dChart.SetDataRange((Single)xAxes.Minimum, (Single)xAxes.Maximum, (Single)yAxes.Minimum, (Single)yAxes.Maximum, (Single)zAxes.Minimum, (Single)zAxes.Maximum);
        }

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
            ScaleX *= scale;
            ScaleY *= scale;
            ScaleZ *= scale;
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
                RotateX = q.Axis.X;
                RotateY = q.Axis.Y;
                RotateZ = q.Axis.Z;
                RotateAngle = q.Angle;
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
