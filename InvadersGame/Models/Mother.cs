using InvadersGame.Enums;
using InvadersGame.Helpers;
using System;

namespace InvadersGame.Models
{
    public class Mother : BaseActor
    {
        public int Score { get; set; }
        public int MotherTimer { get; set; }

        public Mother(int Ypos, int EnemiesAlive)
        {
            Visible = false;
            this.Ypos = Ypos;
            Width = Constants.MotherWidth;
            Height = Constants.MotherHeight;

            SetTimer(EnemiesAlive);
        }

        public void Hit()
        {
            Hit(40);
        }

        public void SetTimer(int EnemiesAlive)
        {
            var random = new Random();

            MotherTimer = random.Next(300 + (EnemiesAlive * 2), 500 + (EnemiesAlive * 4));
        }

        public void TimerTick(int EnemiesAlive)
        {
            MotherTimer--;

            if (MotherTimer <= 0)
            {
                Start(EnemiesAlive);
                SoundHelper.PlaySound(SoundsEnum.UfoLow);
            }
        }

        public void Start(int EnemiesAlive)
        {
            var random = new Random();

            Xpos = Constants.GameAreaWidth + (Width / 2);
            Visible = true;
            Status = StatusEnum.Alive;
            Score = Constants.MotherScoreStep
                * random.Next(Constants.MotherScoreMin / Constants.MotherScoreStep, 1 + (Constants.MotherScoreMax / Constants.MotherScoreStep));
            SetTimer(EnemiesAlive);
        }

        public void AnimateDying()
        {
            if (Status != StatusEnum.Dying)
            {
                return;
            }

            AnimationTimer--;

            if (AnimationTimer <= 0)
            {
                Status = StatusEnum.Dead;
            }
        }

    }
}
