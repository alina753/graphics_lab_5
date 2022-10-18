using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Laba4.Primitives
{
    class Line : IPrimitive
    {
        public PointApp A { get; set; }
        public PointApp B { get; set; }
        
        public Line(PointApp a, PointApp b)
        {
            this.A = a;
            this.B = b;
        }
        public void Draw(Graphics g, bool selected)
        {
            Pen pen = new Pen(selected ? Color.Red : Color.Black);
            pen.Width = 2; // Толщина пера
            g.DrawLine(pen, A.X, A.Y, B.X, B.Y);
        }
        public float Distance(PointApp p)
        {
            float dx = B.X - A.X;
            float dy = B.Y - A.Y;
            float n = (float)Math.Sqrt(dy * dy + dx * dx);
            return (dx * p.Y - dy * p.X - dx * A.Y + dy * A.X) / n;
            // положительное - справа, отрицательное - слева
        }
    }
}
