using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsolePaint.Services
{
    public static class FileManager
    {
        /// <summary>
        /// Сохраняет список фигур в файл JSON, используя обёртку для сохранения информации о типе.
        /// </summary>
        public static void SaveShapesToFile(List<Shape> shapes, string filename)
        {
            ArgumentNullException.ThrowIfNull(shapes);
            ArgumentNullException.ThrowIfNull(filename);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

            var wrappers = new List<ShapeWrapper>();
            foreach (var shape in shapes)
            {
                if (shape == null)
                    continue;

                string typeName = shape.GetType().AssemblyQualifiedName ?? shape.GetType().FullName ?? "UnknownType";

                string shapeJson = JsonSerializer.Serialize(shape, shape.GetType(), options);

                var wrapper = new ShapeWrapper
                {
                    Type = typeName,
                    Json = shapeJson
                };
                wrappers.Add(wrapper);
            }

            string json = JsonSerializer.Serialize(wrappers, options);
            File.WriteAllText(filename, json);
        }

        /// <summary>
        /// Загружает список фигур из файла JSON, восстанавливая типы объектов.
        /// </summary>
        public static List<Shape> LoadShapesFromFile(string filename)
        {
            // Если filename null или пуст, или файл не существует — возвращаем пустой список
            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                return [];
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

            string json = File.ReadAllText(filename);

            // Десериализация может вернуть null, если JSON пуст или некорректен
            List<ShapeWrapper>? wrappers = JsonSerializer.Deserialize<List<ShapeWrapper>>(json, options);
            if (wrappers == null)
            {
                return [];
            }

            var shapes = new List<Shape>();
            foreach (var wrapper in wrappers)
            {
                if (wrapper == null)
                    continue;

                if (string.IsNullOrWhiteSpace(wrapper.Type) || string.IsNullOrWhiteSpace(wrapper.Json))
                {
                    continue;
                }

                Type? type = Type.GetType(wrapper.Type);
                if (type == null)
                {
                    continue;
                }

                object? deserialized = JsonSerializer.Deserialize(wrapper.Json, type, options);
                if (deserialized is Shape shape)
                {
                    shapes.Add(shape);
                }
            }
            return shapes;
        }
    }
    /// <summary>
    /// Обёртка для сохранения информации о типе фигуры.
    /// </summary>
    file class ShapeWrapper
    {
        public string Type { get; init; } = string.Empty;  //=>init
        public string Json { get; init; } = string.Empty;
    }
}
