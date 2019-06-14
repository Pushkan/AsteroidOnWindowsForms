using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    class Star:BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            baseImage = Image.FromFile("pluto.png");
        }     
        //Так как у нас абстрактная фунция, её обязательно нужно прописать
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(baseImage, Pos.X, Pos.Y, 20, 20);
        }
        //Если звезда долетает до конца экрана, то перемещаем её в конец
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;         
            if (Pos.X < 0) Pos.X = Game.Width;
        }

    }
}
