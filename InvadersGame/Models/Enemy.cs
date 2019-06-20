using InvadersGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvadersGame.Models
{
    public class Enemy : BaseActor
    {
        public EnemyTypesEnum EnemyType { get; set; }
        public bool Animation { get; set; }
        public bool Render { get; set; }
        public int Score { get; set; }

        public void Hit()
        {
            Hit(5);
        }
    }
}
