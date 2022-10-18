using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Laba4.Primitives
{
    class Polygon : IPrimitive
    {
        public List<PointApp> Points { get; set; } // точки полигона
        public Polygon()
        {
            this.Points = new List<PointApp>();
        }
        public Polygon(List<PointApp> points)
        {
            this.Points = points;
        }
        public void Draw(Graphics g, bool selected)
        {
            if(Points.Count == 1) // если одна точка
            {
                Points[0].Draw(g, selected);
            }
            else
            {
                Pen pen = new Pen(selected? Color.Red : Color.Black);
                pen.Width = 2;
                for(int i =0;i<Points.Count -1;++i)
                {
                    g.DrawLine(pen, Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y);
                }
                g.DrawLine(pen, Points[0].X, Points[0].Y, Points[Points.Count - 1].X, Points[Points.Count - 1].Y);
            }
        }
    }
}
