namespace WPFChart3D
{
    //			0______________1
    //		   /|	          /|
    //        3 ____________2  |
    //        | 4  ---------|- 5
    //        |/            | /
    //        7 ___________ 6 
    using System;

    public class Bar3D: Mesh3D
    {
        public Bar3D(Double x0, Double y0, Double z0, Double W, Double L, Double H)
        {
            SetMesh();
            SetData(x0, y0, z0, W, L, H);
        }

        public void SetMesh()
        {
            SetSize(8, 12);
            SetTriangle(0, 0, 2, 1);
            SetTriangle(1, 0, 3, 2);
            SetTriangle(2, 1, 2, 5);
            SetTriangle(3, 2, 6, 5);
            SetTriangle(4, 3, 6, 2);
            SetTriangle(5, 3, 7, 6);
            SetTriangle(6, 0, 4, 3);
            SetTriangle(7, 3, 4, 7);
            SetTriangle(8, 4, 6, 7);
            SetTriangle(9, 4, 5, 6);
            SetTriangle(10, 0, 5, 4);
            SetTriangle(11, 0, 1, 5);
        }

        public void SetData(Double x0, Double y0, Double z0, Double W, Double L, Double H)
        {
            SetPoint(0, x0 - W / 2, y0 + L / 2, z0 + H / 2);
            SetPoint(1, x0 + W / 2, y0 + L / 2, z0 + H / 2);
            SetPoint(2, x0 + W / 2, y0 - L / 2, z0 + H / 2);
            SetPoint(3, x0 - W / 2, y0 - L / 2, z0 + H / 2);

            SetPoint(4, x0 - W / 2, y0 + L / 2, z0 - H / 2);
            SetPoint(5, x0 + W / 2, y0 + L / 2, z0 - H / 2);
            SetPoint(6, x0 + W / 2, y0 - L / 2, z0 - H / 2);
            SetPoint(7, x0 - W / 2, y0 - L / 2, z0 - H / 2);

            XMin = x0 - W / 2;
            XMax = x0 + W / 2;
            YMin = y0 - L / 2;
            YMax = y0 + L / 2;
            ZMin = z0 - H / 2;
            ZMax = z0 + H / 2;
        }
    }
}
