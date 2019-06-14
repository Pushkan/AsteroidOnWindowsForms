﻿using System;
using System.IO;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        //Для получения изображения свинюшки подключаем System.IO, создаем переменную типа Image, получаем её
        Image baseImage = Image.FromFile("pig.png");
        public virtual void Draw()
        {
            //Game.Buffer.Graphics.DrawEllipse(Pens.White, Pos.X, Pos.Y, Size.Width, Size.Height);            
            //Выводим свинюшку в заданную позицию
            Game.Buffer.Graphics.DrawImage(baseImage, Pos);
        }
        public virtual void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }
    }
}