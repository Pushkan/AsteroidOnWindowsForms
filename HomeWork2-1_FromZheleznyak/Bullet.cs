using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    class Bullet : BaseObject
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            //Переделал апдейт так, чтобы Dir влияла на скорость снаряда, 
            //однако теперь при больших скоростях снаряда возможна ситуация, когда снаряд пролетает маленькие астероиды насквозь.
            //Dir.X не должна быть больше минимального размера астероида
            Pos.X = Pos.X + Dir.X;
        }
    }
}
