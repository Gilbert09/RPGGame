using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    public class Sprite : BaseSprite
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle enclosingRectangle;

        public bool Passable = true;
        public List<Sprite> Children = new List<Sprite>();
        public Sprite Parent { get; set; }
        public bool HasParent { get { return Parent == null ? false : true; } }
        public int Experiance { get; set; }

        public float X { get { return position.X; } set { position.X = value; } }
        public float Y { get { return position.Y; } set { position.Y = value; } }
        public int Width { get { return enclosingRectangle.Width; } }
        public int Height { get { return enclosingRectangle.Height; } }
        public Texture2D Texture { get { return texture; } }
        public Rectangle EnclosingRectangle { get { return enclosingRectangle; } }
        public float Health { get; set; }
        public Vector2 Position
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.enclosingRectangle = texture.Bounds;
            this.Experiance = 0;
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y) - camera.Position, Color.White);
        }

        public override void Collided(Sprite sprite)
        {
            
        }

        public bool CollidedWith(Sprite otherSprite)
        {
            Rectangle r1 = new Rectangle((int)position.X, (int)position.Y, Width, Height);
            Rectangle r2 = new Rectangle((int)otherSprite.X, (int)otherSprite.Y, otherSprite.Width, otherSprite.Height);
            return r1.Intersects(r2);
        }

        public bool CollidedWith(Sprite otherSprite, Vector2 newPosition)
        {
            Rectangle r1 = new Rectangle((int)newPosition.X, (int)newPosition.Y, Width, Height);
            Rectangle r2 = new Rectangle((int)otherSprite.X, (int)otherSprite.Y, otherSprite.Width, otherSprite.Height);
            return r1.Intersects(r2);
        }

        public void Hurt(float damage)
        {
            Health -= damage;
            FloatingText.AddDamage(damage.ToString(), new Vector2(X + (Width / 2) - 1.5f, Y));
            if (Health <= 0)
            {
                SpriteManager.RemoveSprite(this);
                Color[] colors = ResourceHolder.TextureColorData(Texture);
                ParticleSystem.AddParticle(new Particle(Position + new Vector2(Width / 2, Height / 2), colors));
            }
        }
    }
}
