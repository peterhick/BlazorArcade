using BreakoutGame.Enums;
using BreakoutGame.Helpers;
using BreakoutGame.Models;
using Common.JsInterop;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BreakoutGame
{
    public class BreakoutGameModel : BlazorComponent
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
        internal Models.Ball Ball { get; set; } = new Models.Ball();

        [Parameter]
        internal Models.Bricks Bricks { get; set; }

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
        public static bool RunGame;

        public string StopGame()
        {
            SoundHelper.PlaySound(SoundsEnum.Debug1);
            timer.AutoReset = false;
            timer.Enabled = false;
            timer.Stop();

            return string.Empty;
        }

        ///// <summary>
        ///// Game loop.
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
            if (Common.Global.CurrentGame != "breakout")
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
            if (Common.Global.CurrentGame != "breakout")
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
            if (Common.Global.CurrentGame != "breakout")
            {
                return;
            }

            base.OnInit();
            Initialise();
            //// Timer method 1.
            //RunGame().ConfigureAwait(false);
            // Timer method 2.
            SetGameTimer();

            //BlazorMain.Program.
            Console.WriteLine(nameof(OnInit));

            //CountOutput = DateTime.Now.ToLongTimeString();
        }

        //public void Dispose()
        //{
        //    StopGame();
        //}

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
                MoveBall();
                DebugActions();

                Animate();
                BallHitBricks();
                BallHitPlayer();

                GameCounter++;
            }

            GameWorkFlow();

            Var1 = "Ball Dx: " + Ball.Dx;
            Var2 = "Ball Dy: " + Ball.Dy;
            Var3 = "Player ball hits: " + Player.HitCounter;
            Var4 = "Current game: " + Common.Global.CurrentGame;
        }

        private void DebugActions()
        {
            // Slow ball down.
            if (CheckKeyDown(KeysEnum.KeyDebug1))
            {
                if (Ball.Dx > 1)
                {
                    Ball.Dx--;
                }
                else if (Ball.Dx < -1)
                {
                    Ball.Dx++;
                }

                if (Ball.Dy > 1)
                {
                    Ball.Dy--;
                }
                else if (Ball.Dy < -1)
                {
                    Ball.Dy++;
                }
            }

            // Speed ball up.
            if (CheckKeyDown(KeysEnum.KeyDebug2))
            {
                if (Ball.Dx > 0)
                {
                    Ball.Dx++;
                }
                else if (Ball.Dx < 0)
                {
                    Ball.Dx--;
                }

                if (Ball.Dy > 0)
                {
                    Ball.Dy++;
                }
                else if (Ball.Dy < 0)
                {
                    Ball.Dy--;
                }
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
                    if (Bricks.BricksPresent() == 0)
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
                        InitialiseBall();
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

        private void BallHitPlayer()
        {
            if (!Ball.Visible || GameStatus == GameEnum.Prepare || Player.Status != StatusEnum.Alive)
            {
                return;
            }

            if ((Ball.Xpos - (Ball.Width / 2) <= Player.Xpos + (Player.Width / 2))
            && (Ball.Xpos + (Ball.Width / 2) >= Player.Xpos - (Player.Width / 2))
                && (Ball.Dy < 0)
                && (Ball.Ypos - (Ball.Height / 2) <= Player.Ypos + Player.Height)
                && (Ball.Ypos + (Ball.Height / 2) >= Player.Ypos))
            {
                //Player.Hit();
                //Ball.Visible = false;
                //Lives -= 1;
                Ball.Dy = Math.Abs(Ball.Dy);

                // Speed up ball at specific hit counts.
                Player.HitCounter++;
                if (Constants.SpeedHits.Contains(Player.HitCounter))
                {
                    Ball.Dx += Ball.Dx > 0 ? Constants.BallSpeedUp : -Constants.BallSpeedUp;
                    Ball.Dy += Ball.Dy > 0 ? Constants.BallSpeedUp : -Constants.BallSpeedUp;
                }

                //GameStatus = GameEnum.Dying;

                //if (Lives <= 0)
                //{
                //    GameStatus = GameEnum.Stop;
                //}

                SoundHelper.PlaySound(SoundsEnum.HitPlayer);

                return;
            }
        }

        private void Animate()
        {
            Player.Animate(GameStatus);

            if (Player.AnimateDying())
            {
                GameStatus = GameEnum.Dying;
            }
        }

        private void BallHitBricks()
        {
            if (!Ball.Visible)
            {
                return;
            }

            // Check ball hit any bricks.

            foreach (var row in Bricks.Matrix)
            {
                foreach (var brick in row.Where(e => e.Status == StatusEnum.Alive && e.Visible))
                {
                    // Ball overlap brick.
                    var xOverlap = (Ball.Xpos - (Ball.Width / 2) < brick.Xpos + (brick.Width / 2))
                        && (Ball.Xpos + (Ball.Width / 2) > brick.Xpos - (brick.Width / 2));
                    var yOverlap = (Ball.Ypos - (Ball.Height / 2) < brick.Ypos + (brick.Height / 2))
                        && (Ball.Ypos + (Ball.Height / 2) > brick.Ypos - (brick.Height / 2));

                    if ((Ball.Xpos - (Ball.Width / 2) < brick.Xpos + (brick.Width / 2))
                    && (Ball.Xpos + (Ball.Width / 2) > brick.Xpos - (brick.Width / 2))
                        && (Ball.Ypos - (Ball.Height / 2) < brick.Ypos + (brick.Height / 2))
                        && (Ball.Ypos + (Ball.Height / 2) > brick.Ypos - (brick.Height / 2)))
                    {
                        // Brick disappears, add score.
                        brick.Hit();
                        //Ball.Visible = false;
                        Score += brick.Score;
                        SoundHelper.PlaySound(SoundsEnum.HitBrick);

                        //// Set ball to edge of brick.
                        //if (Ball.Dy > 0)
                        //{
                        //    Ball.Ypos = brick.Ypos - (brick.Height / 2) - (Ball.Height / 2);
                        //}
                        //else
                        //{
                        //    Ball.Ypos = brick.Ypos + (brick.Height / 2) + (Ball.Height / 2);
                        //}

                        //if (Ball.Dx > 0)
                        //{
                        //    Ball.
                        //}

                        // Check whether ball has hit in Y or X direction.
                        var lastXOverlap = (Ball.LastXpos - (Ball.Width / 2) < brick.Xpos + (brick.Width / 2))
                            && (Ball.LastXpos + (Ball.Width / 2) > brick.Xpos - (brick.Width / 2));
                        var lastYOverlap = (Ball.LastYpos - (Ball.Height / 2) < brick.Ypos + (brick.Height / 2))
                            && (Ball.LastYpos + (Ball.Height / 2) > brick.Ypos - (brick.Height / 2));

                        if (!lastYOverlap && yOverlap)
                        {
                            //// Set ball to edge of brick.
                            //if (Ball.Dy > 0)
                            //{
                            //    Ball.Ypos = brick.Ypos - (brick.Height / 2) - (Ball.Height / 2);
                            //}
                            //else
                            //{
                            //    Ball.Ypos = brick.Ypos + (brick.Height / 2) + (Ball.Height / 2);
                            //}

                            Ball.Dy = -Ball.Dy;
                            Ball.Ypos += Ball.Dy;
                            return;
                        }

                        //if (!lastXOverlap && xOverlap)
                        //{
                        //    Ball.Dx = -Ball.Dx;
                        //}

                        return;
                    }
                }
            }
        }

        private void MovePlayer()
        {
            if (Player.Status != StatusEnum.Alive)
            {
                return;
            }

            const int dxv = Constants.PlayerSpeed;
            var dx = 0;

            if (CheckKeyDown(KeysEnum.KeyLeft))
                dx = -dxv;
            if (CheckKeyDown(KeysEnum.KeyRight))
                dx = dxv;

            Player.Xpos += dx;

            if (Player.Xpos < Constants.ArenaLeftWidth + (Player.Width / 2))
                Player.Xpos = Constants.ArenaLeftWidth + (Player.Width / 2);
            if (Player.Xpos > Constants.GameAreaWidth - Constants.ArenaRightWidth - (Player.Width / 2))
                Player.Xpos = Constants.GameAreaWidth - Constants.ArenaRightWidth - (Player.Width / 2);
        }

        private void MoveBall()
        {
            if (Ball.Visible)
            {
                Ball.LastXpos = Ball.Xpos;
                Ball.LastYpos = Ball.Ypos;

                Ball.Ypos += Ball.Dy;
                Ball.Xpos += Ball.Dx;

                // Temporarily bounce ball off bottom.
                if (Ball.Ypos < 0)
                {
                    Ball.Visible = false;
                    //Ball.Dy = Math.Abs(Ball.Dy);

                    Player.Hit();
                    Lives -= 1;
                    SoundHelper.PlaySound(SoundsEnum.LostBall);

                    //GameStatus = GameEnum.Dying;

                    //if (Lives <= 0)
                    //{
                    //    GameStatus = GameEnum.Stop;
                    //}
                }

                // Collision with edges.
                var topLimit = Constants.GameAreaHeight - Constants.ArenaTopWidth - (Ball.Height / 2);
                if (Ball.Ypos >= topLimit)
                {
                    Ball.Ypos = topLimit;
                    Ball.Dy = -Math.Abs(Ball.Dy);
                    SoundHelper.PlaySound(SoundsEnum.HitEdge);
                }

                var leftLimit = Constants.ArenaLeftWidth + (Ball.Width / 2);
                if (Ball.Xpos <= leftLimit)
                {
                    Ball.Xpos = leftLimit;
                    Ball.Dx = Math.Abs(Ball.Dx);
                    SoundHelper.PlaySound(SoundsEnum.HitEdge);
                }

                var rightLimit = Constants.GameAreaWidth - Constants.ArenaRightWidth - (Ball.Width / 2);
                if (Ball.Xpos >= rightLimit)
                {
                    Ball.Xpos = rightLimit;
                    Ball.Dx = -Math.Abs(Ball.Dx);
                    SoundHelper.PlaySound(SoundsEnum.HitEdge);
                }
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
            Player = new Models.Player(Constants.GameAreaWidth / 2, Constants.InitialPlayerYpos);
            Bricks = new Models.Bricks(Constants.BricksWide, Constants.BricksHigh, Constants.BricksXgap, Constants.BricksYgap,
                Constants.GameAreaWidth / 2, Constants.BricksBottomYpos, 1);

            InitialiseBall();

            Score = score;
            Lives = lives;
            GameStatus = GameEnum.Prepare;
            GameStatusTimer = Constants.PlayerPrepareTime;
            GameCounter = 0;
        }

        private void InitialiseBall()
        {
            var random = new Random();

            Ball = new Ball
            {
                Width = Constants.BallSize,
                Height = Constants.BallSize,
                Xpos = random.Next(2) == 0
                    ? Constants.ArenaLeftWidth + (Ball.Width / 2)
                    : Constants.GameAreaWidth - Constants.ArenaRightWidth - (Ball.Width / 2),
                Ypos = Constants.BallStartYpos,
                Dx = Constants.BallSpeed,
                Dy = -Constants.BallSpeed,
                Visible = true
            };
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
                    return this.KeysDown.Any(k => k == ConsoleKey.NumPad1);
                case KeysEnum.KeyDebug2:
                    return this.KeysDown.Any(k => k == ConsoleKey.NumPad2);
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
