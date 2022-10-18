using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba4.Primitives
{
    class Transformation
    {
        private float[] matrix;

        public Transformation()
        {
            this.matrix = new float[9]; // пустая матрица
        }

        public Transformation(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33)
        {
            matrix = new float[9] { a11, a12, a13, a21, a22, a23, a31, a32, a33 };
        }
        // добавить оператор *, поворот и масштабирование
    }
}
