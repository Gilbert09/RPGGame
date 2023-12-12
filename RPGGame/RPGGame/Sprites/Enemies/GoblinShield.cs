using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework;

namespace RPGGame.Sprites.Enemies
{
    class GoblinShield : Enemy, IHurtable
    {
        public GoblinShield(SpriteFrameHolder sprite, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(sprite, startingDirection, state, incStill, camera)
        {
            Health = 150;
            ExperiancePoints = 25;
            EnemyStance = EnemyStance.Guarding;
            EnemyState = EnemyState.Hostile;
            TileViewRange = 4;
            TileReturnRange = 6;
            speed = 0.3f;
            Damage = 25;
            ShootRate = ShootCoolDownRate = 20;
        }
    }
}
