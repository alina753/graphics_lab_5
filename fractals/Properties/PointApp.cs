using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Laba4.Primitives
{
    class PointApp : IPrimitive
    {
        private float size = 6; // толщина точек
        private float[] coordinates = new float[] { 0, 0, 1 }; //x,y,z
        public float X
        {
            get
            {
                return coordinates[0]; // return x
            }
            set
            {
                coordinates[0] = value; // переустановка x
            }
        }
        public float Y
        {
            get
            {
                return coordinates[1]; // return y
            }
            set
            {
                coordinates[1] = value; // переустановка y
            }
        }
        public PointApp() { } // пустой конструктор
        public PointApp(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static PointApp FromPoint(Point p)
        {
            return new PointApp(p.X, p.Y);
        }
        public void Draw(Graphics g, bool selected)
        {
            Brush brh;
            if (selected)
            {
                brh = Brushes.Red;
            }
            else
            {
                brh = Brushes.Black;
            }
            g.FillRectangle(brh, X - size / 2, Y - size / 2,size, size);
        }
    }
}
