using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsolePaint.Shapes;

namespace ConsolePaint
{
    public static class FileManager
    {
        /// <summary>
        /// Сохраняет список фигур в файл JSON, используя обёртку для сохранения информации о типе.
        /// </summary>
        public static void SaveShapesToFile(List<Shape> shapes, string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                // Преобразователь для перечислений (если используются)
                Converters = { new JsonStringEnumConverter() }
            };

            // Оборачиваем каждую фигуру в обёртку, которая хранит имя типа и JSON-представление фигуры.
            List<ShapeWrapper> wrappers = new List<ShapeWrapper>();
            foreach (var shape in shapes)
            {
                wrappers.Add(new ShapeWrapper
                {
                    Type = shape.GetType().AssemblyQualifiedName,
                    Json = JsonSerializer.Serialize(shape, shape.GetType(), options)
                });
            }

            string json = JsonSerializer.Serialize(wrappers, options);
            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// Загружает список фигур из файла JSON, восстанавливая типы объектов.
        /// </summary>
        public static List<Shape> LoadShapesFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                return new List<Shape>();
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            string json = File.ReadAllText(filename);
            List<ShapeWrapper> wrappers = JsonSerializer.Deserialize<List<ShapeWrapper>>(json, options);

            List<Shape> shapes = new List<Shape>();
            foreach (var wrapper in wrappers)
            {
                Type type = Type.GetType(wrapper.Type);
                if (type != null)
                {
                    Shape shape = (Shape)JsonSerializer.Deserialize(wrapper.Json, type, options);
                    shapes.Add(shape);
                }
            }
            return shapes;
        }
    }

    /// <summary>
    /// Обёртка для сохранения информации о типе фигуры.
    /// </summary>
    public class ShapeWrapper
    {
        public string Type { get; set; }
        public string Json { get; set; }
    }
}
