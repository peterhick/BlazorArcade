namespace InvadersGame.Helpers
{
    public class Constants
    {
        // Player.
        public const int PlayerWidth = 22;
        public const int PlayerHeight = 15;
        public const int InitialPlayerYpos = 20;
        public const int InitialPlayerLives = 3;
        public const int PlayerPrepareTime = 15;
        public const int PlayerNextTime = 30;
        public const int RemainingLifeBonus = 1000;

        // Bunkers.
        public const int BunkerCount = 4;
        public const int BunkerWidth = 5;
        public const int BunkerHeight = 4;
        public const int BrickSize = 12;
        public const int BunkerBottomYpos = 70;

        // Enemy.
        public const int EnemiesBottomYpos = 200;
        public const int EnemiesWide = 10;
        public const int EnemiesHigh = 6;
        public const int EnemiesXgap = 30;
        public const int EnemiesYgap = 25;
        public const int Enemy1Width = 19;
        public const int Enemy1Height = 14;
        public const int Enemy2Width = 18;
        public const int Enemy2Height = 13;
        public const int Enemy3Width = 14;
        public const int Enemy3Height = 15;
        public const int EnemyExplosionWidth = 21;
        public const int EnemyExplosionHeight = 15;
        public const int InitialEnemyDx = -2;
        public const int InitialEnemyDy = 20;
        public const int InitialEnemyMoveTimer = 10;
        public const int InitialEnemyAnimationTimer = 0;
        public const int Enemy1Score = 10;
        public const int Enemy2Score = 20;
        public const int Enemy3Score = 30;

        // Mother.
        public const int MotherWidth = 40;
        public const int MotherHeight = 18;
        public const int InitialMotherYpos = 380;
        public const int MotherSpeed = 4;
        public const int MotherScoreMin = 50;
        public const int MotherScoreMax = 300;
        public const int MotherScoreStep = 50;

        // Bullet.
        public const int BulletWidth = 2;
        public const int BulletHeight = 6;
        public const int EnemyBulletSpeed = 10;
        public const int PlayerBulletSpeed = 15;

        // Game.
        public const int FrameRate = 60;
        public const int GameAreaWidth = 600;
        public const int GameAreaHeight = 450;
        public const int GameLeftRightMargin = 30;
    }
}
