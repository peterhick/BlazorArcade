using Common.JsInterop;
using InvadersGame.Components;
using InvadersGame.Enums;
using InvadersGame.Helpers;
using InvadersGame.Models;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace InvadersGame
{
    public class InvadersGameModel : BlazorComponent
    {
        [Parameter]
        internal string DateTimeOutput { get; set; }
        [Parameter]
        internal int CountOutput { get; set; } = 0;
        [Parameter]
        internal string Var1 { get; set; }
        [Parameter]
        internal string Var2 { get; set; }
        [Parameter]
        internal string Var3 { get; set; }
        [Parameter]
        internal string Var4 { get; set; }

        [Parameter]
        internal Models.Player Player { get; set; }
        [Parameter]
        internal Models.Bullet PlayerBullet { get; set; } = new Models.Bullet();
        [Parameter]
        internal Models.Bullet EnemyBullet { get; set; } = new Models.Bullet();

        [Parameter]
        internal Models.Enemies Enemies { get; set; }
        [Parameter]
        internal Models.Mother Mother { get; set; }

        [Parameter]
        internal Models.Bunkers Bunkers { get; set; }

        [Parameter]
        internal ConsoleKey LastKeyPress { get; set; }

        [Parameter]
        internal int Score { get; set; } = 0;
        [Parameter]
        internal int Lives { get; set; } = 0;
        [Parameter]
        internal GameEnum GameStatus { get; set; }

        // Game.
        private List<ConsoleKey> KeysDown = new List<ConsoleKey>();
        private GameEnum SavedGameStatus;
        private int GameStatusTimer;
        private int GameCounter;

        private DateTime lastDateTime = DateTime.Now;
        internal int iterationCounter = 0;
        internal double actualFrameRate = 0;
        internal bool Render = true;

        private Timer timer;

        public string StopGame()
        {
            timer.AutoReset = false;
            timer.Enabled = false;
            timer.Stop();

            return string.Empty;
        }

        ///// <summary>
        ///// Game loop.
        ///// Drawing enemies as:
        /////     IMG tags, framerate is between 27 and 60.
        /////     DIV tags, framerate is roughly the same.
        /////     SVG tags, framerate is roughly the same.
        ///// Keep number of parameters passed to sub components to a minimum (they can be complex models).
        ///// 
        ///// Timer method 1.
        ///// </summary>
        ///// <returns></returns>
        //private async Task RunGame()
        //{
        //    while (true)
        //    {
        //        await Task.Delay((actualFrameRate < Constants.FrameRate) ? 1 : 1000 / Constants.FrameRate);

        //        GameLogic();
        //        StateHasChanged();

        //        FrameCounter();
        //    }
        //}

        /// <summary>
        /// Timer method 2.
        /// </summary>
        private void SetGameTimer()
        {
            if (Common.Global.CurrentGame != "invaders")
            {
                return;
            }

            timer = new Timer(1000 / Constants.FrameRate);
            timer.Elapsed += async (s, e) => await GameLoop();
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        private void StartGameTimer()
        {
            if (Common.Global.CurrentGame != "invaders")
            {
                return;
            }

            timer.Start();
        }

        private async Task GameLoop()
        {
            GameLogic();
            StateHasChanged();

            FrameCounter();
            StartGameTimer();
        }

        private void FrameCounter()
        {
            DateTimeOutput = DateTime.Now.ToString("HH:mm:ss ffff");
            CountOutput++;

            iterationCounter++;

            var currentDateTime = DateTime.Now;
            var totalSeconds = (int)(currentDateTime - lastDateTime).TotalSeconds;

            if (totalSeconds > 0 && totalSeconds < int.MaxValue)
            {
                actualFrameRate = iterationCounter / totalSeconds;
                iterationCounter = 0;
                lastDateTime = currentDateTime;
            }
        }

        protected override void OnInit()
        {
            if (Common.Global.CurrentGame != "invaders")
            {
                return;
            }

            base.OnInit();
            Initialise();
            // Timer method 1.
            //RunGame().ConfigureAwait(false);
            // Timer method 2.
            SetGameTimer();

            Console.WriteLine(nameof(OnInit));

            //CountOutput = DateTime.Now.ToLongTimeString();
        }

        private void GameLogic()
        {
            // Game loop.
            if (GameStatus == GameEnum.Pause)
            {
                if (LastKeyPress == ConsoleKey.P && CheckKeyDown(KeysEnum.KeyPause))
                {
                    LastKeyPress = default;
                    GameStatus = SavedGameStatus;
                }

                return;
            }

            if (GameStatus != GameEnum.Dead && GameStatus != GameEnum.Next)
            {
                if (LastKeyPress == ConsoleKey.P && CheckKeyDown(KeysEnum.KeyPause))
                {
                    LastKeyPress = default;
                    SavedGameStatus = GameStatus;
                    GameStatus = GameEnum.Pause;

                    return;
                }

                MovePlayer();
                MovePlayerBullet();
                PlayerFireBullet();

                //Enemies.ClearRender();

                var enemyAnimationTimer = MoveEnemies();
                MoveEnemyBullet();
                MoveMotherShip();
                Animate();
                EnemyFireBullet();
                BulletHitEnemies();
                BulletHitMother();
                BulletHitPlayer();
                BulletHitBunker();
                EnemiesHitBunkers();
                EnemiesHitPlayer();
                PlayerBackgroundSound(GameCounter, enemyAnimationTimer);

                GameCounter++;
                Var3 = enemyAnimationTimer.ToString();
            }

            GameWorkFlow();

            //Var1 = Enemies.bulletTimer;
            //Var2 = EnemyBullet.Visible ? 1 : 0;
            //Var1 = GameStatus.ToString();
            Var1 = "Mother.AnimationTimer: " + Mother.AnimationTimer.ToString();
            //Var2 = GameStatusTimer.ToString();
            Var2 = "Mother.MotherTimer: " + Mother.MotherTimer.ToString();
            //Var2="Bunker Xindex: "+
            //Var3 = "Bunker.Width: " + Bunker.Width.ToString();
            //Var4 = "Bunker.Xpos - ((Bunker.Width * Constants.BrickSize) / 2): " + (Bunker.Xpos - ((Bunker.Width * Constants.BrickSize) / 2)).ToString();
            Var4 = "Current game: " + Common.Global.CurrentGame;
        }

        private void PlayerBackgroundSound(int counter, int timer)
        {
            if (GameStatus == GameEnum.Dead || GameStatus == GameEnum.Next || GameStatus == GameEnum.Pause)
            {
                return;
            }

            var bgSoundSpeed = (timer + 1) * 4;
            if (counter % bgSoundSpeed != 0)
            {
                return;
            }

            var soundCount = (counter / bgSoundSpeed) % 4;

            switch (soundCount)
            {
                case 0:
                    SoundHelper.PlaySound(SoundsEnum.Inv1);
                    break;
                case 1:
                    SoundHelper.PlaySound(SoundsEnum.Inv2);
                    break;
                case 2:
                    SoundHelper.PlaySound(SoundsEnum.Inv3);
                    break;
                case 3:
                    SoundHelper.PlaySound(SoundsEnum.Inv4);
                    break;
                default:
                    break;
            }
        }

        private void EnemiesHitPlayer()
        {
            if (Player.Status != StatusEnum.Alive)
            {
                return;
            }

            foreach (var row in Enemies.Matrix)
            {
                foreach (var enemy in row.Where(e => e.Status == StatusEnum.Alive))
                {
                    if ((Player.Xpos - (Player.Width / 2) < enemy.Xpos + (enemy.Width / 2))
                    && (Player.Xpos + (Player.Width / 2) > enemy.Xpos - (enemy.Width / 2))
                        && (Player.Ypos < enemy.Ypos + enemy.Height)
                        && (Player.Ypos + Player.Height > enemy.Ypos))
                    {
                        Player.Hit();
                        Lives -= 1;

                        return;
                    }
                }
            }
        }

        private void EnemiesHitBunkers()
        {
            foreach (var bunker in Bunkers.BunkerList.Where(b => b.Visible))
            {
                //if ((bullet.Xpos - (bullet.Width / 2) < bunker.Xpos + (bunker.Width / 2))
                //&& (bullet.Xpos + (bullet.Width / 2) > bunker.Xpos - (bunker.Width / 2))
                //    && (bullet.Ypos < bunker.Ypos + bunker.Height)
                //    && (bullet.Ypos + bullet.Height > bunker.Ypos))

                foreach (var row in Enemies.Matrix)
                {
                    foreach (var enemy in row.Where(e => e.Status == StatusEnum.Alive))
                    {
                        // Overall bunker collision first.
                        if ((bunker.Xpos - (bunker.Width / 2) < enemy.Xpos + (enemy.Width / 2))
                        && (bunker.Xpos + (bunker.Width / 2) > enemy.Xpos - (enemy.Width / 2))
                            && (bunker.Ypos < enemy.Ypos + enemy.Height)
                            && (bunker.Ypos + bunker.Height > enemy.Ypos))
                        {
                            // Collision with Specific column, checking rows of bricks.
                            var brickXindex = Math.Min(bunker.BricksWide - 1, Math.Max(0, (enemy.Xpos - (bunker.Xpos - (bunker.Width / 2))) / Constants.BrickSize));
                            var brickYindex = bunker.BricksHigh - 1 - Math.Min(bunker.BricksHigh - 1, Math.Max(0, (enemy.Ypos - (bunker.Ypos - (bunker.Height / 2))) / Constants.BrickSize));

                            bunker.Bricks[brickYindex][brickXindex].Visible = false;
                        }
                    }
                }

            }
        }

        private void BulletHitBunker()
        {
            ActorBulletHitBunker(EnemyBullet);
            ActorBulletHitBunker(PlayerBullet);
        }

        private void ActorBulletHitBunker(Models.Bullet bullet)
        {
            if (!bullet.Visible)
            {
                return;
            }

            foreach (var bunker in Bunkers.BunkerList.Where(b => b.Visible))
            {
                if (!bullet.Visible)
                {
                    return;
                }

                // Overall bunker collision first.
                if ((bullet.Xpos - (bullet.Width / 2) < bunker.Xpos + (bunker.Width / 2))
                && (bullet.Xpos + (bullet.Width / 2) > bunker.Xpos - (bunker.Width / 2))
                    && (bullet.Ypos < bunker.Ypos + bunker.Height)
                    && (bullet.Ypos + bullet.Height > bunker.Ypos))
                {
                    // Collision with Specific column, checking rows of bricks.
                    var brickXindex = Math.Min(bunker.BricksWide - 1, Math.Max(0, (bullet.Xpos - (bunker.Xpos - (bunker.Width / 2))) / Constants.BrickSize));

                    // Determine brick row order based on bullet direction.
                    var rowStep = bullet.Dy >= 0 ? -1 : 1;
                    var rowStartIndex = bullet.Dy >= 0 ? bunker.Bricks.Count - 1 : 0;
                    var rowEndIndex = bunker.Bricks.Count - 1 - rowStartIndex + rowStep;

                    while (rowStartIndex != rowEndIndex)
                    {
                        if (bunker.Bricks[rowStartIndex][brickXindex].Visible)
                        {
                            bullet.Visible = false;
                            bunker.Bricks[rowStartIndex][brickXindex].Visible = false;
                            break;
                        }

                        rowStartIndex += rowStep;
                    }
                }
            }
        }

        private void BulletHitMother()
        {
            if (!PlayerBullet.Visible || Mother.Status != StatusEnum.Alive)
            {
                return;
            }

            if ((PlayerBullet.Xpos - (PlayerBullet.Width / 2) < Mother.Xpos + (Constants.MotherWidth / 2))
            && (PlayerBullet.Xpos + (PlayerBullet.Width / 2) > Mother.Xpos - (Constants.MotherWidth / 2))
                && (PlayerBullet.Ypos < Mother.Ypos + Constants.MotherHeight)
                && (PlayerBullet.Ypos + PlayerBullet.Height > Mother.Ypos))
            {
                Mother.Hit();
                PlayerBullet.Visible = false;
                Score += Mother.Score;
                SoundHelper.PlaySound(SoundsEnum.Explosion);

                return;
            }
        }

        private void GameWorkFlow()
        {
            GameStatusTimer--;

            if (GameStatusTimer > 0)
            {
                return;
            }

            GameStatusTimer = 0;

            switch (GameStatus)
            {
                case GameEnum.Stop:
                    break;
                case GameEnum.Prepare:
                    Player.Visible = true;
                    GameStatus = GameEnum.Play;
                    break;
                case GameEnum.Play:
                    if (Enemies.EnemiesAlive() == 0)
                    {
                        GameStatus = GameEnum.Next;
                        GameStatusTimer = Constants.PlayerNextTime;
                    }
                    break;
                case GameEnum.Next:
                    var initScore = Score + (Lives * Constants.RemainingLifeBonus);

                    Initialise(initScore, Lives);
                    break;
                case GameEnum.Dying:
                    if (Lives > 0)
                    {
                        Player = new Models.Player(Constants.GameAreaWidth / 2, Constants.InitialPlayerYpos);
                        GameStatus = GameEnum.Prepare;
                        GameStatusTimer = Constants.PlayerPrepareTime;
                    }
                    else
                    {
                        GameStatus = GameEnum.Dead;
                        LastKeyPress = default;
                    }
                    break;
                case GameEnum.Dead:
                    if (LastKeyPress != default)
                    {
                        Initialise();
                    }
                    break;
                default:
                    break;
            }
        }

        private void BulletHitPlayer()
        {
            if (!EnemyBullet.Visible || GameStatus == GameEnum.Prepare || Player.Status != StatusEnum.Alive)
            {
                return;
            }

            if ((EnemyBullet.Xpos - (EnemyBullet.Width / 2) < Player.Xpos + (Player.Width / 2))
            && (EnemyBullet.Xpos + (EnemyBullet.Width / 2) > Player.Xpos - (Player.Width / 2))
                && (EnemyBullet.Ypos < Player.Ypos + Player.Height)
                && (EnemyBullet.Ypos + EnemyBullet.Height > Player.Ypos))
            {
                Player.Hit();
                EnemyBullet.Visible = false;
                Lives -= 1;
                //GameStatus = GameEnum.Dying;

                //if (Lives <= 0)
                //{
                //    GameStatus = GameEnum.Stop;
                //}

                SoundHelper.PlaySound(SoundsEnum.Explosion);

                return;
            }
        }

        private void EnemyFireBullet()
        {
            if (!EnemyBullet.Visible)
            {
                var enemyBullet = Enemies.FireBullet();

                if (enemyBullet != null)
                {
                    EnemyBullet = enemyBullet;
                }
            }
        }

        private void MoveEnemyBullet()
        {
            if (EnemyBullet.Visible)
            {
                EnemyBullet.Ypos += EnemyBullet.Dy;

                if (EnemyBullet.Ypos < -EnemyBullet.Height)
                {
                    EnemyBullet.Visible = false;
                }
            }
        }

        private void Animate()
        {
            Enemies.AnimateDying();
            Mother.AnimateDying();
            //Console.WriteLine("Mother.AnimationTimer: " + Mother.AnimationTimer);
            Player.Animate(GameStatus);

            if (Player.AnimateDying())
            {
                GameStatus = GameEnum.Dying;
                //GameStatusTimer = 30;
            }
        }

        private void BulletHitEnemies()
        {
            if (!PlayerBullet.Visible)
            {
                return;
            }

            foreach (var row in Enemies.Matrix)
            {
                foreach (var enemy in row.Where(e => e.Status == StatusEnum.Alive))
                {
                    if ((PlayerBullet.Xpos - (PlayerBullet.Width / 2) < enemy.Xpos + (enemy.Width / 2))
                    && (PlayerBullet.Xpos + (PlayerBullet.Width / 2) > enemy.Xpos - (enemy.Width / 2))
                        && (PlayerBullet.Ypos < enemy.Ypos + enemy.Height)
                        && (PlayerBullet.Ypos + PlayerBullet.Height > enemy.Ypos))
                    {
                        enemy.Hit();
                        PlayerBullet.Visible = false;
                        Score += enemy.Score;
                        SoundHelper.PlaySound(SoundsEnum.InvKilled);

                        return;
                    }
                }
            }
        }

        private int MoveEnemies()
        {
            return Enemies.MoveEnemies(Player.Ypos);
        }

        private void MovePlayer()
        {
            if (Player.Status != StatusEnum.Alive)
            {
                return;
            }

            const int dxv = 4;
            var dx = 0;

            if (CheckKeyDown(KeysEnum.KeyLeft))
                dx = -dxv;
            if (CheckKeyDown(KeysEnum.KeyRight))
                dx = dxv;

            Player.Xpos += dx;

            if (Player.Xpos < Constants.GameLeftRightMargin)
                Player.Xpos = Constants.GameLeftRightMargin;
            if (Player.Xpos > Constants.GameAreaWidth - Constants.GameLeftRightMargin)
                Player.Xpos = Constants.GameAreaWidth - Constants.GameLeftRightMargin;
        }

        private void PlayerFireBullet()
        {
            if (Player.Status != StatusEnum.Alive)
            {
                return;
            }

            if (!PlayerBullet.Visible && CheckKeyDown(KeysEnum.KeyFire))
            {
                PlayerBullet.Xpos = Player.Xpos;
                PlayerBullet.Ypos = Player.Ypos + Player.Height;
                PlayerBullet.Width = Constants.BulletWidth;
                PlayerBullet.Height = Constants.BulletHeight;
                PlayerBullet.Dy = Constants.PlayerBulletSpeed;
                PlayerBullet.Visible = true;

                SoundHelper.PlaySound(SoundsEnum.Shoot);
            }
        }

        private void MovePlayerBullet()
        {
            if (PlayerBullet.Visible)
            {
                PlayerBullet.Ypos += PlayerBullet.Dy;

                if (PlayerBullet.Ypos >= Constants.GameAreaHeight)
                {
                    PlayerBullet.Visible = false;
                }
            }
        }

        private void MoveMotherShip()
        {
            if (Mother.Visible && Mother.Status == StatusEnum.Alive)
            {
                Mother.Xpos -= Constants.MotherSpeed;

                if (Mother.Xpos < -Mother.Width / 2)
                {
                    Mother.Visible = false;
                }
            }
            else if (Mother.Status != StatusEnum.Dying)
            {
                Mother.TimerTick(Enemies.EnemiesAlive());
            }
        }

        private void Initialise(int score = 0, int lives = Constants.InitialPlayerLives)
        {
            InteropKeyPress.RemoveAllEvents();
            InteropKeyPress.KeyDown += OnKeyDown;
            InteropKeyPress.KeyUp += OnKeyUp;
            //InteropKeyPress.KeyPress += OnKeyPress;
            SoundHelper.InitialiseSound();

            InitialiseGame(score, lives);
            StateHasChanged();
        }

        private void InitialiseGame(int score = 0, int lives = Constants.InitialPlayerLives)
        {
            var random = new Random();

            Player = new Models.Player(Constants.GameAreaWidth / 2, Constants.InitialPlayerYpos);
            Enemies = new Models.Enemies(Constants.EnemiesWide, Constants.EnemiesHigh, Constants.EnemiesXgap, Constants.EnemiesYgap,
                Constants.GameAreaWidth / 2, Constants.EnemiesBottomYpos, random.Next(50, 100));
            Mother = new Models.Mother(Constants.InitialMotherYpos, Lives);
            //Bunker = new Models.Bunker(Constants.BunkerWidth, Constants.BunkerHeight, Constants.GameAreaWidth/2, 70);
            Bunkers = new Models.Bunkers(Constants.BunkerCount);

            Score = score;
            Lives = lives;
            GameStatus = GameEnum.Prepare;
            GameStatusTimer = Constants.PlayerPrepareTime;
            GameCounter = 0;
        }

        private void OnKeyDown(object sender, ConsoleKey e)
        {
            //Console.WriteLine($"{nameof(OnKeyDown)}: {e}");
            LastKeyPress = e;

            if (!KeysDown.Contains(e))
            {
                KeysDown.Add(e);
            }
        }

        private void OnKeyUp(object sender, ConsoleKey e)
        {
            //Console.WriteLine($"{nameof(OnKeyUp)}: {e}");

            if (KeysDown.Contains(e))
            {
                KeysDown.Remove(e);
            }
        }

        private bool CheckKeyDown(KeysEnum keysEnum) 
        {
            switch (keysEnum)
            {
                case KeysEnum.KeyDown:
                    return this.KeysDown.Any(k => k == ConsoleKey.DownArrow || k == ConsoleKey.S);
                case KeysEnum.KeyUp:
                    return this.KeysDown.Any(k => k == ConsoleKey.UpArrow || k == ConsoleKey.W);
                case KeysEnum.KeyLeft:
                    return this.KeysDown.Any(k => k == ConsoleKey.LeftArrow || k == ConsoleKey.A);
                case KeysEnum.KeyRight:
                    return this.KeysDown.Any(k => k == ConsoleKey.RightArrow || k == ConsoleKey.D);
                case KeysEnum.KeyFire:
                    return this.KeysDown.Any(k => k == ConsoleKey.Spacebar);
                case KeysEnum.KeyPause:
                    return this.KeysDown.Any(k => k == ConsoleKey.P);
                case KeysEnum.KeyDebug1:
                    return this.KeysDown.Any(k => k == ConsoleKey.D1);
            }

            return false;
        }

        //private void OnKeyPress(object sender, ConsoleKey e)
        //{
        //    Console.WriteLine($"{nameof(OnKeyPress)}: {e}");
        //    LastKeyPressed = e;
        //    LastKeyPress = LastKeyPressed;
        //}

    }
}
