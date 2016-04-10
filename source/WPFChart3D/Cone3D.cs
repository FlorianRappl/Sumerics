namespace WPFChart3D
{
    using System;

    public class Cone3D : Mesh3D
    {
        Int32 _nRes;

        public Cone3D(Double a, Double b, Double h, Int32 nRes)
        {
            SetMesh(nRes);
            SetData(a, b, h);
        }

        void SetMesh(Int32 nRes)
        {
            var nVertNo = nRes + 2;
            var nTriNo = 2 * nRes;
            SetSize(nVertNo, nTriNo);

            for (var i = 0; i < nRes - 1; i++)
            {
                SetTriangle(i, i, i + 1, nRes + 1);
                SetTriangle(nRes + i, i + 1, i, nRes);
            }

            SetTriangle(nRes - 1, nRes - 1, 0, nRes + 1);
            SetTriangle(2 * nRes - 1, 0, nRes - 1, nRes);
            _nRes = nRes;
        }

        void SetData(Double a, Double b, Double h)
        {
            var aXYStep = 2.0f * 3.1415926f / ((Double)_nRes);

            for (var i = 0; i < _nRes; i++)
            {
                var aXY = i * aXYStep;
                SetPoint(i, a * Math.Cos(aXY), b * Math.Sin(aXY), 0);
            }

            SetPoint(_nRes, 0, 0, 0);
            SetPoint(_nRes + 1, 0, 0, h);

            XMin = -a;
            XMax = a;
            YMin = -b;
            YMax = b;
            ZMin = 0;
            ZMax = h;
        }
    }
}
