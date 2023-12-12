using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace RPGGame.Core
{
    public class AnimatedSprite : Sprite
    {
        public AnimationState animationState;
        public int direction;
        public int frameCount;
        public int xStart;
        public int yStart;
        public int frameWidth;
        public int frameHeight;
        public int currentFrame;
        public int frameNumber;
        public bool incStill;

        public AnimatedSprite(Texture2D texture, int frameCount, int xStart, int yStart, int frameWidth, int frameHeight, Vector2 position, int startingDirection, AnimationState state, bool incStill)
            : base(texture, position)
        {
            this.animationState = state;
            this.direction = startingDirection;
            this.frameCount = frameCount;
            this.xStart = xStart;
            this.yStart = startingDirection * frameHeight;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.incStill = incStill;
            enclosingRectangle = new Rectangle(enclosingRectangle.X, enclosingRectangle.Y, frameWidth, frameHeight);
            if (incStill) { this.frameCount--; this.xStart += frameWidth; }
            currentFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            frameNumber++;
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            int xOffSet;
            int yOffSet;

            if (animationState == AnimationState.Still)
            {
                currentFrame = 0;
                xOffSet = (xStart + (currentFrame % (frameCount + 1)) * frameWidth) - frameWidth;
                yOffSet = yStart + (currentFrame / frameCount) * frameHeight;
                spriteBatch.Draw(Texture,
                    new Vector2(X - camera.Position.X, Y - camera.Position.Y),
                    //new Rectangle((int)X - (int)camera.Position.X, (int)Y - (int)camera.Position.Y, frameWidth, frameHeight),
                    new Rectangle(xOffSet, yOffSet, frameWidth, frameHeight), Color.White);
            }
            else if (animationState == AnimationState.Animating)
            {
                currentFrame = (frameNumber / 20) % frameCount;
                xOffSet = xStart + (currentFrame % frameCount) * frameWidth;
                yOffSet = yStart + (currentFrame / frameCount) * frameHeight;
                spriteBatch.Draw(Texture,
                    new Vector2(X - camera.Position.X, Y - camera.Position.Y),
                    //new Rectangle((int)X - (int)camera.Position.X, (int)Y - (int)camera.Position.Y, frameWidth, frameHeight),
                    new Rectangle(xOffSet, yOffSet, frameWidth, frameHeight), Color.White);
            }
        }

        public void ChangeState(AnimationState state)
        {
            animationState = state;
        }

        public void ChangeDirection(int direction)
        {
            this.direction = direction;
            this.yStart = direction * frameHeight;
        }

        public void LockToMap()
        {
            position.X = MathHelper.Clamp(position.X, 0, TileMap.WidthInPixels - Tile.TileWidth - frameWidth);
            position.Y = MathHelper.Clamp(position.Y, 0, TileMap.HeightInPixels - Tile.TileWidth - frameHeight);
        }
    }

    public static class AnimationDirection {
        public const int Down = 0;
        public const int Up = 1;
        public const int Left = 2;
        public const int Right = 3;
    }

    public enum AnimationState { Animating, Still }
}
