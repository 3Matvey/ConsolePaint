namespace ConsolePaint;

public class Terminal
{
    private Canvas canvas;
    private Menu menu;

    private int cursorX;
    private int cursorY;
    private int canvasWidth;
    private int canvasHeight;

    // Храним список фигур в Menu (или внутри Canvas), 
    // здесь предполагается, что Menu хранит фигуры и возвращает их методом GetShapes().

    public Terminal(int canvasWidth, int canvasHeight)
    {
        this.canvasWidth = canvasWidth;
        this.canvasHeight = canvasHeight;
        canvas = new Canvas(canvasWidth, canvasHeight);
        menu = new Menu(canvas);

        // Изначально ставим курсор в (0,0) внутри холста
        cursorX = 0;
        cursorY = 0;
    }

    public void Run()
    {
        Console.Clear();
        // Первичная отрисовка:
        canvas.DrawFrame();                // Рамка
        canvas.DrawShapes(menu.GetShapes()); // Фигуры
        DrawMenu();                       // Меню
        DrawCursor();                     // Курсор

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveCursor(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    MoveCursor(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    MoveCursor(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    MoveCursor(1, 0);
                    break;

                case ConsoleKey.D:
                    // Полное обновление экрана после вызова меню
                    HandleMenuCall();
                    break;

                case ConsoleKey.Escape:
                    return; // Выход из приложения
            }
        }
    }

    /// <summary>
    /// Частично обновляем только курсор.
    /// 1) Стираем старый курсор
    /// 2) Изменяем координаты (с учётом ограничений)
    /// 3) Рисуем курсор на новом месте
    /// </summary>
    private void MoveCursor(int dx, int dy)
    {
        // Стираем старый курсор
        EraseCursor();

        // Меняем координаты (с ограничениями)
        cursorX = Math.Max(0, Math.Min(cursorX + dx, canvasWidth - 1));
        cursorY = Math.Max(0, Math.Min(cursorY + dy, canvasHeight - 1));

        // Рисуем новый курсор
        DrawCursor();
    }

    /// <summary>
    /// При вызове меню (например, создание фигур), 
    /// мы делаем полную перерисовку экрана (рамка, фигуры, меню, курсор).
    /// </summary>
    private void HandleMenuCall()
    {
        // Меню может добавить новые фигуры, очистить холст и т.д.
        menu.ShowMenu();

        // Полностью обновляем отображение:
        Console.Clear();
        canvas.DrawFrame();
        canvas.DrawShapes(menu.GetShapes());
        DrawMenu();
        DrawCursor();
    }

    // Стираем курсор, возвращая символ, который там был
    private void EraseCursor()
    {
        // Получаем пиксель, который сейчас хранится в Canvas на месте старого курсора
        Pixel oldPixel = canvas.GetPixel(cursorX, cursorY);

        // Перемещаем курсор консоли в позицию (cursorX+1, cursorY+1)
        Console.SetCursorPosition(cursorX + 1, cursorY + 1);

        // Ставим цвет пикселя
        Console.ForegroundColor = oldPixel.Color;

        // Выводим символ, который там был
        Console.Write(oldPixel.Symbol);

        // Возвращаем цвет по умолчанию
        Console.ForegroundColor = ConsoleColor.White;
    }


    // Рисуем курсор
    private void DrawCursor()
    {
        int drawX = cursorX + 1;
        int drawY = cursorY + 1;

        // Сохраняем позицию консоли (не обязательно)
        int prevLeft = Console.CursorLeft;
        int prevTop = Console.CursorTop;

        Console.SetCursorPosition(drawX, drawY);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("_");  // Символ курсора
        Console.ForegroundColor = ConsoleColor.White;

        // Возвращаемся в прежнее место (не обязательно)
        Console.SetCursorPosition(prevLeft, prevTop);
    }

    private void DrawMenu()
    {
        // Меню располагается под холстом, значит начинаем с (0, canvasHeight+2)
        Console.SetCursorPosition(0, canvasHeight + 2);
        Console.WriteLine("Меню: [D] - Нарисовать фигуру, [Esc] - Выход. Стрелки: перемещение курсора");
    }
}
