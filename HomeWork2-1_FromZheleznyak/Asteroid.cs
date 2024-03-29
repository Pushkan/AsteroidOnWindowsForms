﻿using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{

    class Asteroid : BaseObject
    {
        //Подсчет баллов за сбитые астероиды
        public static int score = 0;

        public static event Message MessageDie;
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            //Пропишем получение изображения лишь раз в конструкторе, чтобы не забивать память
            baseImage = Image.FromFile("img/asteroid.png");
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
        public void Die()
        {
            MessageDie?.Invoke();
        }
    }

}
