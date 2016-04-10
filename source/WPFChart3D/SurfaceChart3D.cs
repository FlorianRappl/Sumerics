namespace WPFChart3D
{
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    class SurfaceChart3D : Chart3D
    {
        public override void Select(ViewportRect rect, TransformMatrix matrix, Viewport3D viewport3d)
        {
            var nDotNo = GetDataNo();

            if (nDotNo != 0)
            {
                var xMin = rect.MinX();
                var xMax = rect.MaxX();
                var yMin = rect.MinY();
                var yMax = rect.MaxY();

                for (var i = 0; i < nDotNo; i++)
                {
                    var pt = matrix.VertexToViewportPt(new Point3D(_vertices[i].X, _vertices[i].Y, _vertices[i].Z), viewport3d);
                    _vertices[i].IsSelected = (pt.X > xMin) && (pt.X < xMax) && (pt.Y > yMin) && (pt.Y < yMax);
                }
            }
        }

        public override void HighlightSelection(MeshGeometry3D meshGeometry, Color selectColor)
        {
            var nDotNo = GetDataNo();

            if (nDotNo != 0)
            {
                for (var i = 0; i < nDotNo; i++)
                {
                    var color = _vertices[i].IsSelected ? selectColor : _vertices[i].Color;
                    var mapPt = TextureMapping.GetMappingPosition(color, true);
                    var nMin = _vertices[i].MinI;
                    meshGeometry.TextureCoordinates[nMin] = mapPt;
                }
            }
        }
    }
}
