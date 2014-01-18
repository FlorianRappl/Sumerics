using System.Collections;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Collections.Generic;

namespace WPFChart3D
{
    class UniformSurfaceChart3D: SurfaceChart3D
    {
        // the grid number on each axis
        int m_nGridXNo;
        int m_nGridYNo;

        public void SetPoint(int i, int j, float x, float y, float z)
        {
            int nI = j * m_nGridXNo + i;
            m_vertices[nI].x = x;
            m_vertices[nI].y = y;
            m_vertices[nI].z = z;
        }

        public void SetZ(int i, int j, float z)
        {
            m_vertices[j*m_nGridXNo + i].z = z;
        }

        public void SetColor(int i, int j, Color color)
        {
            int nI = j * m_nGridXNo + i;
            m_vertices[nI].color = color;
        }

        public void SetGrid(int xNo, int yNo)
        {
            m_nGridXNo = xNo;
            m_nGridYNo = yNo;
        }

        public void SetGrid(int xNo, int yNo, float xMin, float xMax, float yMin, float yMax)
        {
            SetDataNo(xNo * yNo);
            m_nGridXNo = xNo;
            m_nGridYNo = yNo;
            m_xMin = xMin;
            m_xMax = xMax;
            m_yMin = yMin;
            m_yMax = yMax;
            var dx = (m_xMax - m_xMin) / (xNo - 1f);
            var dy = (m_yMax - m_yMin) / (yNo - 1f);

            for (int i = 0; i < xNo; i++)
            {
                for (int j = 0; j < yNo; j++)
                {
                    var xV = m_xMin + dx * i;
                    var yV = m_yMin + dy * j;
                    m_vertices[j * xNo + i] = new Vertex3D();
                    SetPoint(i, j, xV, yV, 0);
                }
            }
         
        }

        // convert the uniform surface chart to a array of Mesh3D (only one element)
		public List<Mesh3D> GetMeshes()
        {
            var meshes = new List<Mesh3D>();
            var surfaceMesh = new ColorMesh3D();

            surfaceMesh.SetSize(m_nGridXNo * m_nGridYNo, 2 * (m_nGridXNo - 1) * (m_nGridYNo - 1));

            for (int i = 0; i < m_nGridXNo; i++)
            {
                for (int j = 0; j < m_nGridYNo; j++)
                {
                    var nI = j * m_nGridXNo + i;
                    var vert = m_vertices[nI];
                    m_vertices[nI].nMinI = nI;
                    surfaceMesh.SetPoint(nI, new Point3D(vert.x, vert.y, vert.z));
                    surfaceMesh.SetColor(nI, vert.color);
                }
            }

            // set triangle
            int nT = 0;
            var nx = m_nGridXNo - 1;
            var ny = m_nGridYNo - 1;

            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    int i1 = i + 1;
                    int j1 = j + 1;
                    int n00 = j * m_nGridXNo + i;
                    int n10 = j * m_nGridXNo + i1;
                    int n01 = j1 * m_nGridXNo + i;
                    int n11 = j1 * m_nGridXNo + i1;

                    surfaceMesh.SetTriangle(nT++, n00, n10, n01);
                    surfaceMesh.SetTriangle(nT++, n01, n10, n11);
                }
            }

            meshes.Add(surfaceMesh);
            return meshes;
        }
    }
}
