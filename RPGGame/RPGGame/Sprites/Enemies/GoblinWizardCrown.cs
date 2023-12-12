using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework;
using RPGGame.Entities;

namespace RPGGame.Sprites.Enemies
{
    class GoblinWizardCrown : Enemy, IHurtable
    {
        public GoblinWizardCrown(SpriteFrameHolder sprite, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(sprite, startingDirection, state, incStill, camera)
        {
            Health = 70;
            ExperiancePoints = 20;
            EnemyStance = EnemyStance.Guarding;
            EnemyState = EnemyState.Hostile;
            TileViewRange = 6;
            TileReturnRange = 7;
            speed = 0.35f;
            Damage = 45;
            ShootRate = ShootCoolDownRate = 50;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (!shootCoolDown && EnemyStance == EnemyStance.Aggressive)
            {
                PlayableCharacter current = SpriteManager.CurrentCharacter;
                float angle = (float)Math.Atan2(current.Y - Y, current.X - X);
                Vector2 angleDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                angleDirection.Normalize();

                EnemyProjectile projectile = new EnemyProjectile(ResourceHolder.RedBolt, new Vector2(X + (Width / 2), Y + (Height / 2)), angleDirection * 1.3f, 45, true, this, Damage);
                Children.Add(projectile);
                SpriteManager.AddSprite(projectile);

                shootCoolDown = true;
            }
        }
    }
}
