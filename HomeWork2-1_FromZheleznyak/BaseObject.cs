using System;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    abstract class BaseObject:ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;       
        protected Image baseImage;
        public delegate void Message();

        protected BaseObject(Point pos, Point dir, Size size)
        {
            //Стандартный конструктор для создания BaseObject
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        //Прописываем абстрактные функции для обязательного переопределения их в производных классах
        public abstract void Draw();

        public abstract void Update();
        //Подключаем коллизию
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        public Rectangle Rect => new Rectangle(Pos, Size);

    }
}
