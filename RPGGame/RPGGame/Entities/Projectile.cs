using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPGGame.Sprites;

namespace RPGGame.Entities
{
    class Projectile : LivingEntity
    {
        public float Damage
        {
            get
            {
                Random r = new Random();
                return r.Next((int)((StandardDamage / 100) * 90), (int)((StandardDamage / 100) * 110));
            }
        }
        private float StandardDamage { get; set; }

        public Projectile(Texture2D texture, Vector2 position, Vector2 velocity, float life, bool rotate, Sprite parent, float damage)
            : base(texture, position, velocity, life, rotate)
        {
            Parent = parent;
            StandardDamage = damage;
        }

        public override void Collided(Sprite sprite)
        {
            base.Collided(sprite);
            if (sprite != Parent && !(sprite is Projectile))
            {
                Parent.Children.Remove(this);
                Die();

                if (sprite is IHurtable)
                {
                    sprite.Hurt(Damage);
                    if (sprite.Health <= 0)
                    {
                        SpriteManager.RemoveSprite(sprite);
                        Color[] colors = ResourceHolder.TextureColorData(sprite.Texture);
                        ParticleSystem.AddParticle(new Particle(sprite.Position + new Vector2(sprite.Width / 2, sprite.Height / 2), colors));
                        if (sprite is Enemy)
                        {
                            Enemy e = (Enemy)sprite;
                            if (HasParent)
                            {
                                Parent.Experiance += e.ExperiancePoints;
                                FloatingText.AddExp("+" + e.ExperiancePoints + " exp", new Vector2(X + (Width / 2) - 3f, Y));
                            }
                        }
                    }
                }
            }
        }
    }
}
