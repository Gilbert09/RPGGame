using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework;

namespace RPGGame.Sprites.Enemies
{
    class GoblinClub : Enemy, IHurtable
    {
        public GoblinClub(SpriteFrameHolder sprite, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(sprite, startingDirection, state, incStill, camera)
        {
            Health = 80;
            ExperiancePoints = 10;
            EnemyStance = EnemyStance.Guarding;
            EnemyState = EnemyState.Hostile;
            TileViewRange = 6;
            TileReturnRange = 7;
            speed = 0.35f;
            Damage = 15;
            ShootRate = ShootCoolDownRate = 10;
        }
    }
}
