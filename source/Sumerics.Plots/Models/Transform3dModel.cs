namespace Sumerics.Plots.Models
{
    using System;

    public class Transform3dModel : LiveModel
    {
        Double _scaleX;
        Double _scaleY;
        Double _scaleZ;
        Double _rotateX;
        Double _rotateY;
        Double _rotateZ;
        Double _angle;

        public Transform3dModel()
        {
            Reset();
        }

        public Double ScaleX
        {
            get { return _scaleX; }
            set { _scaleX = value; RaisePropertyChanged(); }
        }

        public Double ScaleY
        {
            get { return _scaleY; }
            set { _scaleY = value; RaisePropertyChanged(); }
        }

        public Double ScaleZ
        {
            get { return _scaleZ; }
            set { _scaleZ = value; RaisePropertyChanged(); }
        }

        public Double RotateX
        {
            get { return _rotateX; }
            set { _rotateX = value; RaisePropertyChanged(); }
        }

        public Double RotateY
        {
            get { return _rotateY; }
            set { _rotateY = value; RaisePropertyChanged(); }
        }

        public Double RotateZ
        {
            get { return _rotateZ; }
            set { _rotateZ = value; RaisePropertyChanged(); }
        }

        public Double Angle
        {
            get { return _angle; }
            set { _angle = value; RaisePropertyChanged(); }
        }

        public void Reset()
        {
            ScaleX = 1.0;
            ScaleY = 1.0;
            ScaleZ = 1.0;
            RotateX = 0.0;
            RotateY = 0.0;
            RotateZ = 1.0;
            Angle = 0.0;
        }
    }
}
