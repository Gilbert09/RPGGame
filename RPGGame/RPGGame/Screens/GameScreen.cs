using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RPGGame.Core;
using RPGGame.Sprites;

namespace RPGGame
{
    public class GameScreen : Microsoft.Xna.Framework.Game
    {
        //Fields + Settings
        public static int ScreenWidth = 896;
        public static int ScreenHeight = 554;

        public static int WindowWidth = 896;
        public static int WindowHeight = 704;

        public static GameState GameState = RPGGame.GameState.Menu;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static TileMap map = null;
        Camera camera = new Camera(new Vector2(0, 0));
        FPS fps;
        Wizard wiz;
        GameUI gameUI;

        //Title Screen
        Texture2D titleBg;
        Texture2D titlePlay;
        Texture2D titleContinue;
        Texture2D titleQuit;
        private int titleMenuItem = 0;

        //Game Over Screen
        Texture2D gameOverBg;
        Texture2D gameOverReload;
        Texture2D gameOverNew;
        Texture2D gameOverReturn;
        private int gameOverMenuItem = 0;

        //Loading Screen
        Texture2D loadingBackground;
        bool loaded = false;

        public GameScreen()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            IsMouseVisible = true;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ResourceHolder.Init(Content);
            Settings.Init();

            Tile.TileSetTexture = ResourceHolder.TileSet;

            ParticleSystem.Init(GraphicsDevice, camera);

            
            fps = new FPS(Content);
            gameUI = new GameUI();
            wiz = new Wizard(ResourceHolder.Wizard, 3, 0, 0, 8, 8, new Vector2(1024, 1024), AnimationDirection.Down, AnimationState.Still, true, camera);
            SpriteManager.AddSprite(wiz);
            SpriteManager.CurrentCharacter = wiz;

            camera.LockToSprite(wiz);

            titleBg = ResourceHolder.TitleBackground;
            titlePlay = ResourceHolder.TitleSelectPlay;
            titleContinue = ResourceHolder.TitleSelectContinue;
            titleQuit = ResourceHolder.TitleSelectQuit;

            gameOverBg = ResourceHolder.GameOverBackground;
            gameOverReload = ResourceHolder.GameOverSelectReload;
            gameOverNew = ResourceHolder.GameOverSelectNew;
            gameOverReturn = ResourceHolder.GameOverSelectReturn;

            loadingBackground = ResourceHolder.LoadingBackground;
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            Input.KeyboardState = Keyboard.GetState();
            Input.MouseState = Mouse.GetState();
            if (Input.LastKeyboardState == null) Input.LastKeyboardState = Keyboard.GetState();

            if (GameState == RPGGame.GameState.Menu)
            {
                if (Input.KeyPress(Keys.Down)) titleMenuItem = (int)MathHelper.Clamp(titleMenuItem + 1, 0, 2);
                else if (Input.KeyPress(Keys.Up)) titleMenuItem = (int)MathHelper.Clamp(titleMenuItem - 1, 0, 2);
                else if (Input.KeyPress(Keys.Enter))
                {
                    switch (titleMenuItem)
                    {
                        case 0:
                            Settings.ResetSettings();
                            wiz.Reload(false);
                            Settings.SaveWizard(wiz);
                            loaded = false;
                            GameState = RPGGame.GameState.Loading;
                            break;
                        case 1:
                            loaded = false;
                            GameState = RPGGame.GameState.Loading;
                            break;
                        case 2:
                            Exit();
                            break;
                    }
                }
            }
            else if (GameState == RPGGame.GameState.Loading)
            {
                if (!loaded)
                {
                    map = new TileMap(camera);
                    SpriteManager.Update(gameTime, camera);

                    foreach (EnemyBoundArea eba in TileMap.EnemyBoundAreaList.Where(n => new Rectangle((int)wiz.X - 74, (int)wiz.Y - 74, 148, 148).Intersects(n.Rectangle)).ToList())
                    {
                        eba.Deinit();
                        TileMap.EnemyBoundAreaList.Remove(eba);
                    }
                    loaded = true;
                    GameState = RPGGame.GameState.Game;
                }
            }
            else if (GameState == RPGGame.GameState.Game)
            {
                map.Update(gameTime); //Camera Movements
                SpriteManager.Update(gameTime, camera); //Update Sprites
                ParticleSystem.Update(gameTime); //Updates Particles
                FloatingText.Update(gameTime); //Updates Floating Text
                fps.Update(gameTime); //Updates FPS

                if (Input.KeyPress(Keys.Enter))
                {
                    Settings.SaveWizard(wiz);
                    FloatingText.Add("Saved!", wiz.Position, Color.Blue, ResourceHolder.BoldFont);
                }

                if (Input.KeyPress(Keys.Escape))
                {
                    Settings.SaveWizard(wiz);
                    GameState = RPGGame.GameState.Menu;
                }

                if (wiz.Health <= 0) GameState = RPGGame.GameState.GameOver;
            }
            else if (GameState == RPGGame.GameState.GameOver)
            {
                if (Input.KeyPress(Keys.Down)) gameOverMenuItem = (int)MathHelper.Clamp(gameOverMenuItem + 1, 0, 2);
                else if (Input.KeyPress(Keys.Up)) gameOverMenuItem = (int)MathHelper.Clamp(gameOverMenuItem - 1, 0, 2);
                else if (Input.KeyPress(Keys.Enter))
                {
                    switch (gameOverMenuItem)
                    {
                        case 0:
                            wiz.Reload(true);
                            SpriteManager.AddSprite(wiz);
                            GameState = RPGGame.GameState.Game;
                            break;
                        case 1:
                            Settings.ResetSettings();
                            wiz.Reload(false);
                            SpriteManager.AddSprite(wiz);
                            GameState = RPGGame.GameState.Game;
                            break;
                        case 2:
                            GameState = RPGGame.GameState.Menu;
                            break;
                    }
                }
            }

            Input.LastKeyboardState = Input.KeyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (GameState == RPGGame.GameState.Menu)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(1.0f));

                spriteBatch.Draw(titleBg, new Vector2(0, 0), Color.White);
                switch (titleMenuItem)
                {
                    case 0:
                        spriteBatch.Draw(titlePlay, new Vector2(316, 450), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(titleContinue, new Vector2(316, 450), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(titleQuit, new Vector2(316, 450), Color.White);
                        break;
                }

                spriteBatch.End();
            }
            else if (GameState == RPGGame.GameState.Loading)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(1.0f));
                spriteBatch.Draw(loadingBackground, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }
            else if (GameState == RPGGame.GameState.Game)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(8.0f));

                map.Draw(spriteBatch); //Draw Map
                SpriteManager.Draw(spriteBatch, camera); //Draw All Sprites
                ParticleSystem.Draw(spriteBatch); //Draws Particles
                gameUI.Draw(spriteBatch); //Draws Scaled UI

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(1.0f));
                fps.Draw(spriteBatch); //Draws FPS With Smaller Scale
                SpriteFont spriteFont = Content.Load<SpriteFont>("Fonts\\FPS");
                spriteBatch.DrawString(spriteFont, "X: " + wiz.X + " Y: " + wiz.Y, new Vector2(GameScreen.ScreenWidth - 450, 10), Color.White);

                FloatingText.Draw(spriteBatch, camera); //Draws Floating Text
                gameUI.DrawText(spriteBatch); //Draws Unscaled Text
                gameUI.DrawMinimap(spriteBatch, camera); //Draw Minimap

                spriteBatch.End();
            }
            else if (GameState == RPGGame.GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateScale(1.0f));

                spriteBatch.Draw(gameOverBg, new Vector2(0, 0), Color.White);
                switch (gameOverMenuItem)
                {
                    case 0:
                        spriteBatch.Draw(gameOverReload, new Vector2(162, 450), Color.White);
                        break;
                    case 1:
                        spriteBatch.Draw(gameOverNew, new Vector2(162, 450), Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(gameOverReturn, new Vector2(162, 450), Color.White);
                        break;
                }

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            if (GameState == RPGGame.GameState.Game) Settings.SaveWizard(wiz);
        }
    }
    public enum GameState
    {
        Menu,
        Loading,
        Game,
        GameOver
    }

}