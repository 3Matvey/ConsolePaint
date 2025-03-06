using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsolePaint;

namespace ConsolePaintTests
{
    // Фиктивная реализация абстрактного класса Shape для тестирования
    public class DummyShape : Shape
    {
        public DummyShape() : base()
        {
            // Добавляем тестовые пиксели:
            // Один внешний пиксель в точке (0,0)
            OuterPixels.Add(new Pixel(0, 0, 'O', ConsoleColor.White));
            // Один внутренний пиксель в точке (1,1)
            InnerPixels.Add(new Pixel(1, 1, 'I', ConsoleColor.White));
        }

        // Для тестов достаточно пустой реализации
        protected override void CalculatePixels()
        {
            // Реальная логика не нужна для тестирования методов Move и ContainsPoint.
        }
    }

    [TestClass]
    public class ShapeTests
    {
        [TestMethod]
        public void Move_UpdatesPixelCoordinates()
        {
            // Arrange
            var shape = new DummyShape();
            int dx = 5, dy = 3;

            // Act: перемещаем фигуру
            shape.Move(dx, dy);

            // Assert: проверяем, что координаты пикселей обновились
            // Внешний пиксель: из (0,0) должен стать (5,3)
            Assert.AreEqual(5, shape.OuterPixels[0].X, "Внешний пиксель X не обновлён корректно.");
            Assert.AreEqual(3, shape.OuterPixels[0].Y, "Внешний пиксель Y не обновлён корректно.");
            // Внутренний пиксель: из (1,1) должен стать (6,4)
            Assert.AreEqual(6, shape.InnerPixels[0].X, "Внутренний пиксель X не обновлён корректно.");
            Assert.AreEqual(4, shape.InnerPixels[0].Y, "Внутренний пиксель Y не обновлён корректно.");
        }

        [TestMethod]
        public void ContainsPoint_ReturnsCorrectResult()
        {
            // Arrange
            var shape = new DummyShape();
            // До перемещения фигура содержит точку (0,0) и (1,1)

            // Act & Assert
            Assert.IsTrue(shape.ContainsPoint(0, 0), "Фигура должна содержать точку (0,0).");
            Assert.IsTrue(shape.ContainsPoint(1, 1), "Фигура должна содержать точку (1,1).");
            Assert.IsFalse(shape.ContainsPoint(2, 2), "Фигура не должна содержать точку (2,2).");

            // Дополнительно проверим после перемещения
            shape.Move(10, 10);
            Assert.IsTrue(shape.ContainsPoint(10, 10), "После перемещения фигура должна содержать точку (10,10) (бывший (0,0)).");
            Assert.IsTrue(shape.ContainsPoint(11, 11), "После перемещения фигура должна содержать точку (11,11) (бывший (1,1)).");
            Assert.IsFalse(shape.ContainsPoint(0, 0), "После перемещения фигура не должна содержать точку (0,0).");
        }

    }
}
