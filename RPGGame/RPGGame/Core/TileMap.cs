using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RPGGame.Sprites;
using RPGGame.Sprites.Enemies;

namespace RPGGame.Core
{
    public class TileMap
    {
        public static int MapWidth = 256;
        public static int MapHeight = 256;
        public static int WidthInPixels = MapWidth * Tile.TileWidth;
        public static int HeightInPixels = MapHeight * Tile.TileHeight;
        public static List<EnemyBoundArea> EnemyBoundAreaList = new List<EnemyBoundArea>();
        public List<MapRow> Rows = new List<MapRow>();
        public static int TilesAcross = 15;
        public static int TilesDown = 10;

        private int totalTiles;
        private int maxMapHeight;
        private Camera camera;

        public TileMap(Camera camera)
        {
            this.camera = camera;
            totalTiles = Tile.TileSetTexture.Height / 16;

            for (int y = 0; y < MapHeight; y++)
            {
                MapRow thisRow = new MapRow();
                for (int x = 0; x < MapWidth; x++)
                {
                    thisRow.Columns.Add(new MapCell(64));
                }
                Rows.Add(thisRow);
            }

            byte[,] _map = new byte[MapWidth, MapHeight];
            string mapString = File.ReadAllText("Content\\Maps\\map.txt");
            maxMapHeight = mapString.Split(' ').Where(n => (!String.IsNullOrEmpty(n) && n != "\r\n")).Select(n => Convert.ToInt32(n)).ToArray<int>().Max();
            double devision = (maxMapHeight / totalTiles) / 2;
            int numberCount = 0;
            string[] mapRows = Regex.Split(mapString, "\r\n");
            for (int k = 0; k < mapRows.Length; k++)
            {
                numberCount = 0;
                string[] split = mapRows[k].Split(' ');
                foreach (string s in split)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        int sAsInt = Convert.ToInt32(s);
                        for (int l = 0; l < totalTiles; l++)
                        {
                            if (sAsInt >= (devision * l) && sAsInt < devision * (l + 1))
                            {
                                Rows[k].Columns[numberCount].TileID = l * (Tile.TileHeight * 4);
                            }
                        }
                        numberCount++;
                    }
                }
            }

            //Add Trees
            Random r = new Random();
            for (int i = 0; i < 5000; i++)
            {
                int xx = r.Next(256 * 8);
                int yy = r.Next(256 * 8);
                if (Rows[xx / 8].Columns[yy / 8].TileID != 0) SpriteManager.AddSprite(new Tree(new Vector2(xx, yy)));
            }
            
            //Add A few goblins
            GenerateEba(150, camera);

            doAutoTransition();
        }

        public static void GenerateEba(int count, Camera camera)
        {
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                int width = random.Next(20, 150);
                int height = random.Next(20, 150);
                int xStart = random.Next(256 * 8);
                int yStart = random.Next(256 * 8);
                int amountOfEnemies = ((5 * height) / 100) + ((5 * width) / 100);
                List<Enemy> enemies = new List<Enemy>();
                for (int k = 0; k < amountOfEnemies; k++)
                {
                    int whichEnemy = random.Next(1, 6);
                    Enemy enemy = null;
                    switch (whichEnemy)
                    {
                        case 1:
                            enemy = new GoblinShield(new SpriteFrameHolder(ResourceHolder.GoblinShield, 3, 0, 0, 8, 8), 2, AnimationState.Still, true, camera);
                            break;
                        case 2:
                            enemy = new GoblinSword(new SpriteFrameHolder(ResourceHolder.GoblinSword, 3, 0, 0, 8, 8), 2, AnimationState.Still, true, camera);
                            break;
                        case 3:
                            enemy = new GoblinClub(new SpriteFrameHolder(ResourceHolder.GoblinClub, 3, 0, 0, 8, 8), 2, AnimationState.Still, true, camera);
                            break;
                        case 4:
                            enemy = new GoblinWizardWood(new SpriteFrameHolder(ResourceHolder.GoblinWizardWood, 3, 0, 0, 8, 8), 2, AnimationState.Still, true, camera);
                            break;
                        case 5:
                            enemy = new GoblinWizardCrown(new SpriteFrameHolder(ResourceHolder.GoblinWizardCrown, 3, 0, 0, 8, 8), 2, AnimationState.Still, true, camera);
                            break;
                    }
                    enemies.Add(enemy);
                }
                EnemyBoundArea eba = new EnemyBoundArea(new Rectangle(xStart, yStart, width, height), camera);
                eba.Init(enemies);
                EnemyBoundAreaList.Add(eba);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 firstSquare = new Vector2(camera.Position.X / Tile.TileWidth, camera.Position.Y / Tile.TileHeight);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(camera.Position.X % Tile.TileWidth, camera.Position.Y % Tile.TileHeight);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            for (int y = 0; y < TilesDown; y++)
            {
                for (int x = 0; x < TilesAcross; x++)
                {
                    foreach (int tileID in Rows[y + firstY].Columns[x + firstX].BaseTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            new Vector2((x * Tile.TileWidth ) - squareOffset.X, (y * Tile.TileHeight) - squareOffset.Y),
                            Tile.GetSourceRectangle(tileID),
                            Color.White);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
        }

        private void doAutoTransition()
        {
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int height = getBaseTile(y, x);
                    int start = GetTileBaseHeight(height) + 1;
                    for (int i = start; i < totalTiles; i++)
                    {

                        int tileID = CalculateTransistionTileEdge(y, x, i);
                        if (tileID > -1)
                        {
                            Rows[y].Columns[x].AddBaseTile(i * 32 + tileID);
                        }

                        tileID = CalculateTransistionTileCorner(y, x, i);
                        if (tileID > -1)
                        {
                            Rows[y].Columns[x].AddBaseTile(i * 32 + 16 + tileID);
                        }
                    }
                }
            }
        }

        private int GetTileBaseHeight(int tileID)
        {
            return tileID / 32;
        }

        private int getBaseTile(int y, int x)
        {
            if (x < 0 || y < 0 ||
                x >= MapWidth || y >= MapHeight)
            {
                return 0;
            }

            return Rows[y].Columns[x].TileID;
        }

        private int CalculateTransistionTileEdge(int y, int x, int iHeight)
        {
            int temp = 0;

            if (GetTileBaseHeight(getBaseTile(y, x - 1)) == iHeight)
            {
                //Left
                temp += 1;
            }

            if (GetTileBaseHeight(getBaseTile(y - 1, x)) == iHeight)
            {
                //Top
                temp += 2;
            }

            if (GetTileBaseHeight(getBaseTile(y, x + 1)) == iHeight)
            {
                //Right
                temp += 4;
            }
            if (GetTileBaseHeight(getBaseTile(y + 1, x)) == iHeight)
            {
                //bottom
                temp += 8;
            }

            if (temp > 0)
            {
                return temp;
            }

            return -1;
        }

        private int CalculateTransistionTileCorner(int y, int x, int iHeight)
        {
            int temp = 0;

            if (GetTileBaseHeight(getBaseTile(y - 1, x - 1)) == iHeight)
            {
                //Left top
                temp += 1;
            }
            if (GetTileBaseHeight(getBaseTile(y - 1, x + 1)) == iHeight)
            {
                //Top right
                temp += 2;
            }
            if (GetTileBaseHeight(getBaseTile(y + 1, x + 1)) == iHeight)
            {
                //Bottem Right
                temp += 4;
            }
            if (GetTileBaseHeight(getBaseTile(y + 1, x - 1)) == iHeight)
            {
                //Bottom left
                temp += 8;
            }
            if (temp > 0)
            {
                return temp;
            }

            return -1;
        }
    }

    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }
}
