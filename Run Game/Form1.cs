namespace Run_Game
{
    public partial class Form1 : Form
    {
        /* глобальные переменные */
        int gravity;
        int gravityValue = 8;
        int obstacleSpeed = 10;
        int score = 0;
        int highScore = 0;
        bool gameOver = false; /* для проверки, запущена еще игра или нет */
        Random random = new Random();

        public Form1()
        {
            InitializeComponent(); /* загрузка конструктора */
            RestartGame();
        }
        private void GameTimerEvent(object sender, EventArgs e)
        {
            lblScore.Text = "Score: " + score;
            lblhighScore.Text = "High Score: " + highScore;
            player.Top += gravity;
            /* когда игрок приземляется на платформы. */
            if (player.Top > 343)
            {
                gravity = 0; /* не даем игроку падать вниз */
                player.Top = 343; /* останавливаем игрока в точке где он находится */
                player.Image = Properties.Resources.run_down0; /* бег снизу */
            }
            else if (player.Top < 38)
            {
                gravity = 0; /* останавливаем движение вверх */
                player.Top = 38; /* останавливаем игрока в точке где он находится */
                player.Image = Properties.Resources.run_up0; /* бег сверху */
            }
            /* перемещать препятствия */
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;
                    if (x.Left < -100)
                    {
                        x.Left = random.Next(1200, 3000);
                        score += 1;
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        gameTimer.Stop();
                        lblScore.Text += " Game Over!! Press Enter to Restart.";
                        gameOver = true;
                        /* устанавливаем наивысший бал */
                        if (score > highScore)
                        {
                            highScore = score;
                        }
                    }
                }
            }
            /* увеличиваем скорость игрока и препятствий */
            if (score > 10)
            {
                obstacleSpeed = 20;
                gravityValue = 12;
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e) /* настройка работы клавиш */
        {
            if (e.KeyCode == Keys.Space)
            {
                if (player.Top == 342) /* нахождение на нижней платформе */
                {
                    player.Top -= 10;
                    gravity = -gravityValue;
                }
                else if (player.Top == 38) /* нахождение на верхней платформе */
                {
                    player.Top += 10;
                    gravity = gravityValue;
                }
            }
            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }
        private void RestartGame()  /* сброс настроек и убирает препятствия с платформы когда начинается игра */
        {
            lblScore.Parent = pictureBox1;
            lblhighScore.Parent = pictureBox2;
            lblhighScore.Top = 0; /* высший бал равен нулю */
            player.Location = new Point(180, 149); /* начальное местоположение игрока */
            player.Image = Properties.Resources.run_down0;
            score = 0;
            gravityValue = 8;
            gravity = gravityValue;
            obstacleSpeed = 10;
            foreach (Control x in this.Controls) /* перемещаем препятствия случайным образом в пределах окна */
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left = random.Next(1200, 3000);
                }
            }
            gameTimer.Start(); /* таймер игры */
        }
    }
}
