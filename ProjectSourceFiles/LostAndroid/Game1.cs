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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Assignment1a.Components;
using Assignment1a.Objects;
using Assignment1a.Managers;

namespace Assignment1a
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Variable Declarations
        GamePadState playerOnePad;
        GamePadState playerOnePadPrevious;
        GamePadState playerTwoPad;
        GamePadState playerTwoPadPrevious;

        GameState gameState;

        const int NumberOfLevels = 1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;

        Rectangle viewportRect;
        Rectangle titleSafeAreaRect;
        float scale;

        Level[] levels;
        int activeLevel;

        PlayerObject playerOne;
        LevelBuilder playerTwo;

        int score;
        public int Score
        {
            get { return score; }
            set 
            { 
                score = value;
                if (score < 0) 
                { 
                    score = 0; 
                } 
            }
        }
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            SetResolution(1280, 720);
            score = 0;
            activeLevel = 0;
            gameState = GameState.Running;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all textures required
            TextureManager.Load(Content);
            FontManager.Load(Content);
            SoundManager.Load(Content);

            playerOne = new PlayerObject(TextureManager.PlayerStill, TextureManager.PlayerAnimated, Vector2.Zero, scale, Color.White, 1f, 0f, true);
            playerTwo = new LevelBuilder(TextureManager.PlayerTwo, playerOne.Position, scale, Color.Blue, 1f, 0f, PlayerIndex.Two, viewportRect,
                                         TextureManager.Blocks[0]);

            levels = new Level[NumberOfLevels];
            for (int i = 0; i < NumberOfLevels; i++)
            {
                levels[i] = new Level(this, viewportRect, scale, titleSafeAreaRect, playerTwo, playerOne, i);
            }

            //Instantiate tutorial level
            levels[0].Initialize();
            Components.Add(levels[0]);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            playerOnePad = GamePad.GetState(PlayerIndex.One);
            playerTwoPad = GamePad.GetState(PlayerIndex.Two);

            if (gameState.Equals(GameState.Loading))
            {
                if (playerOnePad.Buttons.A == ButtonState.Pressed ||
                    playerTwoPad.Buttons.A == ButtonState.Pressed)
                {
                    gameState = GameState.Running;
                }
            }
            else if (gameState.Equals(GameState.Ended))
            {
                //Ending update
            }
            else if (gameState.Equals(GameState.Running))
            {
                CheckForHUDUpdate();

                base.Update(gameTime);
            }

            playerOnePadPrevious = playerOnePad;
            playerTwoPadPrevious = playerTwoPad;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (gameState.Equals(GameState.Running))
            {
                base.Draw(gameTime);
            }
            else
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                spriteBatch.Draw(background, viewportRect, Color.White);
                spriteBatch.End();
            }
        }

        void SetResolution(int newWidth, int newHeight)
        {
            if (graphics.GraphicsDevice.GraphicsDeviceCapabilities.MaxTextureWidth >= newWidth
                && graphics.GraphicsDevice.GraphicsDeviceCapabilities.MaxTextureHeight >= newHeight)
            {
                graphics.PreferredBackBufferHeight = newHeight;
                graphics.PreferredBackBufferWidth = newWidth;
                graphics.ApplyChanges();

                viewportRect = new Rectangle(0, 0, newWidth, newHeight);
                titleSafeAreaRect = new Rectangle((int)Math.Ceiling(newWidth * 0.1f), (int)Math.Ceiling(newHeight * 0.1f),
                                                  (int)Math.Ceiling(newWidth * 0.8f), (int)Math.Ceiling(newHeight * 0.8f));
                scale = (float)(viewportRect.Width / 1920f);
            }
        }

        void CheckForHUDUpdate()
        {
            levels[activeLevel].PlayerOnePad = playerOnePad;
            levels[activeLevel].PlayerTwoPad = playerTwoPad;
        }

        public void GoUpALevel()
        {
            if (activeLevel < levels.Length - 1)
            {
                levels[activeLevel].Enabled = false;
                levels[activeLevel].Visible = false;

                activeLevel++;

                levels[activeLevel].Enabled = true;
                levels[activeLevel].Visible = true;
            }
        }

        public void GoDownALevel()
        {
            if (activeLevel > 0)
            {
                levels[activeLevel].Enabled = false;
                levels[activeLevel].Visible = false;

                activeLevel--;

                levels[activeLevel].Enabled = true;
                levels[activeLevel].Visible = true;
            }
        }

        public void StartAgain()
        {
            levels[activeLevel].Enabled = false;
            levels[activeLevel].Visible = false;

            activeLevel = 0;

            levels[activeLevel].Enabled = true;
            levels[activeLevel].Visible = true;
        }
    }
}
