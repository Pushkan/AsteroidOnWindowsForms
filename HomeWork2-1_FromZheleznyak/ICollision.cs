using System.Drawing;

namespace HomeWork2_1_FromZheleznyak {

    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }
}
