using _3DTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFChart3D
{
    /// <summary>
    /// Interaction logic for Plot3D.xaml
    /// </summary>
    public partial class Plot3D : UserControl
    {
        #region Constants

        static readonly double SQRT_TWO = Math.Sqrt(2);

        #endregion

        #region Members

        TransformMatrix m_transformMatrix;
        Chart3D m_3dChart;
        ViewportRect m_selectRect;
        ScreenSpaceLines3D wireframes;
        ScreenSpaceLines3D axis;
        Model3D model3d;

        ScaleTransform3D _scale;
        AxisAngleRotation3D _rotate;
        Vector3D _previousPosition3D;
        Point origin;

        double xMin;
        double xMax;
        double yMin;
        double yMax;
        double zMin;
        double zMax;

        #endregion

        #region ctor

        public Plot3D()
        {
            wireframes = new ScreenSpaceLines3D();
            wireframes.Color = Colors.SteelBlue;

            axis = new ScreenSpaceLines3D();
            axis.Color = Colors.Black;
            axis.Thickness = 0.5;

            model3d = new Model3D();
            m_transformMatrix = new TransformMatrix();
            m_selectRect = new ViewportRect();

            InitializeComponent();
            ObtainTransforms();
            mainViewport.Children.Add(wireframes);
            mainViewport.Children.Add(axis);

            trackBall.IsManipulationEnabled = true;
            trackBall.ManipulationDelta += TouchDelta;
            trackBall.ManipulationStarted += TouchStarted;
            trackBall.PreviewMouseWheel += MouseWheelRotated;
        }

        #endregion

        #region Mouse Wheel

        void MouseWheelRotated(object sender, MouseWheelEventArgs e)
        {
            //Am I using some magic number here or is "120" for the
            //standard delta for the mouse wheel too high for usual
            //mice? My awesome razer salmosa has 120...
            if (e.Delta > 0)
                Zoom(e.Delta * 0.01);
            else if (e.Delta < 0)
                Zoom(-100.0 / e.Delta);
        }

        #endregion

        #region Touch Events

        void TouchStarted(object sender, ManipulationStartedEventArgs e)
        {
            origin = e.ManipulationOrigin;
            _previousPosition3D = ProjectToTrackball(mainViewport.ActualWidth, mainViewport.ActualHeight, origin);
        }

        void TouchDelta(object sender, ManipulationDeltaEventArgs e)
        {
            Zoom(SQRT_TWO / e.DeltaManipulation.Scale.Length);
            Rotate(e.DeltaManipulation.Translation);
        }

        #endregion

        #region Properties

        public string Title
        {
            get { return xTitle.Text; }
            set { xTitle.Text = value; }
        }

        public double WireframeThickness
        {
            get { return wireframes.Thickness; }
            set { wireframes.Thickness = value; }
        }

        public Color WireframeColor
        {
            get { return wireframes.Color; }
            set { wireframes.Color = value; }
        }

        public bool ShowAxis
        {
            get { return axis.Points.Count > 0; }
            set
            {
                axis.Points.Clear();

                if (value)
                {
                    var geo = new GeometryModel3D();
                    var cube = new MeshGeometry3D();

                    var p1 = new Point3D(xMin, yMin, zMin);
                    var p2 = new Point3D(xMax, yMin, zMin);
                    var p5 = new Point3D(xMax, yMax, zMin);
                    var p3 = new Point3D(xMin, yMax, zMin);

                    var p4 = new Point3D(xMin, yMin, zMax);
                    var p6 = new Point3D(xMax, yMin, zMax);
                    var p8 = new Point3D(xMin, yMax, zMax);
                    var p7 = new Point3D(xMax, yMax, zMax);

                    cube.Positions.Add(p1);//0 - 0 0 0
                    cube.Positions.Add(p2);//1 - 1 0 0
                    cube.Positions.Add(p3);//2 - 0 1 0
                    cube.Positions.Add(p5);//3 - 1 1 0

                    cube.Positions.Add(p4);//4 - 0 0 1
                    cube.Positions.Add(p6);//5 - 1 0 1
                    cube.Positions.Add(p8);//6 - 0 1 1
                    cube.Positions.Add(p7);//7 - 1 1 1

                    var zh = (zMax - zMin) * 0.5;
                    var xh = (xMax - xMin) * 0.5;
                    var yh = (yMax - yMin) * 0.5;

                    var pc1 = new Point3D(xMin, yMin, zh);
                    var pc2 = new Point3D(xMax, yMin, zh);
                    var pc5 = new Point3D(xMax, yMax, zh);
                    var pc3 = new Point3D(xMin, yMax, zh);

                    var pc4 = new Point3D(xh, yMin, zMax);
                    var pc6 = new Point3D(xMax, yh, zMax);
                    var pc8 = new Point3D(xMin, yh, zMax);
                    var pc7 = new Point3D(xh, yMax, zMax);

                    var pc9 = new Point3D(xh, yMin, zMin);
                    var pc10 = new Point3D(xMax, yh, zMin);
                    var pc11 = new Point3D(xMin, yh, zMin);
                    var pc12 = new Point3D(xh, yMax, zMin);

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
                    axis.MakeWireframe(geo);
                    axis.Transform = new MatrixTransform3D(m_transformMatrix.m_totalMatrix);
                }
            }
        }

        #endregion

        #region Plotting

        public void ResetTransformation()
        {
            var tg = trackBall.Transform as Transform3DGroup;
            var scale = tg.Children[0] as ScaleTransform3D;
            var rotate = (tg.Children[1] as RotateTransform3D).Rotation as AxisAngleRotation3D;
            scale.ScaleX = 1;
            scale.ScaleY = 1;
            scale.ScaleZ = 1;
            rotate.Axis = new Vector3D(0, 0, 1);
            rotate.Angle = 0;
        }

        public void CreateSurface(int nx, int ny)
        {
            var surface = new UniformSurfaceChart3D();
            m_3dChart = surface;
            m_3dChart.SetDataNo(nx * ny);
            surface.SetGrid(nx, ny);
        }

        public void SetVertex(int i, float x, float y, float z)
        {
            var v = new Vertex3D();
            v.x = x;
            v.y = y;
            v.z = z;
            m_3dChart[i] = v;
        }

        public void SetView(float xmin, float xmax, float ymin, float ymax, float zmin, float zmax)
        {
            m_3dChart.SetDataRange(xmin, xmax, ymin, ymax, zmin, zmax);
        }

        public void SetColors(float zmin, float zmax)
        {
            int nv = m_3dChart.GetDataNo();

            for (int i = 0; i < nv; i++)
            {
                var vert = m_3dChart[i];
                var h = (vert.z - zmin) / (zmax - zmin);
                var color = TextureMapping.PseudoColor(h);
                m_3dChart[i].color = color;
            }
        }

        public void SetColors(float zmin, float zmax, Color[] colors)
        {
            int nv = m_3dChart.GetDataNo();

            for (int i = 0; i < nv; i++)
            {
                var vert = m_3dChart[i];
                var h = (vert.z - zmin) / (zmax - zmin);
                var index = (int)Math.Min(Math.Floor(h * colors.Length), colors.Length - 1);
                m_3dChart[i].color = colors[index];
            }
        }

        public Viewport3D Viewport
        {
            get { return mainViewport; }
        }

        public void Publish()
        {
            var meshs = ((UniformSurfaceChart3D)m_3dChart).GetMeshes();

            var backMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            model3d.UpdateModel(meshs, backMaterial);

            var xMin = m_3dChart.XMin();
            var xMax = m_3dChart.XMax();
            var yMin = m_3dChart.YMin();
            var yMax = m_3dChart.YMax();
            var zMin = m_3dChart.ZMin();
            var zMax = m_3dChart.ZMax();

            Project(xMin, xMax, yMin, yMax, zMin, zMax);
            TransformChart();
        }

        public void AddWireframeVertex(double x1, double y1, double z1, double x2, double y2, double z2, double x3, double y3, double z3)
        {
            var p1 = new Point3D(x1, y1, z1);
            var p2 = new Point3D(x2, y2, z2);
            var p3 = new Point3D(x3, y3, z3);

            AddWireframeVertex(wireframes, p1, p2, p3);
        }

        void AddWireframeVertex(ScreenSpaceLines3D space, Point3D p1, Point3D p2, Point3D p3)
        {
            space.Points.Add(p1);
            space.Points.Add(p2);
            space.Points.Add(p2);
            space.Points.Add(p3);
            space.Points.Add(p3);
            space.Points.Add(p1);
        }

        public void AddWireframeVertex(double x, double y, double z)
        {
            wireframes.Points.Add(new Point3D(x, y, z));
        }

        void Project(double xmin, double xmax, double ymin, double ymax, double zmin, double zmax)
        {
            this.xMin = xmin;
            this.xMax = xmax;
            this.yMin = ymin;
            this.yMax = ymax;
            this.zMin = zmin;
            this.zMax = zmax;

            m_transformMatrix.CalculateProjectionMatrix(xMin, xMax, yMin, yMax, zMin, zMax, 0.5);
        }

        public void SetTransformWireframe(double xmin, double xmax, double ymin, double ymax, double zmin, double zmax)
        {
            Project(xmin, xmax, ymin, ymax, zmin, zmax);
            wireframes.Transform = new MatrixTransform3D(m_transformMatrix.m_totalMatrix);
        }

        public void SetWireframe(bool status)
        {
            if (status)
                wireframes.MakeWireframe(model3d.Content);
            else
                wireframes.Points.Clear();
        }

        public void SetSurface(bool status)
        {
            if (status)
            {
                if (!mainViewport.Children.Contains(model3d))
                    mainViewport.Children.Add(model3d);
            }
            else
                mainViewport.Children.Remove(model3d);
        }

        #endregion

        #region Test Surface

        // function for testing surface chart
        public void TestSurfacePlot(int nGridNo)
        {
            int nXNo = nGridNo;
            int nYNo = nGridNo;

            // 1. set the surface grid
            m_3dChart = new UniformSurfaceChart3D();
            ((UniformSurfaceChart3D)m_3dChart).SetGrid(nXNo, nYNo, -100, 100, -100, 100);

            // 2. set surface chart z value
            var xC = m_3dChart.XCenter();
            var yC = m_3dChart.YCenter();
            var nVertNo = m_3dChart.GetDataNo();
            var zV = 0f;

            for (int i = 0; i < nVertNo; i++)
            {
                var vert = m_3dChart[i];
                var r = 0.15 * Math.Sqrt((vert.x - xC) * (vert.x - xC) + (vert.y - yC) * (vert.y - yC));
                zV = r < 1e-10 ? 1f : (float)(Math.Sin(r) / r);
                m_3dChart[i].z = zV;
            }

            m_3dChart.GetDataRange();

            // 3. set the surface chart color according to z vaule
            double zMin = m_3dChart.ZMin();
            double zMax = m_3dChart.ZMax();

            for (int i = 0; i < nVertNo; i++)
            {
                var vert = m_3dChart[i];
                var h = (vert.z - zMin) / (zMax - zMin);
                var color = WPFChart3D.TextureMapping.PseudoColor(h);
                m_3dChart[i].color = color;
            }

            // 4. Get the Mesh3D array from surface chart
            var meshs = ((UniformSurfaceChart3D)m_3dChart).GetMeshes();

            // 5. Set the model display of surface chart
            var model3d = new Model3D();
            var backMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Gray));
            model3d.UpdateModel(meshs, backMaterial);

            //Project
            m_transformMatrix.CalculateProjectionMatrix(m_3dChart.XMin(), m_3dChart.XMax(), m_3dChart.YMin(), m_3dChart.YMax(), zMin, zMax, 0.5);
            TransformChart();
        }

        #endregion

        #region Helper

        void ObtainTransforms()
        {
            var tg = trackBall.Transform as Transform3DGroup;

            if (tg == null || tg.Children.Count != 2)
                return;

            _scale = (ScaleTransform3D)tg.Children[0];
            _rotate = (AxisAngleRotation3D)((RotateTransform3D)tg.Children[1]).Rotation;

            if (trackBall.Viewport3D == null  || trackBall.Viewport3D.Camera == null)
                return;

            if (trackBall.Viewport3D.Camera.IsFrozen)
                trackBall.Viewport3D.Camera = trackBall.Viewport3D.Camera.Clone();

            if (trackBall.Viewport3D.Camera.Transform != tg)
                trackBall.Viewport3D.Camera.Transform = tg;
        }

        void Zoom(double scale)
        {
            _scale.ScaleX *= scale;
            _scale.ScaleY *= scale;
            _scale.ScaleZ *= scale;
        }

        void Rotate(Vector translation)
        {
            origin += translation;
            var pos = ProjectToTrackball(mainViewport.ActualWidth, mainViewport.ActualHeight, origin);
            var axis = Vector3D.CrossProduct(_previousPosition3D, pos);

            if (axis.Length == 0)
                return;

            var angle = Vector3D.AngleBetween(_previousPosition3D, pos);
            var delta = new Quaternion(axis, -angle);

            // Get the current orientantion from the RotateTransform3D
            Quaternion q = new Quaternion(_rotate.Axis, _rotate.Angle);

            // Compose the delta with the previous orientation
            q *= delta;

            // Write the new orientation back to the Rotation3D
            _rotate.Axis = q.Axis;
            _rotate.Angle = q.Angle;

            _previousPosition3D = pos;
        }

        Vector3D ProjectToTrackball(double width, double height, Point point)
        {
            // Scale so bounds map to [0,0] - [2,2]
            double x = 2.0 * point.X / width;
            double y = 2.0 * point.Y / height;

            // Translate 0,0 to the center
            x = x - 1.0;

            // Flip so +Y is up instead of down
            y = 1.0 - y;

            // z^2 = 1 - x^2 - y^2
            double z2 = 1.0 - x * x - y * y;

            //Compute z value
            double z = z2 > 0.0 ? Math.Sqrt(z2) : 0.0;

            //Return
            return new Vector3D(x, y, z);
        }

        // this function is used to rotate, drag and zoom the 3d chart
        void TransformChart()
        {
            if (model3d.Content == null)
                return;

            var group = model3d.Content.Transform as Transform3DGroup;

            if (group != null)
            {
                group.Children.Clear();
                group.Children.Add(new MatrixTransform3D(m_transformMatrix.m_totalMatrix));
            }
        }

        #endregion
    }
}
