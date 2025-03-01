using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConsolePaint
{
    public static class FileManager
    {
        /// <summary>
        /// Сохраняет текущее состояние холста (рамка + внутренняя область) в текстовый файл.
        /// Для получения ширины и высоты холста используется рефлексия.
        /// </summary>
        /// <param name="canvas">Объект холста</param>
        /// <param name="filename">Имя файла для сохранения</param>
        public static void SaveCanvasToFile(Canvas canvas, string filename)
        {
            int width = canvas.Width;
            int height = canvas.Height;

            StringBuilder sb = new StringBuilder();

            // Формируем верхнюю границу (рамку)
            sb.Append(" " + new string('_', width) + Environment.NewLine);

            // Формируем строки внутренней области: каждая строка начинается и заканчивается символом "|"
            for (int y = 0; y < height; y++)
            {
                sb.Append("|");
                for (int x = 0; x < width; x++)
                {
                    // Получаем пиксель; если его нет, используем пробел
                    Pixel p = canvas.GetPixel(x, y);
                    char ch = p?.Symbol ?? ' ';
                    sb.Append(ch);
                }
                sb.Append("|" + Environment.NewLine);
            }

            // Формируем нижнюю границу
            sb.Append(" " + new string('_', width) + Environment.NewLine);

            // Записываем строку в файл
            File.WriteAllText(filename, sb.ToString());
        }
    }
}
