namespace WPFChart3D
{
    using System;

    public class Ellipse3D : Mesh3D
    {
        Int32 _nRes;

        public Ellipse3D(Double a, Double b, Double h, Int32 nRes)
        {
            SetMesh(nRes);
            SetData(a, b, h);
        }

        void SetMesh(Int32 nRes)
        {
            var nVertNo = (nRes - 2) * nRes + 2;
            var nTriNo = 2 * nRes * (nRes - 3) + 2 * nRes;
            SetSize(nVertNo, nTriNo);

            var n00 = 0;
            var n01 = 0;
            var n10 = 0;
            var n11 = 0;
            var nTriIndex = 0;
            var nI2 = 0;
            var j = 1;

            for (var i = 0; i < nRes; i++)
            {
                nI2 = (i == (nRes - 1)) ? 0 : i + 1;
                n00 = 1 + (j - 1) * nRes + i;
                n10 = 1 + (j - 1) * nRes + nI2;
                n01 = 0;

                SetTriangle(nTriIndex, n00, n10, n01);
                nTriIndex++;
            }

            for (j = 1; j < (nRes - 2); j++)
            {
                for (var i = 0; i < nRes; i++)
                {
                    nI2 = (i == (nRes - 1)) ? 0 : i + 1;
                    n00 = 1 + (j - 1) * nRes + i;
                    n10 = 1 + (j - 1) * nRes + nI2;
                    n01 = 1 + j * nRes + i;
                    n11 = 1 + j * nRes + nI2;

                    SetTriangle(nTriIndex, n00, n01, n10);
                    SetTriangle(nTriIndex + 1, n01, n11, n10);
                    nTriIndex += 2;
                }
            }

            j = nRes - 2;

            for (var i = 0; i < nRes; i++)
            {
                nI2 = (i == (nRes - 1)) ? 0 : i + 1;
                n00 = 1 + (j - 1) * nRes + i;
                n10 = 1 + (j - 1) * nRes + nI2;
                n01 = nVertNo - 1;

                SetTriangle(nTriIndex, n00, n01, n10);
                nTriIndex++;
            }

            _nRes = nRes;
        }

        void SetData(Double a, Double b, Double h)
        {
            var aXYStep = 2.0f * 3.1415926f / ((Double)_nRes);
            var aZStep = 3.1415926f / ((Double)_nRes - 1.0);

            SetPoint(0, 0, 0, h);

            for (var j = 1; j < (_nRes - 1); j++)
            {
                for (var i = 0; i < _nRes; i++)
                {
                    var aXY = i * aXYStep;
                    var aZAngle = j * aZStep;

                    var x1 = a * Math.Sin(aZAngle) * Math.Cos(aXY);
                    var y1 = b * Math.Sin(aZAngle) * Math.Sin(aXY);
                    var z1 = h * Math.Cos(aZAngle);
                    SetPoint((j - 1) * _nRes + i + 1, x1, y1, z1);
                }
            }

            SetPoint((_nRes - 2) * _nRes + 1, 0, 0, -h);

            XMin = -a;
            XMax = a;
            YMin = -b;
            YMax = b;
            ZMin = -h;
            ZMax = h;
        }
    }
}
