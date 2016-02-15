namespace WPFChart3D
{
    //				   0  
    //				/  | \		 
    //			   /   1  \
    //            /  /   \ \  
    //          3/----------2 
    using System;

    public class Pyramid3D : Mesh3D
    {
        public Pyramid3D(Double size)
        {
            SetMesh();
            var W = size;
            var L = size * Math.Sqrt(3)/2;
            var H = size * Math.Sqrt(2.0/3.0);
            SetData(W, L, H);
        }

        public Pyramid3D(Double W, Double L, Double H)
        {
            SetMesh();
            SetData(W, L, H);
        }

        void SetMesh()
        {
            SetSize(4, 4);
            SetTriangle(0, 0, 2, 1);
            SetTriangle(1, 0, 3, 2);
            SetTriangle(2, 0, 1, 3);
            SetTriangle(3, 1, 2, 3);
        }

        public void SetData(Double W, Double L, Double H)
	    {
		    SetPoint(0, 0, 0, H);
		    SetPoint(1, 0, L/2, 0);
            SetPoint(2, +W / 2, -L / 2, 0);
            SetPoint(3, -W / 2, -L / 2, 0);
            XMin = - W / 2;
            XMax = + W / 2;
            YMin = - L / 2;
            YMax = + L / 2;
            ZMin = - H / 2;
            ZMax = + H / 2;
        }
    }
}
