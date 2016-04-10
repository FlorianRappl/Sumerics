namespace Sumerics.Plots
{
    using System;

    sealed class BufferState : IEquatable<BufferState>
    {
        public Double MinX { get; set; }

        public Double MaxX { get; set; }

        public Double MinY { get; set; }

        public Double MaxY { get; set; }

        public Double Width { get; set; }

        public Double Height { get; set; }

        public override Boolean Equals(Object obj)
        {
            if (!Object.ReferenceEquals(this, obj))
            {
                var other = obj as BufferState;

                if (other != null)
                {
                    return Equals(other);
                }

                return false;
            }

            return true;
        }

        public Boolean Equals(BufferState buffer)
        {
            return buffer.MinY == MinY &&
                    buffer.MaxY == MaxY &&
                    buffer.MinX == MinX &&
                    buffer.MaxX == MaxX &&
                    buffer.Width == Width && 
                    buffer.Height == Height;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }

        public void ReplaceWith(BufferState state)
        {
            MinX = state.MinX;
            MaxX = state.MaxX;
            MinY = state.MinY;
            MaxY = state.MaxY;
            Width = state.Width;
            Height = state.Height;
        }
    }
}
