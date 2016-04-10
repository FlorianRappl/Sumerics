namespace WPFChart3D
{
    //      vertex index
    //      0  -------------------- 1
    //      |   -----------------   |
    //      |   | 4            5 |  |
    //      |   |                |  |
    //      |   |                |  |
    //      |   | 7            6 |  |
    //      |   -----------------   |
    //      3  -------------------- 2
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    class ViewportRect : Mesh3D
    {
        Double _x1 = 0.0;
        Double _y1 = 0.0;
        Double _x2 = 0.0;
        Double _y2 = 0.0;
        public Double LineWidth = 0.005;
        public Double ZLevel = 1.0;

        public ViewportRect()
        {
            SetSize(8, 8);
            SetTriangle(0, 0, 4, 1);
            SetTriangle(1, 1, 4, 5);
            SetTriangle(2, 1, 5, 2);
            SetTriangle(3, 2, 5, 6);
            SetTriangle(4, 2, 6, 3);
            SetTriangle(5, 3, 6, 7);
            SetTriangle(6, 0, 3, 7);
            SetTriangle(7, 0, 7, 4);
            SetColor(255, 0, 0);
        }

        void SetRect(Double xC, Double yC, Double w, Double h)
        {
            SetPoint(0, xC - w / 2, yC + h / 2, ZLevel);
            SetPoint(1, xC + w / 2, yC + h / 2, ZLevel);
            SetPoint(2, xC + w / 2, yC - h / 2, ZLevel);
            SetPoint(3, xC - w / 2, yC - h / 2, ZLevel);
            SetPoint(4, xC - w / 2 + LineWidth, yC + h / 2 - LineWidth, ZLevel);
            SetPoint(5, xC + w / 2 - LineWidth, yC + h / 2 - LineWidth, ZLevel);
            SetPoint(6, xC + w / 2 - LineWidth, yC - h / 2 + LineWidth, ZLevel);
            SetPoint(7, xC - w / 2 + LineWidth, yC - h / 2 + LineWidth, ZLevel);
        }

        void SetRect()
        {
            var xC = (_x1 + _x2) * 0.5;
            var yC = (_y1 + _y2) * 0.5;
            var w = Math.Abs(_x2 - _x1);
            var h = Math.Abs(_y2 - _y1);
            SetRect(xC, yC, w, h);
        }

        public void SetRect(Point pt1, Point pt2)
        {
            _x1 = pt1.X;
            _y1 = pt1.Y;
            _x2 = pt2.X;
            _y2 = pt2.Y;
            SetRect();
        }

        public List<Mesh3D> GetMeshes()
        {
            var meshs = new List<Mesh3D>();
            meshs.Add(this);

            var nVertNo = GetVertexNo();

            for (var i = 0; i < nVertNo; i++)
            {
                _vertIndices[i] = i;
            }

            return meshs;
        }

        public void OnMouseDown(Point pt, Viewport3D viewport3d, Int32 nModelIndex)
        {
            if (nModelIndex != -1)
            {
                var meshGeometry = Model3D.GetGeometry(viewport3d, nModelIndex);

                if (meshGeometry != null)
                {
                    var pt1 = TransformMatrix.ScreenPtToViewportPt(pt, viewport3d);

                    SetRect(pt1, pt1);
                    UpdatePositions(meshGeometry);
                }
            }
        }

        public void OnMouseMove(Point pt, Viewport3D viewport3d, Int32 nModelIndex)
        {
            if (nModelIndex != -1)
            {
                var meshGeometry = Model3D.GetGeometry(viewport3d, nModelIndex);

                if (meshGeometry != null)
                {
                    var pt2 = TransformMatrix.ScreenPtToViewportPt(pt, viewport3d);
                    _x2 = pt2.X;
                    _y2 = pt2.Y;
                    SetRect();
                    UpdatePositions(meshGeometry);
                }
            }
        }

        public Double MinX()
        {
            return _x1 < _x2 ? _x1 : _x2;
        }

        public Double MaxX()
        {
            return _x1 < _x2 ? _x2 : _x1;
        }
        public Double MinY()
        {
            return _y1 < _y2 ? _y1 : _y2;
        }

        public Double MaxY()
        {
            return _y1 < _y2 ? _y2 : _y1;
        }
    }
}
