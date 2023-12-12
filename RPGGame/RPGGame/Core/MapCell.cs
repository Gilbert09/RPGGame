using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    public class MapCell
    {
        public List<int> BaseTiles = new List<int>();

        public int TileID
        {
            get { return BaseTiles.Count > 0 ? BaseTiles[0] : 0; }
            set
            {
                if (BaseTiles.Count > 0)
                    BaseTiles[0] = value;
                else
                    AddBaseTile(value);

                Rectangle r = Tile.GetSourceRectangle(value);
                Color[] colors = new Color[Tile.TileSetTexture.Width * Tile.TileSetTexture.Height];
                Tile.TileSetTexture.GetData<Color>(colors);
                int selected = r.Y * Tile.TileSetTexture.Width;
                MinimapColor = colors[selected];
            }
        }

        public Color MinimapColor { get; set; }

        public MapCell(int tileID)
        {
            TileID = tileID;
        }

        public void AddBaseTile(int tileID)
        {
            BaseTiles.Add(tileID);
        }
    }
}
