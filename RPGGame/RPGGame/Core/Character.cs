using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPGGame.Core
{
    public class Character : AnimatedSprite
    {
        protected Vector2 lastPosition;
        protected float speed;
        protected Camera camera;

        public float Damage { get; set; }

        public Character(Texture2D texture, int frameCount, int xStart, int yStart, int frameWidth, int frameHeight, Vector2 position, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(texture, frameCount, xStart, yStart, frameWidth, frameHeight, position, startingDirection, state, incStill)
        {
            this.camera = camera;
        }

        public bool TryMove(Vector2 direction)
        {
            Vector2 vector = lastPosition;
            List<Sprite> list = SpriteManager.WizardOnScreenSprites(camera);

            foreach (Sprite s in list)
            {
                if (CollidedWith(s))
                {
                    if (direction.X < 0)
                    {
                        vector = lastPosition;
                        vector.X += speed;
                        if (!CollidedWith(s, vector))
                        {
                            position = vector;
                            return true;
                        }
                    }
                    if (direction.Y < 0)
                    {
                        vector = lastPosition;
                        vector.Y += speed;
                        if (!CollidedWith(s, vector))
                        {
                            position = vector;
                            return true;
                        }
                    }
                    if (direction.X > 0)
                    {
                        vector = lastPosition;
                        vector.X += -speed;
                        if (!CollidedWith(s, vector))
                        {
                            position = vector;
                            return true;
                        }
                    }
                    if (direction.Y > 0)
                    {
                        vector = lastPosition;
                        vector.Y += -speed;
                        if (!CollidedWith(s, vector))
                        {
                            position = vector;
                            return true;
                        }
                    }
                }
            }
            position += direction;
            return false;
        }
    }
}
