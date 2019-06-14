using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    class Cat : BaseObject
    {
        public Cat(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        new Image baseImage = Image.FromFile("cat.png");

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(baseImage, Pos);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }

    }
}
