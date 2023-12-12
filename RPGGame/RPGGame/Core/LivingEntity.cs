using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    class LivingEntity : DirectionalEntity
    {
        private bool rotate;
        private float life;

        public LivingEntity(Texture2D texture, Vector2 position, Vector2 velocity, float life, bool rotate)
            : base(texture, position, velocity)
        {
            this.rotate = rotate;
            this.life = life;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            life--;
            if (life <= 0) Die();
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch, camera);
        }
    }
}
