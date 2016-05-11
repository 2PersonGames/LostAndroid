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

namespace LevelEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState { Input, Running }
        GameState gameState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Level level;

        string mapWidth;
        string mapHeight;
        bool mapWidthDone = false;
        SpriteFont drawFont;

        KeyboardState currentKeyboard;
        KeyboardState previousKeyboard;
        Keys[] keyNumbers;

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
            SetResolution(1280, 720);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            gameState = GameState.Input;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            drawFont = Content.Load<SpriteFont>("font1");

            mapWidth = string.Empty;
            mapHeight = string.Empty;
            keyNumbers = new Keys[10] {Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4,
                                       Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9};

            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (gameState.Equals(GameState.Input))
            {
                //Get input from keyboard
                currentKeyboard = Keyboard.GetState();

                if (!mapWidthDone)
                {
                    //get map width from user
                    foreach (Keys key in keyNumbers)
                    {
                        if (currentKeyboard.IsKeyDown(key) &&
                            previousKeyboard.IsKeyUp(key))
                        {
                            mapWidth += key.ToString()[6];
                        }
                    }
                    if (currentKeyboard.IsKeyDown(Keys.Enter) &&
                        previousKeyboard.IsKeyUp(Keys.Enter))
                    {
                        mapWidthDone = true;
                    }
                }
                else
                {
                    //get map height from user
                    foreach (Keys key in keyNumbers)
                    {
                        if (currentKeyboard.IsKeyDown(key) &&
                            previousKeyboard.IsKeyUp(key))
                        {
                            mapHeight += key.ToString()[6];
                        }
                    }
                    if (currentKeyboard.IsKeyDown(Keys.Enter) &&
                        previousKeyboard.IsKeyUp(Keys.Enter))
                    {
                        level = new Level(this, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), int.Parse(mapWidth), int.Parse(mapHeight), 1280 / 1920f,
                                    new Rectangle((int)(graphics.PreferredBackBufferWidth * 0.1), (int)(graphics.PreferredBackBufferHeight * 0.1),
                                                  (int)(graphics.PreferredBackBufferWidth * 0.8), (int)(graphics.PreferredBackBufferHeight * 0.8)));
                        level.Initialize();
                        Components.Add(level);
                        gameState = GameState.Running;
                    }
                }

                previousKeyboard = currentKeyboard;                
            }
            else if (gameState.Equals(GameState.Running))
            {
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (gameState.Equals(GameState.Input))
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(drawFont, "Width: " + mapWidth.ToString(), new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(drawFont, "Height: " + mapHeight.ToString(), new Vector2(0, 100), Color.White);
                spriteBatch.End();
            }
            else if (gameState.Equals(GameState.Running))
            {
                base.Draw(gameTime);
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
            }
        }
    }
}
