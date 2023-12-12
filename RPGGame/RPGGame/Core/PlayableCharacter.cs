using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPGGame.Core
{
    public class PlayableCharacter : Character
    {
        public PlayableCharacter(Texture2D texture, int frameCount, int xStart, int yStart, int frameWidth, int frameHeight, Vector2 position, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(texture, frameCount, xStart, yStart, frameWidth, frameHeight, position, startingDirection, state, incStill, camera)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Vector2 motion = Vector2.Zero;

            lastPosition = position;

            if (Input.KeyDown(Keys.A))
            {
                ChangeDirection(AnimationDirection.Left);
                if (animationState == AnimationState.Still) ChangeState(AnimationState.Animating);
                motion.X = -speed;
            }
            else if (Input.KeyDown(Keys.D))
            {
                ChangeDirection(AnimationDirection.Right);
                if (animationState == AnimationState.Still) ChangeState(AnimationState.Animating);
                motion.X = speed;
            }
            if (Input.KeyDown(Keys.W))
            {
                ChangeDirection(AnimationDirection.Up);
                if (animationState == AnimationState.Still) ChangeState(AnimationState.Animating);
                motion.Y = -speed;
            }
            else if (Input.KeyDown(Keys.S))
            {
                ChangeDirection(AnimationDirection.Down);
                if (animationState == AnimationState.Still) ChangeState(AnimationState.Animating);
                motion.Y = speed;
            }

            if (!Input.KeyDown(Keys.A) && !Input.KeyDown(Keys.D) && !Input.KeyDown(Keys.W) && !Input.KeyDown(Keys.S) && animationState != AnimationState.Still) ChangeState(AnimationState.Still);

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                TryMove(motion * speed);
                LockToMap();
                camera.LockToSprite(this);
            }
        }
    }
}
