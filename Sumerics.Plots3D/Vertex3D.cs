using System.Windows.Media;

namespace WPFChart3D
{
    class Vertex3D
    {
        public Color color;                         // color of the dot
        public float x, y, z;                       // location of the dot
        public int nMinI;                           // link to the viewport positions array index
        public bool selected = false;               // is this dot selected by user
    }
}
