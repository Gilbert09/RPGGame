using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework;

namespace RPGGame.Sprites.Enemies
{
    class GoblinSword : Enemy, IHurtable
    {
        public GoblinSword(SpriteFrameHolder sprite, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(sprite, startingDirection, state, incStill, camera)
        {
            Health = 110;
            ExperiancePoints = 15;
            EnemyStance = EnemyStance.Guarding;
            EnemyState = EnemyState.Hostile;
            TileViewRange = 3;
            TileReturnRange = 5;
            speed = 0.35f;
            Damage = 35;
            ShootRate = ShootCoolDownRate = 17;
        }
    }
}
