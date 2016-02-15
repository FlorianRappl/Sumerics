namespace WPFChart3D
{
    using System;

    public class Cylinder3D : Mesh3D
    {
        Int32 _nRes;

        public Cylinder3D(Double a, Double b, Double h, Int32 nRes)
        {
            SetMesh(nRes);
            SetData(a, b, h);
        }

        void SetMesh(Int32 nRes)
        {
            var nVertNo = 2 * nRes + 2;
            var nTriNo = 4 * nRes;
            SetSize(nVertNo, nTriNo);

            for (var i = 0; i < nRes; i++)
            {
                var n1 = i;
                var n2 = (i == (nRes - 1)) ? 0 : i + 1;
                SetTriangle(i * 4 + 0, n1, n2, nRes + n1);
                SetTriangle(i * 4 + 1, nRes + n1, n2, nRes + n2);
                SetTriangle(i * 4 + 2, n2, n1, 2 * nRes);
                SetTriangle(i * 4 + 3, nRes + n1, nRes + n2, 2 * nRes + 1);
            }

            _nRes = nRes;
        }

        void SetData(Double a, Double b, Double h)
        {
            var aXYStep = 2.0f * 3.1415926f / ((Double)_nRes);

            for (var i = 0; i < _nRes; i++)
            {
                var aXY = i * aXYStep;
                SetPoint(i, a * Math.Cos(aXY), b * Math.Sin(aXY), -h / 2);
            }

            for (var i = 0; i < _nRes; i++)
            {
                var aXY = i * aXYStep;
                SetPoint(_nRes + i, a * Math.Cos(aXY), b * Math.Sin(aXY), h / 2);
            }

            SetPoint(2 * _nRes, 0, 0, -h / 2);
            SetPoint(2 * _nRes + 1, 0, 0, h / 2);

            XMin = -a;
            XMax = a;
            YMin = -b;
            YMax = b;
            ZMin = -h / 2;
            ZMax = h / 2;
        }
    }
}

