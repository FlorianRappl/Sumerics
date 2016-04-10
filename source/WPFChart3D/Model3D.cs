namespace WPFChart3D
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Media3D;

    public class Model3D : ModelVisual3D
    {
        TextureMapping _mapping;

        public Model3D()
        {
            _mapping = new TextureMapping();
        }

        public void SetRGBColor()
        {
            _mapping.SetRGBMaping();
        }

        public void SetPseudoColor()
        {
            _mapping.SetPseudoMaping();
        }

        void SetModel(List<Mesh3D> meshs, Material backMaterial)
        {
            var nMeshNo = meshs.Count;

            if (nMeshNo != 0)
            {
                var triangleMesh = new MeshGeometry3D();
                var nTotalVertNo = 0;

                for (var j = 0; j < nMeshNo; j++)
                {
                    var mesh = meshs[j];
                    var nVertNo = mesh.GetVertexNo();
                    var nTriNo = mesh.GetTriangleNo();

                    if (nVertNo > 0 && nTriNo > 0)
                    {
                        var vx = new Double[nVertNo];
                        var vy = new Double[nVertNo];
                        var vz = new Double[nVertNo];

                        for (var i = 0; i < nVertNo; i++)
                        {
                            vx[i] = vy[i] = vz[i] = 0;
                        }

                        for (var i = 0; i < nTriNo; i++)
                        {
                            var tri = mesh.GetTriangle(i);
                            var vN = mesh.GetTriangleNormal(i);

                            var n0 = tri.N0;
                            var n1 = tri.N1;
                            var n2 = tri.N2;

                            vx[n0] += vN.X;
                            vy[n0] += vN.Y;
                            vz[n0] += vN.Z;
                            vx[n1] += vN.X;
                            vy[n1] += vN.Y;
                            vz[n1] += vN.Z;
                            vx[n2] += vN.X;
                            vy[n2] += vN.Y;
                            vz[n2] += vN.Z;
                        }

                        for (var i = 0; i < nVertNo; i++)
                        {
                            var length = 1.0 / Math.Sqrt(vx[i] * vx[i] + vy[i] * vy[i] + vz[i] * vz[i]);

                            if (length < 1e20)
                            {
                                vx[i] *= length;
                                vy[i] *= length;
                                vz[i] *= length;
                            }

                            triangleMesh.Positions.Add(mesh.GetPoint(i));

                            var color = mesh.GetColor(i);
                            var mapPt = _mapping.GetMappingPosition(color);

                            triangleMesh.TextureCoordinates.Add(new Point(mapPt.X, mapPt.Y));
                            triangleMesh.Normals.Add(new Vector3D(vx[i], vy[i], vz[i]));
                        }

                        for (var i = 0; i < nTriNo; i++)
                        {
                            var tri = mesh.GetTriangle(i);
                            var n0 = tri.N0;
                            var n1 = tri.N1;
                            var n2 = tri.N2;

                            triangleMesh.TriangleIndices.Add(nTotalVertNo + n0);
                            triangleMesh.TriangleIndices.Add(nTotalVertNo + n1);
                            triangleMesh.TriangleIndices.Add(nTotalVertNo + n2);
                        }

                        nTotalVertNo += nVertNo;
                    }
                }

                //Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
                var material = _mapping.Material;

                var triangleModel = new GeometryModel3D(triangleMesh, material);
                triangleModel.Transform = new Transform3DGroup();

                if (backMaterial != null)
                {
                    triangleModel.BackMaterial = backMaterial;
                }

                Content = triangleModel;
            }
        }

        public static MeshGeometry3D GetGeometry(ModelVisual3D visual3d)
        {
            if (visual3d.Content == null)
                return null;

            GeometryModel3D triangleModel = (GeometryModel3D)(visual3d.Content);
            return (MeshGeometry3D)triangleModel.Geometry;
        }
    
        public static MeshGeometry3D GetGeometry(Viewport3D viewport3d, Int32 nModelIndex)
        {
            if (nModelIndex != -1)
            {
                var visual3d = (ModelVisual3D)(viewport3d.Children[nModelIndex]);

                if (visual3d.Content != null)
                {
                    var triangleModel = (GeometryModel3D)(visual3d.Content);
                    return (MeshGeometry3D)triangleModel.Geometry;
                }
            }

            return null;
        }

        public void UpdateModel(List<Mesh3D> meshs, Material backMaterial)
        {
            if (backMaterial == null)
            {
                SetRGBColor();
            }
            else
            {
                SetPseudoColor();
            }

            SetModel(meshs, backMaterial);
        }

    }
}
