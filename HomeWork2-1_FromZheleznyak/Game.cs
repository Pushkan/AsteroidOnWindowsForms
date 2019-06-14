using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

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
        private static int countOfAsteroids;
        public static BaseObject[] _objs;
        //private static Bullet _bullet;
        private static List<Bullet> _bullets = new List<Bullet>();
        //private static Asteroid[] _asteroids;
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        private static Health[] _healths;
        //Создание корабля
        private static Ship _ship = new Ship(new Point(10, 200), new Point(10, 5), new Size(45, 15));
        //Объявляем таймертик, чтобы его можно было прописать в финише
        private static Timer _timer = new Timer();
        //Создаем переменную СтримВрайтер для ведение журнала
        private static StreamWriter f;
        
        static Game()
        {
        }
        public static void Load()
        {
            var rnd = new Random();
            //Создаем звезды и астероиды
            _objs = new BaseObject[10];
            //_asteroids = new Asteroid[countOfAsteroids];
            _healths = new Health[3];
            
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                int pointX = rnd.Next(0, Game.Width);
                int pointY = rnd.Next(0, Game.Height);
                try
                {
                    //Создаем звёзды. Если они находятся за пределами экрана, тогда вызывает своё исключение
                    _objs[i] = new Star(new Point(pointX, pointY), new Point(-r, r), new Size(3, 3));
                    if (pointX < 0 || pointY < 0) throw new MyException();
                }
                catch (MyException)
                {
                    Console.WriteLine($"{DateTime.Now} : Звезда за пределами экрана");
                };
                
            }
            //Создание астероидов
            for (var i = 0; i < countOfAsteroids; i++)
            {
                int r = rnd.Next(40, 90);
                Asteroid newAsteroid = new Asteroid(new Point(Width, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
                _asteroids.Add(newAsteroid);
            }
            //Создание аптечек
            for (var i = 0; i < _healths.Length; i++)
            {
                int r = rnd.Next(30, 50);
                _healths[i] = new Health(new Point(Width, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r), r);
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
            _timer.Interval = 100;
            _timer.Start();
            _timer.Tick += Timer_Tick;

            //Количество астероидов на первом уровне
            countOfAsteroids = 3;

            //Load();
            form.KeyDown += Form_KeyDown;
            //В ивент МессейджДай добавляем метод Финиш
            Ship.MessageDie += Finish;
            //В делегат МессейджАндерАттак добавляем метод выведения урона в консоль
            Ship.MessageUnderAttack += AttackLog;

            //Тоже самое для лечения
            Ship.MessageAddEnergy += HealthLog;            

            //В делегат добавляем ведение журнала ещё и в файл

            //Обозначим в файле начало игры
            f = new StreamWriter("logs.txt", true);
            f.WriteLine($"{DateTime.Now} : Game has been started");

            Ship.MessageUnderAttack += AttackLogFile;
            Ship.MessageAddEnergy += HealthLogFile;

            //В ивент МессайджДай у астероида добавляем метод выведения надписи в консоль и файл
            Asteroid.MessageDie += DieLog;
            Asteroid.MessageDie += DieLogFile;
            Asteroid.MessageDie += () => Asteroid.score++;
        }
        public static void Draw()
        {
            //Очистка экрана
            Buffer.Graphics.Clear(Color.Black);
            //Вывод звёзд
            foreach (BaseObject obj in _objs)
                obj.Draw();
            //Вывод аптечек (переместил выше, чтобы не перекрывали астероиды)
            foreach (Health h in _healths)
                h?.Draw();
            //Вывод Астероидов
            foreach (Asteroid a in _asteroids)
                a?.Draw();
            //Вывод пули, корабля
            foreach (Bullet b in _bullets) b?.Draw();
            _ship?.Draw();
            //Если есть корабль, выводим его энергию
            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString("Score:" + Asteroid.score, SystemFonts.DefaultFont, Brushes.White, 100, 0);
            }
            //Рендер буффера
            Buffer.Render();

        }
        public static void Update()
        {
            //Апдейт каждой звезды
            foreach (BaseObject obj in _objs) obj.Update();
            //Апдейт каждого снаряда
            foreach (Bullet b in _bullets) b?.Update();
            //Апдейт каждого астероида
            for (var i = 0; i < _asteroids.Count; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                //Проверяем, не столкнулась ли пуля с астероидом
                for (int j = 0; j < _bullets.Count; j++)
                    if (_bullets[j] != null && _asteroids[i] != null && _bullets[j].Collision(_asteroids[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        //Вызываем ивенты смерти
                        _asteroids[i].Die();
                        _asteroids[i] = null;
                        _bullets.RemoveAt(j);
                        j--;
                    }
                //Дополнительная проверка, так как астероид мог уничтожиться чуть выше
                if (_asteroids[i] == null) continue;
                //Проверяем, не столкнулся ли астероид с кораблём
                if (!_ship.Collision(_asteroids[i])) continue;
                var rnd = new Random();
                //Наносим урон кораблю
                _ship?.EnergyLow(rnd.Next(1, 10));
                System.Media.SystemSounds.Asterisk.Play();
                //Проверяем, не ушла ли энергия корабля в ноль или меньше
                if (_ship.Energy <= 0) _ship?.Die();
            }
            for (int i = 0; i < _healths.Length; i++)
            {
                if (_healths[i] == null) continue;
                _healths[i].Update();
                if (!_ship.Collision(_healths[i])) continue;
                var rnd = new Random();
                //Лечим корабль
                _ship?.EnergyAdd(_healths[i].power);
                _healths[i] = null;                
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
            //Заканчиваем игру, если сбиты все астероиды
            //Временная заглушка для окончания игры
            if (Asteroid.score == countOfAsteroids)
            {
                Finish();
            }
        }


        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            //При нажатии клавиш назначаем события
            //Переделал создание снаряда так, чтобы он вылетал из середины корабля
            if (e.KeyCode == Keys.ControlKey) _bullets.Add(new Bullet(new Point(_ship.Rect.X + _ship.Rect.Width / 2, _ship.Rect.Y + _ship.Rect.Height), new Point(20, 0), new Size(4, 1)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
        }


        public static void Finish()
        {
            _timer.Stop();
            //Добавил ещё одну отрисовку, чтобы было понятно, что энергия корабля ушла в минус
            Draw();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
            //Закрываем файл, чтобы сохранить логи
            f.WriteLine($"{DateTime.Now} : Game ended!");
            f.Close();
        }

        private static void AttackLog(int damage)
        {
            //Выводим в консоль, сколько корабль получил урона
            Console.WriteLine($"{DateTime.Now} : Ship under attack (-{damage})");
        }

        private static void HealthLog(int damage)
        {
            //Выводим в консоль, на сколько полечился корабль
            Console.WriteLine($"{DateTime.Now} : Ship healed (+{damage})");
        }

        private static void AttackLogFile(int damage)
        {
            //Выводим в файл, сколько корабль получил урона
            f.WriteLine($"{DateTime.Now} : Ship under attack (-{damage})");
        }

        private static void HealthLogFile(int damage)
        {
            //Выводим в файл, на сколько полечился корабль
            f.WriteLine($"{DateTime.Now} : Ship healed (+{damage})");
        }

        private static void DieLog()
        {
            //Выводим в консоль, сколько корабль получил урона
            Console.WriteLine($"{DateTime.Now} : Asteroid has been destroed");
        }

        private static void DieLogFile()
        {
            //Выводим в файл, сколько корабль получил урона
            f.WriteLine($"{DateTime.Now} : Asteroid has been destroed");
        }
    }
}
