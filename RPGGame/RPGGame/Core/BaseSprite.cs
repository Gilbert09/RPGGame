using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPGGame.Core
{
    public abstract class BaseSprite
    {
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, Camera camera);

        public abstract void Collided(Sprite sprite);
    }
}
