using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Laba4.Primitives;

namespace fractals
{   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

        string aksiom;
        double angle;
        double direction;
        Dictionary<char, string> rules;
        int iters;
        List<int> nolinepoint = new List<int>();

        private Graphics g;
        private List<PointApp> points = new List<PointApp>();
        private List<Line> lines = new List<Line>();
        Stack<Tuple<PointApp, double>> memory;

        

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            this.pictureBox1.Invalidate();
            this.points.Clear();
            this.lines.Clear();

            string filename = "";
            if (domainUpDown1.SelectedItem.Equals("Куст 1"))
            {
                filename = "files/bush_1.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Куст 2"))
            {
                filename = "files/bush_3.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Снежинка Коха"))
            {
                filename = "files/kohs_nowflake.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("6-угольная мозаика"))
            {
                filename = "files/6.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Дерево"))
            {
                filename = "files/tree.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Кривая Коха"))
            {
                filename = "files/kohs_curved.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Ковер Серпинского"))
            {
                filename = "files/carpet.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("6-угольная кривая Госпера"))
            {
                filename = "files/gosper_6.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Наконечник Серпинского"))
            {
                filename = "files/end_serp.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Кривая Гильберта"))
            {
                filename = "files/gilbers_curved.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Кривая Дракона Х.Х."))
            {
                filename = "files/dragons_curved.txt";
            }



            iters = iteration_count.Text.Length == 0 ? 1 : int.Parse(iteration_count.Text);
            StreamReader f = new StreamReader(filename);
            List<string> s = f.ReadLine().Split().ToList();

            aksiom = s[0];

            angle = double.Parse(s[1]);
            direction = (-1)*double.Parse(s[2]);
            //direction = double.Parse(s[2]);

            rules = new Dictionary<char, string>();
            List<string> str;
            while (!f.EndOfStream)
            {
                str = f.ReadLine().Split().ToList();
                rules.Add(char.Parse(str[0]), str[2]);
            }
            f.Close();


            string pred = aksiom.ToString();
            string result = aksiom.ToString();//нулевая итерация

            for (int i = 0; i < iters; ++i)
            {
                result = "";
                for (int j = 0; j < pred.Length; ++j)
                {
                    char temp = pred[j];
                    if (rules.ContainsKey(temp))
                    {
                        result += rules[temp];
                    }
                    else
                    {
                        result += pred[j];
                    }
                }
                pred = result;
            }

            if (result.Any(x => x == '@'))
            {
                /*fractal_rand(result);
                render_rand();*/
            }
            else
            {
                //MessageBox.Show(result);
                Paint_fractal(result);
                render();
            }
        }

        private PointApp Rotate(double alpha, PointApp begin_point, PointApp end_point)
        {
            alpha = (float)((Math.PI * alpha) / 180);
            float[] center = new float[3];
            List<PointApp> RotateList = new List<PointApp>();


            center[0] += begin_point.X;
            center[1] += begin_point.Y;
            center[2] = 0;

            float[] RotateMatrix = new float[9] { (float)Math.Cos(alpha),         (float)Math.Sin(alpha),    0,
                                                  (float)Math.Sin(alpha) * -1,    (float)Math.Cos(alpha),    0,
                                                  center[0],                      center[1],                 1 };

            float[] point_matrix = new float[3] { end_point.X, end_point.Y, 1 };
            float[] res_point = new float[2];

            for (int i = 0; i < 2; i += 1)
            {
                for (int j = 0; j < 3; j += 1)
                {
                    res_point[i] += (point_matrix[j] - center[j]) * RotateMatrix[i + j * 3];
                }
            }

            return new PointApp(res_point[0], res_point[1]);
        }

        int line_lenght;
        int countP = 0;
        public void Paint_fractal(string rule)
        {
            line_lenght = 10;
            memory = new Stack<Tuple<PointApp, double>>();
            points.Add(new PointApp(0, pictureBox1.Height - 10));

            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] == 'F')
                {
                    PointApp p1 = points.Last();
                    PointApp p2 = new PointApp(p1.X + line_lenght, p1.Y);
                    p2 = Rotate(direction, p1, p2);
                    points.Add(p2);

                    countP++;
                }
                else if (rule[i] == '+')
                {
                    direction += angle;
                }
                else if (rule[i] == '-')
                {
                    direction -= angle;
                }
                else if (rule[i] == '[')
                {
                    var temp = Tuple.Create(points.Last(), direction);
                    memory.Push(temp);
                }
                else if (rule[i] == ']')
                {
                    var t0 = memory.Pop();
                    nolinepoint.Add(countP);
                    points.Add(t0.Item1);
                    direction = t0.Item2;
                    countP++;
                }
            }
        }

        private void render()
        {
            float minx, maxx, miny, maxy;
            IEnumerable<PointApp> queue = points.OrderBy(x => x.X);
            minx = queue.First().X;
            maxx = queue.Last().X;
            queue = points.OrderBy(y => y.Y);
            miny = queue.First().Y;
            maxy = queue.Last().Y;
            if (maxy == miny)
            {
                points = points.Select(p => new PointApp(Convert.ToInt32((pictureBox1.Width - 1) * (p.X - minx) / (maxx - minx)),
                pictureBox1.Height - 5)).ToList();
            }
            else
            {
                points = points.Select(p => new PointApp(Convert.ToInt32((pictureBox1.Width - 1) * (p.X - minx) / (maxx
                - minx)),
                Convert.ToInt32((pictureBox1.Height - 1) * (p.Y - miny) / (maxy - miny)))).ToList();
            }
            var pen = new Pen(Color.Black);
            if (nolinepoint.Count == 0)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    Line line = new Line(points[i], points[i+1]);
                    lines.Add(line);
                }
            }
            else
                for (int i = 0; i < points.Count - 1; ++i)
                {
                    if (nolinepoint.Any(x => x == i))
                    {
                        continue;
                    }
                    Line line = new Line(points[i], points[i + 1]);
                    lines.Add(line);
                }
            lines.ForEach(line => line.Draw(g));
            pictureBox1.Image = pictureBox1.Image;
            pictureBox1.Invalidate();
            pen.Dispose();
            points.Clear();
            nolinepoint.Clear();
            countP = 0;
        }


    }
}
