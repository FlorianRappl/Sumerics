namespace WPFChart3D
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    sealed class UniformSurfaceChart3D : SurfaceChart3D
    {
        Int32 _nGridXNo;
        Int32 _nGridYNo;

        public void SetPoint(Int32 i, Int32 j, Single x, Single y, Single z)
        {
            var nI = j * _nGridXNo + i;
            _vertices[nI].X = x;
            _vertices[nI].Y = y;
            _vertices[nI].Z = z;
        }

        public void SetZ(Int32 i, Int32 j, Single z)
        {
            _vertices[j * _nGridXNo + i].Z = z;
        }

        public void SetColor(Int32 i, Int32 j, Color color)
        {
            var nI = j * _nGridXNo + i;
            _vertices[nI].Color = color;
        }

        public void SetGrid(Int32 xNo, Int32 yNo)
        {
            _nGridXNo = xNo;
            _nGridYNo = yNo;
        }

        public void SetGrid(Int32 xNo, Int32 yNo, Single xMin, Single xMax, Single yMin, Single yMax)
        {
            SetDataNo(xNo * yNo);
            _nGridXNo = xNo;
            _nGridYNo = yNo;
            _xMin = xMin;
            _xMax = xMax;
            _yMin = yMin;
            _yMax = yMax;
            var dx = (_xMax - _xMin) / (xNo - 1f);
            var dy = (_yMax - _yMin) / (yNo - 1f);

            for (var i = 0; i < xNo; i++)
            {
                for (var j = 0; j < yNo; j++)
                {
                    var xV = _xMin + dx * i;
                    var yV = _yMin + dy * j;
                    _vertices[j * xNo + i] = new Vertex3D();
                    SetPoint(i, j, xV, yV, 0);
                }
            }

        }

        public List<Mesh3D> GetMeshes()
        {
            var meshes = new List<Mesh3D>();
            var surfaceMesh = new ColorMesh3D();

            surfaceMesh.SetSize(_nGridXNo * _nGridYNo, 2 * (_nGridXNo - 1) * (_nGridYNo - 1));

            for (var i = 0; i < _nGridXNo; i++)
            {
                for (var j = 0; j < _nGridYNo; j++)
                {
                    var nI = j * _nGridXNo + i;
                    var vert = _vertices[nI];
                    _vertices[nI].MinI = nI;
                    surfaceMesh.SetPoint(nI, new Point3D(vert.X, vert.Y, vert.Z));
                    surfaceMesh.SetColor(nI, vert.Color);
                }
            }

            // set triangle
            var nT = 0;
            var nx = _nGridXNo - 1;
            var ny = _nGridYNo - 1;

            for (var i = 0; i < nx; i++)
            {
                for (var j = 0; j < ny; j++)
                {
                    var i1 = i + 1;
                    var j1 = j + 1;
                    var n00 = j * _nGridXNo + i;
                    var n10 = j * _nGridXNo + i1;
                    var n01 = j1 * _nGridXNo + i;
                    var n11 = j1 * _nGridXNo + i1;

                    surfaceMesh.SetTriangle(nT++, n00, n10, n01);
                    surfaceMesh.SetTriangle(nT++, n01, n10, n11);
                }
            }

            meshes.Add(surfaceMesh);
            return meshes;
        }
    }
}
