using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsolePaint
{
    public static class FileManager
    {
        /// <summary>
        /// Сохраняет список фигур в файл JSON, используя обёртку для сохранения информации о типе.
        /// </summary>
        public static void SaveShapesToFile(List<Shape> shapes, string filename)
        {
            // Проверим, чтобы не было null
            if (shapes == null) throw new ArgumentNullException(nameof(shapes));
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };

            var wrappers = new List<ShapeWrapper>();
            foreach (var shape in shapes)
            {
                // Защита от случайного null
                if (shape == null)
                    continue;

                // AssemblyQualifiedName обычно не null, но перестрахуемся:
                string typeName = shape.GetType().AssemblyQualifiedName ?? shape.GetType().FullName ?? "UnknownType";

                // Сериализуем саму фигуру
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
                return new List<Shape>();
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
                return new List<Shape>();
            }

            var shapes = new List<Shape>();
            foreach (var wrapper in wrappers)
            {
                // Если элемент списка оказался null — пропускаем
                if (wrapper == null)
                    continue;

                // Проверяем поля на null
                if (string.IsNullOrWhiteSpace(wrapper.Type) || string.IsNullOrWhiteSpace(wrapper.Json))
                {
                    // Можно логировать предупреждение, но здесь просто пропустим
                    continue;
                }

                // Попробуем получить Type по строке
                Type? type = Type.GetType(wrapper.Type);
                if (type == null)
                {
                    // Если тип не найден (например, переименовали класс) — пропускаем
                    continue;
                }

                // Десериализуем как object, потом проверяем, что это Shape
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
        public string Type { get; set; } = string.Empty;
        public string Json { get; set; } = string.Empty;
    }
}
