using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPGGame.Core
{
    public static class FloatingText
    {
        private static List<Quin<string, Vector2, int, Color, SpriteFont>> textList = new List<Quin<string, Vector2, int, Color, SpriteFont>>();
        private static List<Quin<string, Vector2, int, Color, SpriteFont>> addTextList = new List<Quin<string, Vector2, int, Color, SpriteFont>>();
        private static List<Quin<string, Vector2, int, Color, SpriteFont>> removeTextList = new List<Quin<string, Vector2, int, Color, SpriteFont>>();

        public static void Update(GameTime gameTime)
        {
            foreach (Quin<string, Vector2, int, Color, SpriteFont> t in addTextList) textList.Add(t);
            addTextList.Clear();

            foreach (Quin<string, Vector2, int, Color, SpriteFont> t in textList)
            {
                t.Third -= 4;
                Vector2 v = t.Second;
                v.Y -= 0.2f;
                t.Second = v;

                if (t.Third <= 0) removeTextList.Add(t);
            }

            foreach (Quin<string, Vector2, int, Color, SpriteFont> t in removeTextList) textList.Remove(t);
            removeTextList.Clear();
        }

        public static void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (Quin<string, Vector2, int, Color, SpriteFont> t in textList)
            {
                spriteBatch.DrawString(t.Fifth, t.First, (t.Second * 8) - (camera.Position * 8), Color.FromNonPremultiplied(t.Fourth.R, t.Fourth.G, t.Fourth.B, t.Third));
            }
        }

        public static void AddDamage(string text, Vector2 position)
        {
            addTextList.Add(new Quin<string, Vector2, int, Color, SpriteFont>(text, position, 255, Color.Red, ResourceHolder.StandardFont));
        }

        public static void AddExp(string text, Vector2 position)
        {
            addTextList.Add(new Quin<string, Vector2, int, Color, SpriteFont>(text, position, 255, Color.Green, ResourceHolder.BoldFont));
        }

        public static void AddLevelUp(string text, Vector2 position)
        {
            addTextList.Add(new Quin<string, Vector2, int, Color, SpriteFont>(text, position, 255, Color.Yellow, ResourceHolder.BoldFont));
        }

        public static void Add(string text, Vector2 position, Color color, SpriteFont spriteFont)
        {
            addTextList.Add(new Quin<string, Vector2, int, Color, SpriteFont>(text, position, 255, color, spriteFont));
        }
    }

    class Quin<T, R, E, W, Q>
    {
        public T First { get; set; }
        public R Second { get; set; }
        public E Third { get; set; }
        public W Fourth { get; set; }
        public Q Fifth { get; set; }

        public Quin(T first, R second, E third, W fourth, Q fifth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
            Fifth = fifth;
        }
    }
}
