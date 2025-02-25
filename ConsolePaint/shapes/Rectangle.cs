using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePaint.Shapes
{
    class Rectangle(int x1, int y1, int x2, int y2, char symbol) : Shape
    {
        public override void Draw(Canvas canvas)
        {
            // Рисуем верхнюю и нижнюю горизонтальные линии
            for (int x = x1; x <= x2; x++)
            {
                canvas.SetPixel(x, y1, symbol);
                canvas.SetPixel(x, y2, symbol);
            }

            // Рисуем боковые вертикальные линии
            for (int y = y1; y <= y2; y++)
            {
                canvas.SetPixel(x1, y, symbol);
                canvas.SetPixel(x2, y, symbol);
            }
        }
    }
}
