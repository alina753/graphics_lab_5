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

        char aksiom;
        float corner;
        float direction;
        List<string[]> rules;

        private Graphics g;
        private List<PointApp> points = new List<PointApp>();
        private List<Line> lines = new List<Line>();


        private List<PointApp> Rotate(float alpha)
        {
            alpha = (float)((Math.PI * alpha) / 180);
            float[] center = new float[3];
            List<PointApp> RotateList = new List<PointApp>();

            foreach (PointApp point in points)
            {
                center[0] += point.X;
                center[1] += point.Y;
            }
            center[0] /= points.Count();
            center[1] /= points.Count();
            center[2] = 0;

            float[] RotateMatrix = new float[9] { (float)Math.Cos(alpha),         (float)Math.Sin(alpha),    0,
                                                      (float)Math.Sin(alpha) * -1,    (float)Math.Cos(alpha),    0,
                                                      center[0],                      center[1],                 1 };



            foreach (PointApp point in points)
            {
                float[] point_matrix = new float[3] { point.X, point.Y, 1 };
                float[] res_point = new float[2];

                for (int i = 0; i < 2; i += 1)
                {
                    for (int j = 0; j < 3; j += 1)
                    {
                        res_point[i] += (point_matrix[j] - center[j]) * RotateMatrix[i + j * 3];
                    }
                }

                RotateList.Add(new PointApp(res_point[0], res_point[1]));
            }

            return RotateList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filename = "";
            if (domainUpDown1.SelectedItem.Equals("Куст 1"))
            {
                filename = "files/bush_1.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Куст 2"))
            {
                filename = "files/bush_2.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Куст 3"))
            {
                filename = "files/bush_3.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("Снежинка Коха"))
            {
                filename = "files/kohs_nowflake.txt";
            }
            else if (domainUpDown1.SelectedItem.Equals("6-тиугольная мозаика"))
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
            

           StreamReader f = new StreamReader(filename);
            List<string> s = f.ReadLine().Split().ToList();

            if(s[0].Length == 1)
                aksiom = char.Parse(s[0]);
            else//если аксиома - не один элемент
                aksiom = s[0][0];

            corner = float.Parse(s[1]);
            direction = float.Parse(s[2]);

            rules = new List<string[]>();
            while (!f.EndOfStream)
            {
                rules.Add(f.ReadLine().Split());
            }
            f.Close();

            Paint_fractal(rules);
        }

        PointApp end;
        int line_lenght = 50;
        int iter = 0;
        int step = 0;
        public void Paint_fractal(List<string[]> rules)
        {
            
            // правило аксиомы
            string rule = rules.Where(x => char.Parse(x[0]) == aksiom).ToList()[0][2];

            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] == aksiom)
                {
                    PointApp p1 = step == 0 ? new PointApp(100, pictureBox1.Height - 100) : end;
                    PointApp p2 = new PointApp(p1.X + line_lenght, p1.Y);
                    points.Add(p2);
                    p2 = Rotate(direction).First();
                    end = p2;
                    points.Clear();
                    Line line = new Line(p1, p2);
                    lines.Add(line);

                    step++;
                }
                else if (rule[i] == '+')
                {
                    direction += corner;
                }
                else if (rule[i] == '-')
                {
                    direction -= corner;
                }
                else if (rule[i] == '[')
                {

                }
                else if (rule[i] == ']')
                {

                }
                else // переменная - неаксиома
                {
                    rule = rules.Where(x => char.Parse(x[0]) == rule[i]).ToList()[0][2];
                    recurs(rule, rule[i]);
                }
            }

            iter++;
            if (iter != int.Parse(iteration_count.Text))
                Paint_fractal(rules);

            this.lines.ForEach((p) => p.Draw(g));

            rules.Clear();
        }

        public void recurs(string rule, char symbol)
        {
            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] == symbol)
                {
                    PointApp p1 = iter == 0 ? new PointApp(100, pictureBox1.Height - 100) : end;
                    PointApp p2 = new PointApp(p1.X + line_lenght, p1.Y);
                    points.Add(p2);
                    p2 = Rotate(direction).First();
                    end = p2;
                    points.Clear();
                    Line line = new Line(p1, p2);
                    line.Draw(g);

                    step++;
                }
                else if (rule[i] == '+')
                {
                    direction += corner;
                }
                else if (rule[i] == '-')
                {
                    direction -= corner;
                }
                else if (rule[i] == '[')
                {

                }
                else if (rule[i] == ']')
                {

                }
                else // другая переменная
                {
                    rule = rules.Where(x => char.Parse(x[0]) == rule[i]).ToList()[0][2];
                    recurs(rule, rule[i]);
                }
            }
        }

        
    }
}
