using System.Collections;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Collections.Generic;
using _3DTools;

namespace WPFChart3D
{
    public class Model3D : ModelVisual3D
    {
        private TextureMapping m_mapping;

        public Model3D()
        {
            m_mapping = new TextureMapping();
        }

        public void SetRGBColor()
        {
            m_mapping.SetRGBMaping();
        }

        public void SetPseudoColor()
        {
            m_mapping.SetPseudoMaping();
        }

        // set this ModelVisual3D object from a array of mesh3D objects
        void SetModel(List<Mesh3D> meshs, Material backMaterial)
        {
            int nMeshNo = meshs.Count;

            if (nMeshNo == 0) 
                return;

            var triangleMesh = new MeshGeometry3D();
            int nTotalVertNo = 0;

            for (int j = 0; j < nMeshNo; j++)
            {
                var mesh = meshs[j];
                int nVertNo = mesh.GetVertexNo();
                int nTriNo = mesh.GetTriangleNo();

                if ((nVertNo <= 0) || (nTriNo <= 0))
					continue;
                
                var vx = new double[nVertNo];
                var vy = new double[nVertNo];
                var vz = new double[nVertNo];

                for (int i = 0; i < nVertNo; i++)
                    vx[i] = vy[i] = vz[i] = 0;

                // get normal of each vertex
                for (int i = 0; i < nTriNo; i++)
                {
                    var tri = mesh.GetTriangle(i);
                    var vN = mesh.GetTriangleNormal(i);

                    int n0 = tri.n0;
                    int n1 = tri.n1;
                    int n2 = tri.n2;

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

                for (int i = 0; i < nVertNo; i++)
                {
                    var length = 1.0 / Math.Sqrt(vx[i]*vx[i] + vy[i]*vy[i] + vz[i]*vz[i]);

                    if (length < 1e20)
                    {
                        vx[i] *= length;
                        vy[i] *= length;
                        vz[i] *= length;
                    }

                    triangleMesh.Positions.Add(mesh.GetPoint(i));

                    var color = mesh.GetColor(i);
                    var mapPt = m_mapping.GetMappingPosition(color);

                    triangleMesh.TextureCoordinates.Add(new System.Windows.Point(mapPt.X, mapPt.Y));
                    triangleMesh.Normals.Add(new Vector3D(vx[i], vy[i], vz[i]));
                }

                for (int i = 0; i < nTriNo; i++)
                {
                    var tri = mesh.GetTriangle(i);

                    int n0 = tri.n0;
                    int n1 = tri.n1;
                    int n2 = tri.n2;

                    triangleMesh.TriangleIndices.Add(nTotalVertNo + n0);
                    triangleMesh.TriangleIndices.Add(nTotalVertNo + n1);
                    triangleMesh.TriangleIndices.Add(nTotalVertNo + n2);
                 }

                nTotalVertNo += nVertNo;
            }

            //Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            var material = m_mapping.m_material;

            var triangleModel = new GeometryModel3D(triangleMesh, material);
            triangleModel.Transform = new Transform3DGroup();

            if (backMaterial != null) 
				triangleModel.BackMaterial = backMaterial;

            Content = triangleModel;
        }

        // get MeshGeometry3D object from Viewport3D
        public static MeshGeometry3D GetGeometry(ModelVisual3D visual3d)
        {
            if (visual3d.Content == null)
                return null;

            GeometryModel3D triangleModel = (GeometryModel3D)(visual3d.Content);
            return (MeshGeometry3D)triangleModel.Geometry;
        }
    
        // get MeshGeometry3D object from Viewport3D
        public static MeshGeometry3D GetGeometry(Viewport3D viewport3d, int nModelIndex)
        {
            if (nModelIndex == -1)
				return null;

            ModelVisual3D visual3d = (ModelVisual3D)(viewport3d.Children[nModelIndex]);

            if (visual3d.Content == null)
				return null;

            GeometryModel3D triangleModel = (GeometryModel3D)(visual3d.Content);
            return (MeshGeometry3D)triangleModel.Geometry;
        }

        // update the ModelVisual3D object in "viewport3d" using Mesh3D array "meshs"
        public void UpdateModel(List<Mesh3D> meshs, Material backMaterial)
        {
            if(backMaterial==null)
                SetRGBColor();
            else
                SetPseudoColor();

            SetModel(meshs, backMaterial);
        }

    }
}
