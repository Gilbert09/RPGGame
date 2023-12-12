using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using RPGGame.Core;
using Microsoft.Xna.Framework.Graphics;
using RPGGame.Sprites;

namespace RPGGame.Entities
{
    class EnemyProjectile : Projectile
    {
        public EnemyProjectile(Texture2D texture, Vector2 position, Vector2 velocity, float life, bool rotate, Sprite parent, float damage)
            : base(texture, position, velocity, life, rotate, parent, damage)
        {

        }

        public override void Collided(Sprite sprite)
        {
            if (sprite != Parent && !(sprite is Enemy) && !(sprite is Projectile))
            {
                Parent.Children.Remove(this);
                Die();
                if (sprite is Wizard)
                {
                    Wizard w = (Wizard)sprite;
                    float nonRandomDamage = Damage;
                    int newDamage = (int)(nonRandomDamage - ((nonRandomDamage / 100) * (w.Defense * 2)));
                    sprite.Hurt(MathHelper.Clamp(newDamage, 0, 999999));
                }
                else if (sprite is IHurtable)
                {
                    sprite.Hurt(Damage);
                    if (sprite.Health <= 0)
                    {
                        SpriteManager.RemoveSprite(sprite);
                        Color[] colors = ResourceHolder.TextureColorData(sprite.Texture);
                        ParticleSystem.AddParticle(new Particle(sprite.Position + new Vector2(sprite.Width / 2, sprite.Height / 2), colors));
                    }
                }
            }
        }
    }
}
