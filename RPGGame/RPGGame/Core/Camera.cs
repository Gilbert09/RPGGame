using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RPGGame.Core
{
    public class Camera
    {
        public Vector2 Position;
        public float Speed { get; set; }
        public float Zoom { get; set; }
        public bool InFreeRoam { get; set; }

        private bool moved = false;
        private bool moving = false;
        private bool inProgress = false;

        private Vector2 spritePos;
        private Vector2 originalCamera;

        private int frame = 0;
        private int maxFrame = 60;

        public Camera(Vector2 position)
        {
            Position = position;
            Speed = 1.5f;
            Zoom = 1f;
        }

        public void Update(GameTime gameTime)
        {
            if (moving)
            {
                inProgress = true;
                MoveCamera(frame);
                frame++;
                if (frame == maxFrame)
                {
                    moved = false;
                    moving = false;
                    inProgress = false;
                    InFreeRoam = false;
                    frame = 0;
                }
            }

            Vector2 motion = Vector2.Zero;

            if (!Input.KeyDown(Keys.W) && !Input.KeyDown(Keys.A) && !Input.KeyDown(Keys.S) && !Input.KeyDown(Keys.D))
            {
                if (Input.KeyDown(Keys.Left)) motion.X = -Speed;
                else if (Input.KeyDown(Keys.Right)) motion.X = Speed;
                if (Input.KeyDown(Keys.Up)) motion.Y = -Speed;
                else if (Input.KeyDown(Keys.Down)) motion.Y = Speed;
            }

            if (motion != Vector2.Zero)
            {
                moved = true;
                motion.Normalize();
                Position += motion * Speed;
                LockCamera();
            }
        }

        private void MoveCamera(int frame)
        {
            Position = originalCamera + (((float)frame / (float)maxFrame) * (spritePos - originalCamera));
            LockCamera();
        }

        private void LockCamera()
        {
            Position.X = MathHelper.Clamp(Position.X, 0, TileMap.WidthInPixels - Tile.TileWidth - (GameScreen.ScreenWidth / 8));
            Position.Y = MathHelper.Clamp(Position.Y, 0, TileMap.HeightInPixels - Tile.TileHeight - (GameScreen.ScreenHeight / 8));
        }

        public void LockToSprite(Sprite sprite)
        {
            if (moved)
            {
                InFreeRoam = true;
                moving = true;
                if (!inProgress) originalCamera = Position;
            }
            else
            {
                Position.X = sprite.X + sprite.Width / 2 - (GameScreen.ScreenWidth / 16);
                Position.Y = sprite.Y + sprite.Height / 2 - (GameScreen.ScreenHeight / 16);
                LockCamera();
            }
            spritePos = new Vector2(sprite.X, sprite.Y) - new Vector2((GameScreen.ScreenWidth / 16) - (sprite.Width / 2), (GameScreen.ScreenHeight / 16) - (sprite.Height / 2));
        }
    }
}
