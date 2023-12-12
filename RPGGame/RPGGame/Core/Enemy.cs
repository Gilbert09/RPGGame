using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPGGame.Sprites;

namespace RPGGame.Core
{
    public class Enemy : AnimatedSprite
    {
        public EnemyStance EnemyStance { get; set; }
        public EnemyState EnemyState { get; set; }
        public float TileViewRange { get; set; }
        public float TileReturnRange { get; set; }
        public EnemyBoundArea BoundingArea { get; set; }
        public Vector2 PositionInArea { get; set; }
        public Vector2 StartingPositionInArea { get; set; }
        public int BoundingAreaDirection { get; set; } //Direction: 0 = up, 1 = right, 2 = down, 3 = left

        public int ExperiancePoints { get; set; }

        public float Damage { get; set; }
        public float ShootRate { get; set; }
        public float ShootCoolDownRate { get; set; }

        public Vector2 lastPosition;
        public float speed;
        protected Camera camera;
        public bool shootCoolDown = false;

        public Enemy(SpriteFrameHolder sprite, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(sprite.Texture, sprite.FrameCount, sprite.XStart, sprite.YStart, sprite.FrameWidth, sprite.FrameHeight, new Vector2(0, 0), startingDirection, state, incStill)
        {
            this.camera = camera;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            lastPosition = position;

            if (shootCoolDown)
            {
                ShootCoolDownRate--;
                if (ShootCoolDownRate <= 0)
                {
                    ShootCoolDownRate = ShootRate;
                    shootCoolDown = false;
                }
            }

            PlayableCharacter current = SpriteManager.CurrentCharacter;
            Vector2 pos = current.Position;
            float distance = Vector2.Distance(pos, position);
            int tileDistance = (int)(distance / 8);
            if (tileDistance <= TileViewRange) EnemyStance = EnemyStance.Aggressive;
            else if (tileDistance >= TileReturnRange && EnemyStance == EnemyStance.Aggressive) EnemyStance = EnemyStance.MovingToGuarding;
            else if (tileDistance >= TileReturnRange && EnemyStance != EnemyStance.MovingToGuarding) EnemyStance = EnemyStance.Guarding;

            if (EnemyStance == EnemyStance.MovingToGuarding)
            {
                if (Y <= BoundingArea.Rectangle.Y + StartingPositionInArea.Y + 1 && Y >= BoundingArea.Rectangle.Y + StartingPositionInArea.Y - 1 && X <= BoundingArea.Rectangle.X + StartingPositionInArea.X + 1 && X >= BoundingArea.Rectangle.X + StartingPositionInArea.X - 1) EnemyStance = EnemyStance.Guarding;
                else
                {
                    float angle = (float)Math.Atan2(BoundingArea.Rectangle.Y + StartingPositionInArea.Y - Y, BoundingArea.Rectangle.X + StartingPositionInArea.X - X);
                    Vector2 angleDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    angleDirection = angleDirection * speed;
                    TryMove(angleDirection);
                }
            }
            else if (EnemyStance == EnemyStance.Guarding)
            {
                int distanceToWalk = 20;
                switch (BoundingAreaDirection)
                {
                    case 0:
                        TryMove(new Vector2(0, -speed));
                        if (position.Y <= BoundingArea.Rectangle.Y + StartingPositionInArea.Y - distanceToWalk) BoundingAreaDirection = 2;
                        break;
                    case 1:
                        TryMove(new Vector2(speed, 0));
                        if (position.X >= BoundingArea.Rectangle.X + StartingPositionInArea.X + distanceToWalk) BoundingAreaDirection = 3;
                        break;
                    case 2:
                        TryMove(new Vector2(0, speed));
                        if (position.Y >= BoundingArea.Rectangle.Y + StartingPositionInArea.Y + distanceToWalk) BoundingAreaDirection = 0;
                        break;
                    case 3:
                        TryMove(new Vector2(-speed, 0));
                        if (position.X <= BoundingArea.Rectangle.X + StartingPositionInArea.X - distanceToWalk) BoundingAreaDirection = 1;
                        break;
                }
            }
            else if (EnemyStance == EnemyStance.Aggressive)
            {
                float angle = (float)Math.Atan2(current.Y - Y, current.X - X);
                Vector2 angleDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                angleDirection = angleDirection * speed;
                TryMove(angleDirection);
            }
        }

        public override void Collided(Sprite sprite)
        {
            base.Collided(sprite);
            if (sprite is Wizard && !shootCoolDown)
            {
                shootCoolDown = true;
                Wizard w = (Wizard)sprite;
                float nonRandomDamage = Damage;
                int newDamage = (int)(nonRandomDamage - ((nonRandomDamage / 100) * (w.Defense * 2)));
                sprite.Hurt(MathHelper.Clamp(newDamage, 0, 9999999));
            }
            else if (sprite is IHurtable && !shootCoolDown && !(sprite is Enemy))
            {
                shootCoolDown = true;
                sprite.Hurt(Damage);
            }
        }

        public bool TryMove(Vector2 direction)
        {
            Vector2 vector = lastPosition;
            List<Sprite> list = SpriteManager.CachedOnScreenSprites;

            if (direction.X > 0) ChangeDirection(AnimationDirection.Right);
            else if (direction.X < 0) ChangeDirection(AnimationDirection.Left);
            ChangeState(AnimationState.Animating);

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

    public enum EnemyState
    {
        Friendly = 0,
        Passive = 1,
        Hostile = 2
    }

    public enum EnemyStance
    {
        Guarding = 0,
        MovingToGuarding = 1,
        Aggressive = 2
    }
}
