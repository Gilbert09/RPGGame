using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    class DirectionalEntity : Entity
    {
        private float degrees;

        public RotatedRectangle RotatedRectangle { get; set; }

        public DirectionalEntity(Texture2D texture, Vector2 position, Vector2 velocity)
            : base(texture, position, velocity)
        {
            degrees = (float)Math.Atan2(Velocity.Y, Velocity.X);
            RotatedRectangle = new RotatedRectangle(enclosingRectangle, (float)Math.Atan2(Velocity.Y, Velocity.X));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            RotatedRectangle.X = (int)X;
            RotatedRectangle.Y = (int)Y;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(Texture,
                new Vector2(X, Y) - camera.Position,
                enclosingRectangle,
                Color.White,
                degrees,
                new Vector2(1f, 1f),
                1,
                SpriteEffects.None,
                0);
        }
    }
}
