using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    class Entity : Sprite
    {
        public Vector2 Velocity { get; set; }

        public Entity(Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position)
        {
            Velocity = velocity;
        }

        public override void Update(GameTime gameTime)
        {
            this.position += Velocity;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y) - camera.Position, Color.White);
        }

        public void Die()
        {
            SpriteManager.RemoveSprite(this);
        }
    }
}
