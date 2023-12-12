using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RPGGame.Sprites;
using RPGGame.Entities;

namespace RPGGame.Core
{
    public static class SpriteManager
    {
        private static List<Sprite> sprites = new List<Sprite>();

        private static List<Sprite> removeSprites = new List<Sprite>();
        private static List<Sprite> addSprites = new List<Sprite>();

        public static PlayableCharacter CurrentCharacter { get; set; }
        public static Wizard CurrentWizard { get { return (Wizard)CurrentCharacter; } }

        public static void Update(GameTime gameTime, Camera camera)
        {
            foreach (Sprite s in removeSprites) sprites.Remove(s);
            removeSprites.Clear();

            foreach (Sprite s in addSprites) sprites.Add(s);
            addSprites.Clear();

            CachedOnScreenSprites = OnScreen(camera);

            foreach (Sprite s in CachedOnScreenSprites)
            {
                s.Update(gameTime);
            }

            foreach (EnemyBoundArea eba in TileMap.EnemyBoundAreaList.ToList())
            {
                if (eba.AliveEnemies() == 0)
                {
                    TileMap.EnemyBoundAreaList.Remove(eba);
                    TileMap.GenerateEba(1, camera);
                }
            }

            CheckCollions(camera);
        }

        public static void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (Sprite s in sprites)
            {
                s.Draw(spriteBatch, camera);
            }
        }

        public static void AddSprite(Sprite sprite)
        {
            addSprites.Add(sprite);
        }

        public static void RemoveSprite(Sprite sprite)
        {
            if (sprites.Contains(sprite)) removeSprites.Add(sprite);
        }

        private static void CheckCollions(Camera camera)
        {
            Vector2 firstSquare = new Vector2(camera.Position.X / Tile.TileWidth, camera.Position.Y / Tile.TileHeight);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            List<Sprite> onScreenSprites = sprites.Where(s =>
                s.X / 8 >= firstX &&
                s.X / 8 <= firstX + TileMap.TilesAcross &&
                s.Y / 8 >= firstY &&
                s.Y / 8 <= firstY + TileMap.TilesDown).ToList<Sprite>();

            List<Sprite> livingOnScreenSprites = onScreenSprites.Where(s => s is LivingEntity || s is PlayableCharacter || s is Enemy).ToList<Sprite>();
            List<Sprite> nonLivingOnScreenSprites = onScreenSprites.Where(s => !(s is LivingEntity) && !(s is PlayableCharacter) && !(s is Enemy)).ToList<Sprite>();

            for (int i = 0; i < livingOnScreenSprites.Count; i++)
            {
                for (int j = 0; j < nonLivingOnScreenSprites.Count; j++)
                {
                    if (livingOnScreenSprites[i] is DirectionalEntity)
                    {
                        DirectionalEntity d = (DirectionalEntity)livingOnScreenSprites[i];
                        if (d.RotatedRectangle.Intersects(new Rectangle((int)nonLivingOnScreenSprites[j].X, (int)nonLivingOnScreenSprites[j].Y, nonLivingOnScreenSprites[j].Width, nonLivingOnScreenSprites[j].Height))) livingOnScreenSprites[i].Collided(nonLivingOnScreenSprites[j]);
                    } 
                    else if (livingOnScreenSprites[i].CollidedWith(nonLivingOnScreenSprites[j]))
                    {
                        livingOnScreenSprites[i].Collided(nonLivingOnScreenSprites[j]);
                    }
                }

                for (int k = 0; k < livingOnScreenSprites.Count; k++)
                {
                    if (i == k) continue;
                    if (livingOnScreenSprites[i].CollidedWith(livingOnScreenSprites[k]))
                    {
                        livingOnScreenSprites[i].Collided(livingOnScreenSprites[k]);
                    }
                }
            }
        }

        public static List<Sprite> CachedOnScreenSprites = null;

        public static List<Sprite> OnScreen(Camera camera)
        {
            Vector2 firstSquare = new Vector2(camera.Position.X / Tile.TileWidth, camera.Position.Y / Tile.TileHeight);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            List<Sprite> onScreenSprites = sprites.Where(s =>
                (s.X / 8 >= firstX - 8 &&
                s.X / 8 <= firstX + TileMap.TilesAcross + 16 &&
                s.Y / 8 >= firstY - 8 &&
                s.Y / 8 <= firstY + TileMap.TilesDown + 16)
                || s is PlayableCharacter
                ).ToList<Sprite>();
            
            return onScreenSprites;
        }

        public static List<Sprite> WizardOnScreenSprites(Camera camera)
        {
            Vector2 firstSquare = new Vector2(camera.Position.X / Tile.TileWidth, camera.Position.Y / Tile.TileHeight);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            List<Sprite> onScreenSprites = sprites.Where(s =>
                s.X / 8 >= firstX - 8 &&
                s.X / 8 <= firstX + TileMap.TilesAcross + 16 &&
                s.Y / 8 >= firstY - 8 &&
                s.Y / 8 <= firstY + TileMap.TilesDown + 16
                && !(s is Projectile)
                ).ToList<Sprite>();

            return onScreenSprites;
        }

        public static List<Sprite> OnScreenEnemies(Camera camera)
        {
            Vector2 firstSquare = new Vector2(camera.Position.X / Tile.TileWidth, camera.Position.Y / Tile.TileHeight);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            List<Sprite> onScreenSprites = sprites.Where(s =>
                s.X / 8 >= firstX - 3 &&
                s.X / 8 <= firstX + TileMap.TilesAcross + 3 &&
                s.Y / 8 >= firstY - 3 &&
                s.Y / 8 <= firstY + TileMap.TilesDown + 3
                && s is Enemy
                ).ToList<Sprite>();

            return onScreenSprites;
        }
    }
}
