using InvadersGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvadersGame.Models
{
    public class BaseActor : Base
    {
        public StatusEnum Status { get; set; } = StatusEnum.Alive;
        public int AnimationTimer { get; set; } = 0;

        public void Hit(int animationTimer)
        {
            Status = StatusEnum.Dying;
            AnimationTimer = animationTimer;
        }
    }
}
