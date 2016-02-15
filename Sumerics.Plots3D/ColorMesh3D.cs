namespace WPFChart3D
{
    using System;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    public class ColorMesh3D: Mesh3D
    {
        Color[] _colors;

        public override void SetVertexNo(Int32 nSize)
        {
            _points = new Point3D[nSize];
            _colors = new Color[nSize];
        }

        public override Color GetColor(Int32 nV)
        {
            return _colors[nV];
        }

        public void SetColor(Int32 nV, Byte r, Byte g, Byte b)
        {
            _colors[nV] = Color.FromRgb(r, g, b);
        }

        public void SetColor(Int32 nV, Color color)
        {
            _colors[nV] = color;
        }
    }
}
