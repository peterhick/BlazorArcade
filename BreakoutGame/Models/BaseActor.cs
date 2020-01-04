using BreakoutGame.Enums;

namespace BreakoutGame.Models
{
    public class BaseActor : Base
    {
        public StatusEnum Status { get; set; } = StatusEnum.Alive;
        public int AnimationTimer { get; set; } = 0;
        public int HitCounter { get; set; } = 0;

        public void Hit(int animationTimer)
        {
            Status = StatusEnum.Dying;
            AnimationTimer = animationTimer;
        }
    }
}
