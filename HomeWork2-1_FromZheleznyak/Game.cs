using System;
using System.Windows.Forms;
using System.Drawing;

namespace HomeWork2_1_FromZheleznyak
{
    //Своё исключение
    class MyException : Exception
    {
        public MyException()
        {
            Console.WriteLine(base.Message);
        }
    }

    class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static BaseObject[] _objs;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        static Game()
        {
        }
        public static void Load()
        {
            var rnd = new Random();
            //Создаем 30 звезд, 1 пулю и 3 астероида
            _objs = new BaseObject[30];
            _bullet = new Bullet(new Point(0, rnd.Next(0, Height)), new Point(5, 0), new Size(4, 1));
            _asteroids = new Asteroid[3];
            
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                //Специально делаю рандом от -200, чтобы иногда вызывать исключение
                int pointX = rnd.Next(-200, Game.Width);
                int pointY = rnd.Next(-200, Game.Height);
                try
                {
                    //Создаем звёзды. Если они находятся за пределами экрана, тогда вызывает своё исключение
                    _objs[i] = new Star(new Point(pointX, pointY), new Point(-r, r), new Size(3, 3));
                    if (pointX < 0 || pointY < 0) throw new MyException();
                }
                catch (MyException)
                {
                    Console.WriteLine(@"Вызываем собственное исключение. Звезда за пределами экрана");
                };
                
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(40, 90);
                _asteroids[i] = new Asteroid(new Point(Width, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
            }

        }
        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            //Каждые 100 миллисекунд вызываем ТаймерТик()
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
            Load();
        }
        public static void Draw()
        {
            //Закрашиваем в черный
            Buffer.Graphics.Clear(Color.Black);
            //Отрисовываем звёзды, астероиды, пулю
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (BaseObject obj in _asteroids)
                obj.Draw();
            _bullet.Draw();
            Buffer.Render();

        }
        public static void Update()
        {

            foreach (BaseObject obj in _objs)
                obj.Update();
            //Пишем, что это Астероиды, чтобы у нас был возможен вызов "Регенерейт". 
            //Можно было проверять на тип, но в нашем случае это не имеет смысла, все объекты - астероиды
            foreach (Asteroid obj in _asteroids)
            {
                obj.Update();
                //Проверяем столкновения
                if (obj.Collision(_bullet))
                {
                    //При столкновении появляется в другом месте
                    obj.Regenerate();
                }
            }
                
            _bullet.Update();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}
