using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    class Ship:BaseObject
    {
        private int _energy = 100;
        public int Energy => _energy;
        public static event Message MessageDie;
        public delegate void MessageDamage(int Damage);
        public static MessageDamage MessageUnderAttack;
        public static MessageDamage MessageAddEnergy;
        private int maxEnergy = 120;

        public void EnergyLow(int n)
        {
            _energy -= n;
            MessageUnderAttack?.Invoke(n);
        }
        public void EnergyAdd(int n)
        {
            _energy += n;
            if (_energy >= maxEnergy) _energy = maxEnergy;

            MessageAddEnergy?.Invoke(n);
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            //Пропишем получение изображения лишь раз в конструкторе, чтобы не забивать память
            baseImage = Image.FromFile("img/ship.png");
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(baseImage, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }

    }
}
