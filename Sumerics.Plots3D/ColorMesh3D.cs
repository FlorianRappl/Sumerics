using System;
using System.Windows.Media.Media3D;

namespace WPFChart3D
{
    public class ColorMesh3D: Mesh3D
    {
        private System.Windows.Media.Color [] m_colors;             // color information of each vertex

        // override the set vertex number, since we include the color information for each vertex
        public override void SetVertexNo(int nSize)
        {
            m_points = new Point3D[nSize];
            m_colors = new System.Windows.Media.Color[nSize];
        }

        // get color information of each vertex
        public override System.Windows.Media.Color GetColor(int nV)
        {
            return m_colors[nV];
        }

        // set color information of each vertex
        public void SetColor(int nV, Byte r, Byte g, Byte b)
        {
            m_colors[nV] = System.Windows.Media.Color.FromRgb(r, g, b);
        }

        public void SetColor(int nV, System.Windows.Media.Color color)
        {
            m_colors[nV] = color;
        }

    }


}
