using System;
using System.Windows.Forms;

namespace HomeWork2_1_FromZheleznyak
{
    /// <summary>
    /// TODO:
    ///     * Добавить выбор картинки Звезды в зависимости от размера.
    ///     * Автогенерируемые уровни, надпись уровня
    ///     * Запись в файл по строкам
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width-300,
                Height = Screen.PrimaryScreen.Bounds.Height-300
            };
            try
            {
                //Вызов исключения АутОфРейндж
                if (form.Width > 1000 || form.Height > 1000 || form.Width < 0 || form.Height < 0) throw new ArgumentOutOfRangeException();                                
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine(@"Высота/Ширина не может быть больше 1000");
            }
            finally
            {
                //Выносить это в Файнали нет никакого смысла. Просто показываю, что я знаю о существовании "файнали" =D
                Game.Init(form);
                form.Show();
                Game.Load();
                Game.Draw();
                Application.Run(form);
            };
        }
    }
}
