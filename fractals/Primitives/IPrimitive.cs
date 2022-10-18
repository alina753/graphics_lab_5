using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba4.Primitives
{
    class IPrimitive
    {
        interface IPrimivite
        {
            void Draw(Graphics g, bool selected);
            void Transform(Transformation t);
        }
    }
}
