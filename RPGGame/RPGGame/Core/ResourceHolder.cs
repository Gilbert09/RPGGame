using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    public static class ResourceHolder
    {
        public static void Init(ContentManager content)
        {
            //Sprites
            Wizard = content.Load<Texture2D>("SpriteSheets\\wizard");

            GoblinShield = content.Load<Texture2D>("SpriteSheets\\goblinshield");
            GoblinSword = content.Load<Texture2D>("SpriteSheets\\goblinsword");
            GoblinClub = content.Load<Texture2D>("SpriteSheets\\goblinclub");
            GoblinWizardWood = content.Load<Texture2D>("SpriteSheets\\goblinwizardwood");
            GoblinWizardCrown = content.Load<Texture2D>("SpriteSheets\\goblinwizardcrown");

            Tree = content.Load<Texture2D>("Entities\\tree");
            SinglePixel = content.Load<Texture2D>("TextureSheets\\1pixel");
            BottomPanel = content.Load<Texture2D>("SpriteSheets\\bottompanel");

            Heart = content.Load<Texture2D>("Entities\\heart");
            Exp = content.Load<Texture2D>("Entities\\exp");
            Mana = content.Load<Texture2D>("Entities\\mana");

            Defense = content.Load<Texture2D>("Entities\\defense");
            Wisdom = content.Load<Texture2D>("Entities\\wisdom");
            Agility = content.Load<Texture2D>("Entities\\agility");

            //Entities
            BlueBolt = content.Load<Texture2D>("Entities\\bluebolt");
            GreenBolt = content.Load<Texture2D>("Entities\\greenbolt");
            RedBolt = content.Load<Texture2D>("Entities\\redbolt");
            PinkBolt = content.Load<Texture2D>("Entities\\pinkbolt");

            LevelUp = content.Load<Texture2D>("Entities\\levelup");
            ManaSpecial = content.Load<Texture2D>("Entities\\manaspecial");

            //Tiles
            TileSet = content.Load<Texture2D>("TextureSheets\\tileset8");

            //Sounds
            Boing = content.Load<SoundEffect>("Sounds\\boing");
            Loop = content.Load<SoundEffect>("Sounds\\loopExample");

            //SpriteFonts
            StandardFont = content.Load<SpriteFont>("Fonts\\Standard");
            BoldFont = content.Load<SpriteFont>("Fonts\\Bold");
            SmallFont = content.Load<SpriteFont>("Fonts\\Small");
            SmallBoldFont = content.Load<SpriteFont>("Fonts\\SmallBold");

            //Title Screen
            TitleBackground = content.Load<Texture2D>("TitleScreen//titlebackground");
            TitleSelectPlay = content.Load<Texture2D>("TitleScreen//play");
            TitleSelectContinue = content.Load<Texture2D>("TitleScreen//continue");
            TitleSelectQuit = content.Load<Texture2D>("TitleScreen//quit");

            //Game Over Screen
            GameOverBackground = content.Load<Texture2D>("GameOverScreen//gameover");
            GameOverSelectReload = content.Load<Texture2D>("GameOverScreen//reload");
            GameOverSelectNew = content.Load<Texture2D>("GameOverScreen//new");
            GameOverSelectReturn = content.Load<Texture2D>("GameOverScreen//return");

            //Loading Screen
            LoadingBackground = content.Load<Texture2D>("LoadingScreen//background");
        }

        public static Color[] TextureColorData(Texture2D texture)
        {
            Color[] texColors = new Color[texture.Height * texture.Width];
            texture.GetData<Color>(texColors);
            if (texColors.Length > 64)
            {
                Color[] temp = new Color[64];
                List<Color> colorList = texColors.Where(n => (n.R != 0) && (n.G != 0) && (n.B != 0)).ToList<Color>();
                for (int i = 0; i < 64; i++) temp[i] = colorList[i];
                return temp;
            }
            return texColors;
        }

        //Sprites
        public static Texture2D Wizard;
        public static Texture2D Tree;
        public static Texture2D SinglePixel;
        public static Texture2D BottomPanel;

        public static Texture2D Heart;
        public static Texture2D Exp;
        public static Texture2D Mana;

        public static Texture2D Defense;
        public static Texture2D Wisdom;
        public static Texture2D Agility;

        public static Texture2D GoblinShield;
        public static Texture2D GoblinSword;
        public static Texture2D GoblinClub;
        public static Texture2D GoblinWizardWood;
        public static Texture2D GoblinWizardCrown;

        //Entities
        public static Texture2D BlueBolt;
        public static Texture2D RedBolt;
        public static Texture2D GreenBolt;
        public static Texture2D PinkBolt;

        public static Texture2D LevelUp;
        public static Texture2D ManaSpecial;

        //Tiles
        public static Texture2D TileSet;

        //Sounds
        public static SoundEffect Boing;
        public static SoundEffect Loop;

        //SpriteFont
        public static SpriteFont StandardFont;
        public static SpriteFont BoldFont;
        public static SpriteFont SmallFont;
        public static SpriteFont SmallBoldFont;

        //Title Screen
        public static Texture2D TitleBackground;
        public static Texture2D TitleSelectPlay;
        public static Texture2D TitleSelectContinue;
        public static Texture2D TitleSelectQuit;

        //Game Over Screen
        public static Texture2D GameOverBackground;
        public static Texture2D GameOverSelectReload;
        public static Texture2D GameOverSelectNew;
        public static Texture2D GameOverSelectReturn;
        
        //Loading Screen
        public static Texture2D LoadingBackground;
    }

    public class SpriteFrameHolder
    {
        public Texture2D Texture { get; set; }
        public int FrameCount { get; set; }
        public int XStart { get; set; }
        public int YStart { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        public SpriteFrameHolder(Texture2D texture, int frameCount, int xStart, int yStart, int frameWidth, int frameHeight)
        {
            Texture = texture;
            FrameCount = frameCount;
            XStart = xStart;
            YStart = yStart;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
        }
    }
}
