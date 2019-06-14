using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    class Star:BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        Image baseImage = Image.FromFile("octopus.png");
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(baseImage, Pos);
        }

    public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            Pos.Y = Pos.Y - Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }

    }
}
