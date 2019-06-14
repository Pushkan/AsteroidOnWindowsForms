using System;
using System.Windows.Forms;

namespace HomeWork2_1_FromZheleznyak
{
    /// <summary>
    /// 1. Добавить свои объекты в иерархию объектов, чтобы получился красивый задний фон, похожий на полет в звездном пространстве.
    /// 2. * Заменить кружочки картинками, используя метод DrawImage.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
            };
            try
            {
                //Вызов исключения АутОфРейндж
                if (form.Width > 1000 || form.Height > 1000 || form.Width < 0 || form.Height < 0) throw new ArgumentOutOfRangeException();                                
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Write(@"Высота/Ширина не может быть больше 1000");
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
