namespace WPFChart3D
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class Mesh3D
    {
        protected Point3D[] _points;
        protected Int32[] _vertIndices;
        protected Color _color;
        protected Triangle3D[] _tris;

        // data range
        public Double XMin;
        public Double XMax;
        public Double YMin;
        public Double YMax;
        public Double ZMin;
        public Double ZMax;

        public Int32 GetVertexNo()
        {
            return _points == null ? 0 : _points.Length;
        }

        public virtual void SetVertexNo(Int32 nSize)
        {
            _points = new Point3D[nSize];
            _vertIndices = new Int32[nSize];
        }

        public Int32 GetTriangleNo()
        {
            return _tris == null ? 0 : _tris.Length;
        }

        public void SetTriangleNo(Int32 nSize)
        {
            _tris = new Triangle3D[nSize];
        }

        public virtual void SetSize(Int32 nVertexNo, Int32 nTriangleNo)
        {
            SetVertexNo(nVertexNo);
            SetTriangleNo(nTriangleNo);
        }

        public Point3D GetPoint(Int32 n)
        {
            return _points[n];
        }

        public void SetPoint(Int32 n, Point3D pt)
        {
            _points[n] = pt;
        }

        public void SetPoint(Int32 n, Double x, Double y, Double z)
        {
            _points[n] = new Point3D(x, y, z);
        }

        public Triangle3D GetTriangle(Int32 n)
        {
            return _tris[n];
        }

        public void SetTriangle(Int32 n, Triangle3D triangle)
        {
            _tris[n] = triangle;
        }

        public void SetTriangle(Int32 i, Int32 m0, Int32 m1, Int32 m2)
        {
            _tris[i] = new Triangle3D(m0, m1, m2);
        }

        // get normal direction of a triangle
        public Vector3D GetTriangleNormal(Int32 n)
        {
            var tri = GetTriangle(n);
            var pt0 = GetPoint(tri.N0);
            var pt1 = GetPoint(tri.N1);
            var pt2 = GetPoint(tri.N2);

            var dx1 = pt1.X - pt0.X;
            var dy1 = pt1.Y - pt0.Y;
            var dz1 = pt1.Z - pt0.Z;

            var dx2 = pt2.X - pt0.X;
            var dy2 = pt2.Y - pt0.Y;
            var dz2 = pt2.Z - pt0.Z;

            var vx = dy1 * dz2 - dz1 * dy2;
            var vy = dz1 * dx2 - dx1 * dz2;
            var vz = dx1 * dy2 - dy1 * dx2;

            var length = Math.Sqrt(vx * vx + vy * vy + vz * vz);
            return new Vector3D(vx / length, vy / length, vz / length);
        }

        public virtual Color GetColor(Int32 nV)
        {
            return _color;
        }

        public void SetColor(Byte r, Byte g, Byte b)
        {
            _color = Color.FromRgb(r, g, b);
        }

        public void SetColor(Color color)
        {
            _color = color;
        }

        public void UpdatePositions(MeshGeometry3D meshGeometry)
        {
            var nVertNo = GetVertexNo();

            for (var i = 0; i < nVertNo; i++)
            {
                meshGeometry.Positions[i] = _points[i];
            }
        }

        // Set the test model
        public virtual void SetTestModel()
        {
            var size = 10.0;
            SetSize(3, 1);
            SetPoint(0, -0.5, 0, 0);
            SetPoint(1, 0.5, 0.5, 0.3);
            SetPoint(2, 0, 0.5, 0);
            SetTriangle(0, 0, 2, 1);
            XMin = 0; 
            XMax = 2 * size;
            YMin = 0; 
            YMax = size;
            ZMin = -size; 
            ZMax = size;
        }

    }
}
