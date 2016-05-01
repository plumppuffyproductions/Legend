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
using Legend.levels;
using Legend.weapons;
using Legend.levels.functions;
using Legend.levels.sublevels;
using Legend.inventory;
using Legend.particles;
using System.Xml;

namespace Legend
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int width;
        int height;

        public static Camera Camera;
        BasicEffect effect;

        public static XmlDocument xmlDoc;
        public static String saveFile = "save.xml";
        public static Random rand = new Random();
        RenderTarget2D rend;
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Home home;
        Intro intro;
        Intro continueintro;
        GameOver gameover;
        KeyboardState ks;
        KeyboardState lastks;
        MouseState ms;
        public static bool transitioneffect = false;
        public static Vector2 rendpos = Vector2.Zero;
        public static Color rendColor = Color.White;
        public static int level = 1;
        public static List<Level> levellist = new List<Level>();
        float rendscale = 1f;
        public static string name = "";
        public static Screens screen = Screens.Home;
        public static Inventory inventory;
        public static int rendOffset = 0;
        public static bool resetRend = false;
        public static HealthManager healthManager;
        public static float deathspeed = 1;
        public static bool toinitialize = false;
        public static bool quitbool = false;

        List<int> Size = new List<int>();
        int currentSize = 0;

        public static string currentSong;

        public static TransitionToLevelEffect ttle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            MediaPlayer.IsVisualizationEnabled = true;
            base.Initialize();
            DisplayMode display = GraphicsDevice.DisplayMode;
            if (display.Width > display.Height)
            {
                int num = display.Height - 100;
                graphics.PreferredBackBufferWidth = num;
                graphics.PreferredBackBufferHeight = num;
                Settings.Scale = num / 310;
                while (num % 310 != 0)
                {
                    num--;
                    graphics.PreferredBackBufferWidth = num;
                    graphics.PreferredBackBufferHeight = num;
                    Settings.Scale = num / 310;
                    graphics.ApplyChanges();
                }
            }
            else
            {
                int num = display.Width - 100;
                graphics.PreferredBackBufferWidth = num;
                graphics.PreferredBackBufferHeight = num;
                Settings.Scale = num / 310;
                while (num % 310 != 0)
                {
                    num--;
                    graphics.PreferredBackBufferWidth = num;
                    graphics.PreferredBackBufferHeight = num;
                    Settings.Scale = num / 310;
                    graphics.ApplyChanges();
                }
            }
            graphics.ApplyChanges();
            for (int i = 620; i < display.Height; i += 310)
            {
                Size.Add(i);
            }

            rend = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            Camera = new Legend.Camera(GraphicsDevice, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));
            effect = new BasicEffect(GraphicsDevice);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
        }

        protected override void LoadContent()
        {


            ttle = new TransitionToLevelEffect();

            GameContent.LoadContent(Content);
            GameContent.Loadxml(Content);

            currentSong = "8bit";
            MediaPlayer.Play(GameContent.eightbit);
            MediaPlayer.IsRepeating = true;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            home = new Home(GameContent.normalfont, GameContent.button, GameContent.buttonhover, GameContent.logo);
            intro = new Intro(GameContent.normalfont, Content.Load<Texture2D>("guis/text box"), GameContent.button, GameContent.buttonhover, GameContent.typewriter, GameContent.spacebar, false);
            continueintro = new Intro(GameContent.normalfont, Content.Load<Texture2D>("guis/text box"), GameContent.button, GameContent.buttonhover, GameContent.typewriter, GameContent.spacebar, true);
            levellist = SubLevels.registerLevels(GameContent.playermove, GameContent.playerattack, GameContent.grass, GameContent.grassbarrier, GameContent.foamsword, GameContent.portal, GameContent.eightbit, GameContent.cantina_theme, GameContent.arcade, GameContent.normalfont, GameContent.button, GameContent.buttonhover, GameContent.tshirt, GameContent.fourpixels, GameContent.slimeparticle, GameContent.skyportal, GameContent.tooltip, GameContent.descriptionsfont, GameContent.keytxture, GameContent.keydown);
            gameover = new GameOver(GameContent.gameovertexture, GameContent.button, GameContent.buttonhover, GameContent.normalfont);
            inventory = new Inventory(GameContent.invtxture, GameContent.selectedinventory);
            healthManager = new HealthManager(GameContent.hitparticle, GameContent.fourpixels);
            width = GraphicsDevice.Viewport.Width;
            height = GraphicsDevice.Viewport.Height;
        }
        protected override void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            ks = Keyboard.GetState();
            width = GraphicsDevice.Viewport.Width;
            height = GraphicsDevice.Viewport.Height;
            if (ks.IsKeyDown(Keys.LeftControl) && lastks.IsKeyUp(Keys.LeftControl))
            {
                currentSize++;
                currentSize %= Size.Count;

                graphics.PreferredBackBufferWidth = Size[currentSize];
                graphics.PreferredBackBufferHeight = Size[currentSize];
                Settings.ScreenSize = Size[currentSize];
                graphics.ApplyChanges();

                Settings.Scale = currentSize + 2;
                rend = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }
            if (screen == Screens.Home) home.Update(ms);
            if (screen == Screens.Intro) intro.Update(ks, ms, gameTime);
            if (screen == Screens.Continue) continueintro.Update(ks, ms, gameTime);
            if (screen == Screens.Level)
            {
                levellist[level - 1].Update(ks, ms, gameTime);
            }
            if (screen == Screens.GameOver) gameover.Update(ms);
            lastks = ks;

            if (resetRend)
                resetRendInfo();

            if (quitbool)
            {
                quit();
            }

            if (ks.IsKeyDown(Keys.F11))
            {
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }

            ttle.Update();
            healthManager.Update();

            Camera.Rotation += .01f;
            Camera.UpdateView();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            /*
            GraphicsDevice.SetRenderTarget(rend);

            GraphicsDevice.Clear(new Color(208, 159, 81));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            inventory.Draw(spriteBatch, GameContent.descriptionsfont);
            if (screen == Screens.Home) home.Draw(spriteBatch);
            if (screen == Screens.Intro) intro.Draw(spriteBatch);
            if (screen == Screens.Continue) continueintro.Draw(spriteBatch);
            if (screen == Screens.Level)
            {
                levellist[level - 1].Draw(spriteBatch);
            }
            if (screen == Screens.GameOver) gameover.Draw(spriteBatch);
            healthManager.Draw(spriteBatch);
            spriteBatch.DrawString(GameContent.descriptionsfont, string.Format("Width: {0}, Height: {1}", width, height), Vector2.Zero, Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            rendpos = new Vector2(GraphicsDevice.Viewport.Width - rend.Width, GraphicsDevice.Viewport.Height - rend.Height) / 2;

            GraphicsDevice.Clear(rendColor);

            spriteBatch.Begin();
            spriteBatch.Draw(rend, rendpos, null, rendColor, 0f, Vector2.Zero, (float)rendscale, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(GameContent.mouse, new Vector2(ms.X, ms.Y), null, Color.White, 0f, Vector2.Zero, GraphicsDevice.Viewport.Width / 300, SpriteEffects.None, 1);
            spriteBatch.End();*/

            effect.View = Camera.View;
            effect.Projection = Camera.Projection;

            GraphicsDevice.Clear(new Color(208, 159, 81));
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, effect);

            home.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.Draw(GameContent.mouse, new Vector2(ms.X, ms.Y), null, Color.White, 0f, Vector2.Zero, GraphicsDevice.Viewport.Width / 300, SpriteEffects.None, 1);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        void resetRendInfo()
        {
            if (rendColor != Color.White)
            {
                MediaPlayer.Volume += 1f / 255f;
                rendColor = new Color(rendColor.R + 1, rendColor.G + 1, rendColor.B + 1);
            }
            else
            {
                resetRend = false;
            }

            if (toinitialize)
            {
                transitioneffect = false;
                rendpos = Vector2.Zero;
                rendColor = Color.White;
                level = 1;
                levellist = new List<Level>();
                rendscale = 1f;
                name = "";
                screen = Screens.Home;
                rendOffset = 0;
                resetRend = false;
                deathspeed = 1;
                toinitialize = false;
                Size = new List<int>();
                currentSize = 0;
                Initialize();
                resetRend = false;
            }
        }

        public void quit()
        {
            Exit();
        }
    }
}
