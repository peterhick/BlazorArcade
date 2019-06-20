using InvadersGame.Enums;
using InvadersGame.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvadersGame.Models
{
    public class Enemies
    {
        public List<List<Enemy>> Matrix { get; set; }

        private int enemyDx = -2;
        private int enemyDy = 20;
        private int enemyMoveTimerStart;
        private int enemyMoveTimer;
        private int enemyAnimationTimerStart;
        private int enemyAnimationTimer;
        private int renderIndex = 0;

        public int bulletTimer = 0;

        public int Left { get; set; }

        public Enemies(int Xcount, int Ycount, int Xgap, int Ygap, int CentreXpos, int BottomYpos, int BulletTimer)
        {
            Matrix = new List<List<Enemy>>();
            //var alive = EnemyStatusEnum.Alive;

            enemyDx = Constants.InitialEnemyDx;
            enemyDy = Constants.InitialEnemyDy;
            enemyMoveTimerStart = Constants.InitialEnemyMoveTimer;
            enemyMoveTimer = 0;
            enemyAnimationTimerStart = Constants.InitialEnemyAnimationTimer;
            enemyAnimationTimer = 0;

            var enemyType = EnemyTypesEnum.Type1;
            var typeCount = 0;
            var yPos = BottomYpos + ((Ycount - 1) * Ygap);

            for (int yCount = 0; yCount < Ycount; yCount++)
            {
                var enemiesRow = new List<Enemy>();
                var xPos = CentreXpos - ((Xcount - 1) * Xgap / 2);
                var width = 0;
                var height = 0;
                var score = 0;

                switch (typeCount)
                {
                    case 0:
                    case 1:
                        enemyType = EnemyTypesEnum.Type3;
                        width = Constants.Enemy3Width;
                        height = Constants.Enemy3Height;
                        score = Constants.Enemy3Score;
                        break;
                    case 2:
                    case 3:
                        enemyType = EnemyTypesEnum.Type2;
                        width = Constants.Enemy2Width;
                        height = Constants.Enemy2Height;
                        score = Constants.Enemy2Score;
                        break;
                    default:
                        enemyType = EnemyTypesEnum.Type1;
                        width = Constants.Enemy1Width;
                        height = Constants.Enemy1Height;
                        score = Constants.Enemy1Score;
                        break;
                }

                for (int xCount = 0; xCount < Xcount; xCount++)
                {
                    enemiesRow.Add(new Enemy
                    {
                        Status = StatusEnum.Alive,
                        //Status = alive,
                        EnemyType = enemyType,
                        Xpos = xPos,
                        Ypos = yPos,
                        Width = width,
                        Height = height,
                        Score = score
                    });
                    //alive = EnemyStatusEnum.Dead;

                    xPos += Xgap;
                }

                yPos -= Ygap;
                typeCount++;
                Matrix.Add(enemiesRow);
            }

            bulletTimer = BulletTimer;
        }

        //public int LeftEdge()
        //{
        //    var leftEdge = int.MaxValue;

        //    foreach (var row in Matrix)
        //    {
        //        var minXpos = row.Min(r => r.Xpos);

        //        if (leftEdge > minXpos)
        //            leftEdge = minXpos;
        //    }

        //    return leftEdge;
        //}

        //public int RightEdge()
        //{
        //    var rightEdge = int.MinValue;

        //    foreach (var row in Matrix)
        //    {
        //        var maxXpos = row.Max(r => r.Xpos);

        //        if (rightEdge < maxXpos)
        //            rightEdge = maxXpos;
        //    }

        //    return rightEdge;
        //}

        public bool MoveLeft(int leftLimit, int dx)
        {
            var hitLimit = false;

            foreach (var row in Matrix)
            {
                for (int enemyIndex = 0; enemyIndex < row.Count; enemyIndex++)
                {
                    var enemy = row[enemyIndex];

                    if (enemy.Status != StatusEnum.Alive)
                    {
                        continue;
                    }

                    if (enemy.Xpos + dx >= leftLimit)
                    {
                        enemy.Xpos += dx;
                    }
                    else
                    {
                        hitLimit = true;
                        break;
                    }

                    if (enemy.Xpos <= leftLimit)
                    {
                        hitLimit = true;
                    }
                }
            }

            return hitLimit;
        }

        public bool MoveRight(int rightLimit, int dx)
        {
            var hitLimit = false;

            foreach (var row in Matrix)
            {
                for (int enemyIndex = row.Count-1; enemyIndex >=0; enemyIndex--)
                {
                    var enemy = row[enemyIndex];

                    if (enemy.Status != StatusEnum.Alive)
                    {
                        continue;
                    }

                    if (enemy.Xpos + dx <= rightLimit)
                    {
                        enemy.Xpos += dx;
                    }
                    else
                    {
                        hitLimit = true;
                        break;
                    }

                    if (enemy.Xpos >= rightLimit)
                    {
                        hitLimit = true;
                    }
                }
            }

            return hitLimit;
        }

        public bool MoveDown(int bottomLimit, int dy)
        {
            var hitLimit = false;

            foreach (var row in Matrix)
            {
                for (int enemyIndex = 0; enemyIndex < row.Count; enemyIndex++)
                {
                    var enemy = row[enemyIndex];

                    if (enemy.Status != StatusEnum.Alive)
                    {
                        continue;
                    }

                    if (enemy.Ypos - dy >= bottomLimit)
                    {
                        enemy.Ypos -= dy;
                    }
                    else
                    {
                        hitLimit = true;
                        break;
                    }

                    if (enemy.Ypos <= bottomLimit)
                    {
                        hitLimit = true;
                    }
                }
            }

            return hitLimit;
        }

        public int EnemiesAlive()
        {
            return Matrix.Sum(y => y.Count(x => x.Status == StatusEnum.Alive));
        }

        public void ClearRender()
        {
            foreach (var enemy in Matrix.SelectMany(r => r))
            {
                enemy.Render = false;
            }
        }

        public int SetRender()
        {
            var enemiesAlive = EnemiesAlive();

            if (enemiesAlive == 0)
            {
                return renderIndex;
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    enemiesAlive = EnemiesAlive();

            //    renderIndex = renderIndex % enemiesAlive;

            //    Matrix.SelectMany(r => r).Where(e => e.Status != EnemyStatusEnum.Dead).ToList()[renderIndex].Render = true;

            //    renderIndex++;
            //}

            for (int i = 0; i < 10; i++)
            {
                enemiesAlive = EnemiesAlive();

                renderIndex = renderIndex % enemiesAlive;

                Matrix.SelectMany(r => r).Where(e => e.Status != StatusEnum.Dead).ToList()[renderIndex].Render = true;

                renderIndex++;
            }

            return renderIndex;
            //foreach (var enemy in Matrix.SelectMany(r => r))
            //{
            //    enemy.Render = true;
            //}
        }

        public void Animate()
        {
            foreach (var enemy in Matrix.SelectMany(r => r).Where(e => e.Status == StatusEnum.Alive))
            {
                enemy.Animation = !enemy.Animation;
            }
        }

        public void AnimateDying()
        {
            foreach (var enemy in Matrix.SelectMany(r => r).Where(e => e.Status == StatusEnum.Dying))
            {
                enemy.AnimationTimer--;

                if (enemy.AnimationTimer <= 0)
                {
                    enemy.Status = StatusEnum.Dead;
                }
            }
        }

        public Bullet FireBullet()
        {
            var enemiesAlive = EnemiesAlive();

            if (enemiesAlive == 0)
            {
                return null;
            }

            bulletTimer -= 1;

            if (bulletTimer > 0)
            {
                return null;
            }

            var random = new Random();
            bulletTimer = random.Next(5 + (enemiesAlive / 3), 10 + (enemiesAlive / 2));
            var enemy = GetEnemy(random.Next(0, enemiesAlive));

            return new Bullet
            {
                Xpos = enemy.Xpos,
                Ypos = enemy.Ypos - Constants.BulletHeight,
                Width = Constants.BulletWidth,
                Height = Constants.BulletHeight,
                Dy = -Constants.EnemyBulletSpeed,
                Visible = true
            };
        }

        private Enemy GetEnemy(int index)
        {
            return Matrix.SelectMany(r => r).Where(e => e.Status == StatusEnum.Alive).ToList()[index];
        }

        public int MoveEnemies(int BottomLimit)
        {
            enemyMoveTimerStart = EnemiesAlive() / 5;

            if (enemyMoveTimer > 0)
            {
                enemyMoveTimer--;

                return enemyMoveTimerStart;
            }

            enemyMoveTimer = enemyMoveTimerStart;

            AnimateEnemies();

            if (enemyDx < 0)
            {
                if (MoveLeft(Constants.GameLeftRightMargin, enemyDx))
                {
                    enemyDx = -enemyDx;
                    MoveDown(BottomLimit, enemyDy);
                }
            }
            else
            {
                if (MoveRight(Constants.GameAreaWidth - Constants.GameLeftRightMargin, enemyDx))
                {
                    enemyDx = -enemyDx;
                    MoveDown(BottomLimit, enemyDy);
                }
            }

            return enemyMoveTimerStart;
        }

        private void AnimateEnemies()
        {
            if (enemyAnimationTimer > 0)
            {
                enemyAnimationTimer--;
                return;
            }

            enemyAnimationTimer = enemyAnimationTimerStart;
            Animate();
        }

    }
}
