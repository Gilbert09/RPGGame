using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    public class EnemyBoundArea
    {
        public Rectangle Rectangle { get; set; }

        private List<Enemy> enemies = new List<Enemy>();
        private Camera camera;

        public EnemyBoundArea(Rectangle rectangle, Camera camera)
        {
            Rectangle = rectangle;
            this.camera = camera;
        }

        public void Init(List<Enemy> list)
        {
            enemies = list;

            Random random = new Random();
            foreach (Enemy e in enemies)
            {
                e.BoundingArea = this;
                e.BoundingAreaDirection = random.Next(3);
                e.StartingPositionInArea = e.PositionInArea = new Vector2(random.Next(Rectangle.Width), random.Next(Rectangle.Height));
                e.Position = new Vector2(Rectangle.X + e.PositionInArea.X, Rectangle.Y + e.PositionInArea.Y);
                SpriteManager.AddSprite(e);
            }
        }

        public void Deinit()
        {
            foreach (Enemy e in enemies.ToList())
            {
                SpriteManager.RemoveSprite(e);
                enemies.Remove(e);
            }
        }

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
        }

        public int AliveEnemies()
        {
            return enemies.Where(x => x.Health > 0).ToList().Count;
        }
    }
}