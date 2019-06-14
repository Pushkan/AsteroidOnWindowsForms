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
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);

        }
    }
}
