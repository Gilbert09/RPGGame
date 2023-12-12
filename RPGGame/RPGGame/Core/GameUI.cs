using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPGGame.Sprites;

namespace RPGGame.Core
{
    class GameUI
    {
        public GameUI() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            Wizard character = SpriteManager.CurrentWizard;
            spriteBatch.Draw(ResourceHolder.BottomPanel, new Rectangle(0, (GameScreen.WindowHeight - 136) / 8, 896 / 8, 136 / 8), Color.White); //Panel
            spriteBatch.Draw(ResourceHolder.Heart, new Rectangle(1, (GameScreen.WindowHeight - 120) / 8, 4, 4), Color.White); //Heart Symbol
            spriteBatch.Draw(ResourceHolder.Exp, new Rectangle(1, (GameScreen.WindowHeight - 80) / 8, 4, 4), Color.White); //Exp Symbol
            spriteBatch.Draw(ResourceHolder.Mana, new Rectangle(1, (GameScreen.WindowHeight - 40) / 8, 4, 4), Color.White); //Mana Symbol

            spriteBatch.Draw(ResourceHolder.Defense, new Rectangle(68, (GameScreen.WindowHeight - 120) / 8, 4, 4), Color.White); //Defense Symbol
            spriteBatch.Draw(ResourceHolder.Wisdom, new Rectangle(68, (GameScreen.WindowHeight - 80) / 8, 4, 4), Color.White); //Wisdom Symbol
            spriteBatch.Draw(ResourceHolder.Agility, new Rectangle(68, (GameScreen.WindowHeight - 40) / 8, 4, 4), Color.White); //Agility Symbol

            //Draw Health Bar
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle(6, 74, 36, 2), Color.DimGray);
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle(6, 74, (int)Math.Ceiling(36 * (character.Health / character.MaxHealth)), 2), Color.Red);

            //Draw Exp Bar
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle(6, 79, 36, 2), Color.DimGray);
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle(6, 79, (int)Math.Floor(36 * ((double)character.Experiance / character.MaxExperiance)), 2), Color.Green);

            //Draw Mana Bar
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle(6, 84, 36, 2), Color.DimGray);
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle(6, 84, (int)Math.Floor(36 * ((double)character.Mana / character.MaxMana)), 2), Color.DodgerBlue);
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            Wizard character = SpriteManager.CurrentWizard;

            //Draw Health Text
            Vector2 healthSize = ResourceHolder.SmallFont.MeasureString(character.Health + "/" + character.MaxHealth);
            spriteBatch.DrawString(ResourceHolder.SmallFont, character.Health + "/" + character.MaxHealth, new Vector2(64 + (16 * 8) - (healthSize.X / 2), 73.5f * 8), Color.Black);

            //Draw Exp Text
            Vector2 expSize = ResourceHolder.SmallFont.MeasureString(character.Experiance + "/" + character.MaxExperiance);
            spriteBatch.DrawString(ResourceHolder.SmallFont, character.Experiance + "/" + character.MaxExperiance, new Vector2(64 + (16 * 8) - (expSize.X / 2), 78.5f * 8), Color.Black);

            //Draw Exp Text
            Vector2 manaSize = ResourceHolder.SmallFont.MeasureString(character.Mana + "/" + character.MaxMana);
            spriteBatch.DrawString(ResourceHolder.SmallFont, character.Mana + "/" + character.MaxMana, new Vector2(64 + (16 * 8) - (manaSize.X / 2), 83.5f * 8), Color.Black);

            //Draw Level
            spriteBatch.DrawString(ResourceHolder.SmallBoldFont, "Lvl " + character.Level, new Vector2(48 + 3, 78.6f * 8), Color.Black);

            //Draw Stats
            spriteBatch.DrawString(ResourceHolder.SmallBoldFont, character.Defense.ToString(), new Vector2(73 * 8, 73.5f * 8), Color.White);
            spriteBatch.DrawString(ResourceHolder.SmallBoldFont, character.Wisdom.ToString(), new Vector2(73 * 8, 78.6f * 8), Color.White);
            spriteBatch.DrawString(ResourceHolder.SmallBoldFont, character.Agility.ToString(), new Vector2(73 * 8, 83.5f * 8), Color.White);
        }

        public void DrawMinimap(SpriteBatch spriteBatch, Camera camera)
        {
            List<MapRow> rows = GameScreen.map.Rows;

            Vector2 firstSquare = new Vector2((camera.Position.X - 32) / Tile.TileWidth, (camera.Position.Y - 24) / Tile.TileHeight);
            int firstX = firstSquare.X < 0 ? 0 : (int)firstSquare.X;
            int firstY = firstSquare.Y < 0 ? 0 : (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2((firstX == 0 ? 0 : camera.Position.X) % Tile.TileWidth, (firstY == 0 ? 0 : camera.Position.Y) % Tile.TileHeight);

            Vector2 mapOffset = new Vector2((GameScreen.WindowWidth / 2) - 88, GameScreen.WindowHeight - 128);

            for (int y = 0; y < TileMap.TilesDown + 7; y++)
            {
                for (int x = 0; x < TileMap.TilesAcross + 7; x++)
                {
                    foreach (int tileID in rows[(int)MathHelper.Clamp(y + firstY, 0, 255)].Columns[(int)MathHelper.Clamp(x + firstX, 0, 255)].BaseTiles)
                    {
                        spriteBatch.Draw(
                            ResourceHolder.SinglePixel,
                            new Vector2((x * Tile.TileWidth) - squareOffset.X + mapOffset.X, (y * Tile.TileHeight) - squareOffset.Y + mapOffset.Y),
                            Tile.GetSourceRectangle(tileID),
                            rows[(int)MathHelper.Clamp(y + firstY, 0, 255)].Columns[(int)MathHelper.Clamp(x + firstX, 0, 255)].MinimapColor);
                    }
                }
            }

            spriteBatch.Draw(ResourceHolder.SinglePixel, SpriteManager.CurrentCharacter.Position - (camera.Position - new Vector2(32, 24)) + mapOffset, Tile.GetSourceRectangle(64), Color.Yellow);

            foreach (Sprite s in SpriteManager.OnScreenEnemies(camera))
            {
                spriteBatch.Draw(ResourceHolder.SinglePixel, s.Position - (camera.Position - new Vector2(32, 24)) + mapOffset, Tile.GetSourceRectangle(64), Color.Red);
            }

            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle((GameScreen.WindowWidth / 2) - 102, GameScreen.WindowHeight - 136, 80 * 8, 8), Color.Black); //Top
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle((GameScreen.WindowWidth / 2) + 82, GameScreen.WindowHeight - 136, 8, 180), Color.Black); //Right
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle((GameScreen.WindowWidth / 2) - 96, GameScreen.WindowHeight - 8, 180, 8), Color.Black); //Bottom
            spriteBatch.Draw(ResourceHolder.SinglePixel, new Rectangle((GameScreen.WindowWidth / 2) - 96, GameScreen.WindowHeight - 136, 8, 180), Color.Black); //Left
        }
    }
}
