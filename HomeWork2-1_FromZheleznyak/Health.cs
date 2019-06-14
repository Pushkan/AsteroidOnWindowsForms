using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{

    class Health : BaseObject
    {
        public int power;
        public Health(Point pos, Point dir, Size size, int _power) : base(pos, dir, size)
        {
            power = _power;
            //Пропишем получение изображения лишь раз в конструкторе, чтобы не забивать память
            baseImage = Image.FromFile("img/health.png");
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(baseImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0)
            {
                Random r = new Random();
                Pos.X = Game.Width;
                Pos.Y = r.Next(0, Game.Height);
            }
        }
    }

}