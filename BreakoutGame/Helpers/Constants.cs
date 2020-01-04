using System.Collections.Generic;

namespace BreakoutGame.Helpers
{
    public class Constants
    {
        // Player.
        public const int PlayerWidth = 80;
        public const int PlayerHeight = 10;
        public const int PlayerSpeed = 14;
        public const int InitialPlayerYpos = 20;
        public const int InitialPlayerLives = 3;
        public const int PlayerPrepareTime = 15;
        public const int PlayerNextTime = 30;
        public const int RemainingLifeBonus = 1000;

        // Arena.
        public const int ArenaLeftWidth = 5;
        public const int ArenaRightWidth = 5;
        public const int ArenaTopWidth = 5;

        // Bricks.
        public const int BricksBottomYpos = 375;
        public const int BricksWide = 24;
        public const int BricksHigh = 8;
        public const int BricksXgap = 40;
        public const int BricksYgap = 16;
        public const int Brick1Score = 1;
        public const int Brick2Score = 3;
        public const int Brick3Score = 5;
        public const int Brick4Score = 7;
        public const int BrickWidth = 35;
        public const int BrickHeight = 12;

        // Ball.
        public const int BallStartYpos = 320;
        public const int BallSize = 8;
        public const int BallSpeed = 6;
        public static readonly List<int> SpeedHits = new List<int> { 4, 12 };
        public const int BallSpeedUp = 2;

        // Game.
        public const int FrameRate = 60;
        public const int GameAreaWidth = 1000;
        public const int GameAreaHeight = 600;
    }
}
