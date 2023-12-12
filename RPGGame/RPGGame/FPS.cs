using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGGame
{
    class FPS
    {
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        SpriteFont spriteFont;

        public FPS(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("Fonts\\FPS");
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            frameCounter++;
            string fps = string.Format("fps: {0}", frameRate);

            spriteBatch.DrawString(spriteFont, fps, new Vector2(GameScreen.ScreenWidth - 75, 10), Color.White);
            
        }
    }
}
