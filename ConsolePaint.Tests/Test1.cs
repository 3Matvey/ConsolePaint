using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsolePaint;
using ConsolePaint.Shapes;
using ConsolePaint.Commands;

namespace ConsolePaintTests
{
    // Фиктивная реализация ICanvas для тестов
    public class FakeCanvas : ICanvas
    {
        public List<Shape> Shapes { get; } = new List<Shape>();
        public int Width { get; set; } = 80;
        public int Height { get; set; } = 25;
        public List<string> Log { get; } = new List<string>();

        public void DrawFrame() => Log.Add("DrawFrame");
        public void Draw(Shape shape) => Log.Add("Draw");
        public void Fill(Shape shape) => Log.Add("Fill");
        public void Clear() => Log.Add("Clear");
        public void EraseShape(Shape shape) => Log.Add("EraseShape");
        public void AddShape(Shape shape) { Shapes.Add(shape); Log.Add("AddShape"); }
        public void RemoveShape(Shape shape) { Shapes.Remove(shape); Log.Add("RemoveShape"); }
        public void RedrawAllShapes() => Log.Add("RedrawAllShapes");
        public void SetPixel(int x, int y, char symbol, ConsoleColor color) => Log.Add($"SetPixel({x},{y},{symbol},{color})");
        public Pixel GetPixel(int x, int y) => new Pixel(x, y, ' ', ConsoleColor.Black);
    }

    // Тестовая реализация фигуры
    public class TestShape : Shape
    {
        public List<Pixel> innerPixels = new List<Pixel>();
        public List<Pixel> outerPixels = new List<Pixel>();


        // Для проверки перемещения фигуры
        public int X { get; set; }
        public int Y { get; set; }

        public override void Move(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }
        protected override void CalculatePixels()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class AddShapeActionTests
    {
        [TestMethod]
        public void Execute_AddsShapeAndRedrawsCanvas()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape();
            var action = new AddShapeAction(canvas, shape);

            // Act
            action.Execute();

            // Assert
            CollectionAssert.Contains(canvas.Shapes, shape, "Фигура должна быть добавлена в канву.");
            CollectionAssert.Contains(canvas.Log, "AddShape", "Метод AddShape должен быть вызван.");
            CollectionAssert.Contains(canvas.Log, "RedrawAllShapes", "Метод RedrawAllShapes должен быть вызван.");
        }

        [TestMethod]
        public void Undo_RemovesShapeAndRedrawsCanvas()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape();
            canvas.Shapes.Add(shape); // эмулируем, что фигура уже добавлена
            var action = new AddShapeAction(canvas, shape);

            // Act
            action.Undo();

            // Assert
            CollectionAssert.DoesNotContain(canvas.Shapes, shape, "Фигура должна быть удалена из канвы.");
            CollectionAssert.Contains(canvas.Log, "RemoveShape", "Метод RemoveShape должен быть вызван.");
            CollectionAssert.Contains(canvas.Log, "RedrawAllShapes", "Метод RedrawAllShapes должен быть вызван.");
        }
    }

    [TestClass]
    public class RemoveShapeActionTests
    {
        [TestMethod]
        public void Execute_RemovesShapeAndRedrawsCanvas()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape();
            canvas.Shapes.Add(shape); // Фигура добавлена изначально
            var action = new RemoveShapeAction(canvas, shape);

            // Act
            action.Execute();

            // Assert
            CollectionAssert.DoesNotContain(canvas.Shapes, shape, "Фигура должна быть удалена из канвы.");
            CollectionAssert.Contains(canvas.Log, "RemoveShape", "Метод RemoveShape должен быть вызван.");
            CollectionAssert.Contains(canvas.Log, "RedrawAllShapes", "Метод RedrawAllShapes должен быть вызван.");
        }

        [TestMethod]
        public void Undo_AddsShapeAndRedrawsCanvas()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape();
            var action = new RemoveShapeAction(canvas, shape);

            // Act
            action.Undo();

            // Assert
            CollectionAssert.Contains(canvas.Shapes, shape, "Фигура должна быть добавлена обратно в канву.");
            CollectionAssert.Contains(canvas.Log, "AddShape", "Метод AddShape должен быть вызван.");
            CollectionAssert.Contains(canvas.Log, "RedrawAllShapes", "Метод RedrawAllShapes должен быть вызван.");
        }
    }

    [TestClass]
    public class MoveShapeActionTests
    {
        [TestMethod]
        public void Execute_MovesShapeAndRedrawsCanvas()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape { X = 0, Y = 0 };
            int dx = 5, dy = 3;
            var action = new MoveShapeAction(canvas, shape, dx, dy);

            // Act
            action.Execute();

            // Assert
            Assert.AreEqual(5, shape.X, "Координата X должна увеличиться на dx.");
            Assert.AreEqual(3, shape.Y, "Координата Y должна увеличиться на dy.");
            CollectionAssert.Contains(canvas.Log, "EraseShape", "Метод EraseShape должен быть вызван.");
            CollectionAssert.Contains(canvas.Log, "RedrawAllShapes", "Метод RedrawAllShapes должен быть вызван.");
        }

        [TestMethod]
        public void Undo_MovesShapeBackAndRedrawsCanvas()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape { X = 10, Y = 10 };
            int dx = 4, dy = -2;
            var action = new MoveShapeAction(canvas, shape, dx, dy);

            // Сначала выполняем перемещение
            action.Execute();
            // Очищаем лог, чтобы тестировать только вызовы в Undo
            canvas.Log.Clear();

            // Act
            action.Undo();

            // Assert: фигура возвращается в исходную позицию
            Assert.AreEqual(10, shape.X, "Координата X должна вернуться к исходной после Undo.");
            Assert.AreEqual(10, shape.Y, "Координата Y должна вернуться к исходной после Undo.");
            CollectionAssert.Contains(canvas.Log, "EraseShape", "Метод EraseShape должен быть вызван при Undo.");
            CollectionAssert.Contains(canvas.Log, "RedrawAllShapes", "Метод RedrawAllShapes должен быть вызван при Undo.");
        }
    }

    [TestClass]
    public class FillShapeActionTests
    {
        [TestMethod]
        public void Execute_FillsShapePixelsAndCallsCanvasFill()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape();
            // Добавляем несколько пикселей с исходными значениями
            shape.innerPixels.Add(new Pixel(0, 0, 'x', ConsoleColor.White));
            shape.innerPixels.Add(new Pixel(1, 1, 'x', ConsoleColor.White));

            char newSymbol = 'x';
            ConsoleColor newColor = ConsoleColor.White;
            var action = new FillShapeAction(canvas, shape, newSymbol, newColor);

            // Act
            action.Execute();

            // Assert: проверяем, что все пиксели изменены
            foreach (var pixel in shape.innerPixels)
            {
                Assert.AreEqual(newSymbol, pixel.Symbol, "Символ пикселя должен измениться.");
                Assert.AreEqual(newColor, pixel.Color, "Цвет пикселя должен измениться.");
            }
            CollectionAssert.Contains(canvas.Log, "Fill", "Метод Fill канвы должен быть вызван.");
        }

        [TestMethod]
        public void Undo_RestoresOriginalPixelsAndCallsCanvasFill()
        {
            // Arrange
            var canvas = new FakeCanvas();
            var shape = new TestShape();
            shape.innerPixels.Add(new Pixel(0, 0, 'a', ConsoleColor.White));
            shape.innerPixels.Add(new Pixel(1, 1, 'b', ConsoleColor.White));

            char newSymbol = 'x';
            ConsoleColor newColor = ConsoleColor.Red;
            var action = new FillShapeAction(canvas, shape, newSymbol, newColor);

            // Выполняем заливку и затем отмену
            action.Execute();
            action.Undo();

            // Assert: исходные значения должны восстановиться
            Assert.AreEqual('a', shape.innerPixels[0].Symbol, "Первый пиксель должен вернуть исходный символ.");
            Assert.AreEqual(ConsoleColor.White, shape.innerPixels[0].Color, "Первый пиксель должен вернуть исходный цвет.");
            Assert.AreEqual('b', shape.innerPixels[1].Symbol, "Второй пиксель должен вернуть исходный символ.");
            Assert.AreEqual(ConsoleColor.White, shape.innerPixels[1].Color, "Второй пиксель должен вернуть исходный цвет.");
            CollectionAssert.Contains(canvas.Log, "Fill", "Метод Fill канвы должен быть вызван при Undo.");
        }
    }
}
