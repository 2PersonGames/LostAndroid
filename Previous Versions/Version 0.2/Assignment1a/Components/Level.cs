using System;
using System.IO;
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
using Assignment1a.Objects;

namespace Assignment1a.Components
{
    class Level : DrawableGameComponent
    {
        #region Variable declarations
        const int BlockWidth = 50;
        const int BlockHeight = 50;
        const int MaxNumberOfBlocks = 2;

        GameState gameState;
        public GameState LevelState
        {
            get { return gameState; }
        }

        GamePadState playerOnePad;
        public GamePadState PlayerOnePad
        {
            set { playerOnePad = value; }
        }
        GamePadState playerOnePadPrevious;
        GamePadState playerTwoPad;
        public GamePadState PlayerTwoPad
        {
            set { playerTwoPad = value; }
        }
        GamePadState playerTwoPadPrevious;

        int mapWidth;
        int mapHeight;
        Rectangle map;
        Rectangle[] borderBlocks;
        Rectangle viewportRect;
        Texture2D[] blocks;

        Matrix camera;
        SpriteBatch spriteBatch;
        float scale;

        PlayerObject playerOne;
        LevelBuilder playerTwo;

        List<CollidableObject> collideableBlocks;
        List<GameObject> bonusBlocks;

        CollidableObject start;
        CollidableObject finish;

        int levelNumber;

        AlarmObject alarm;
        public AlarmObject Alarm
        {
            get { return alarm; }
        }

        BackgroundObject background;
        #endregion

        public Level(Game game, Rectangle nViewportRect, float nScale, Rectangle titleSafeAreaRect,
                     LevelBuilder nPlayerTwo, PlayerObject nPlayerOne, int nLevelNumber)
            : base(game)
        {
            viewportRect = nViewportRect;

            playerOne = nPlayerOne;
            playerTwo = nPlayerTwo;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            scale = nScale;
            bonusBlocks = new List<GameObject>();
            collideableBlocks = new List<CollidableObject>();
            blocks = new Texture2D[MaxNumberOfBlocks];

            gameState = GameState.Running;

            levelNumber = nLevelNumber;

            alarm = new AlarmObject(120);
        }

        protected override void LoadContent()
        {
            for (int i = 0; i < MaxNumberOfBlocks; i++)
            {
                string textureLocation = "Sprites\\Blocks\\block" + i.ToString();
                blocks[i] = Game.Content.Load<Texture2D>(textureLocation);
            }

            start = new CollidableObject(Game.Content.Load<Texture2D>("Sprites\\Blocks\\door"), Vector2.Zero,
                                         scale * 0.1f, Color.White, 0.8f, 0f, true);
            finish = new CollidableObject(Game.Content.Load<Texture2D>("Sprites\\Blocks\\door"), Vector2.Zero,
                                          scale * 0.1f, Color.White, 0.8f, 0f, true);

            Load(levelNumber);

            Vector2 playerPosition = Vector2.Zero;
            playerPosition.Y = start.Position.Y + start.Rect.Height - playerOne.Rect.Height;
            playerPosition.X = start.Position.X;
            playerOne.Position = playerPosition;
            collideableBlocks.Add(playerOne);

            map = new Rectangle(0, 0, mapWidth * BlockWidth, mapHeight * BlockHeight);

            Rectangle tileMap = new Rectangle(map.X - BlockWidth, map.Y - BlockHeight, map.Width + (BlockWidth * 2), map.Height + (BlockHeight * 2));
            background = new BackgroundObject(Game, tileMap, scale);

            //Declare an array of rectangles going around the map edge
            DeclareBorderBlocks(mapWidth, mapHeight);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (gameState.Equals(GameState.Running))
            {
                background.Update();

                playerOne.Update(collideableBlocks, map, playerOnePad, playerOnePadPrevious);
                CameraUpdate();
                Rectangle playerTwoBoundary = new Rectangle(-(int)camera.Translation.X, -(int)camera.Translation.Y,
                                                            viewportRect.Width, viewportRect.Height);
                playerTwo.Update(playerTwoBoundary, collideableBlocks, borderBlocks);

                //Update all blocks in game
                foreach (CollidableObject block in collideableBlocks)
                {
                    if (!block.Equals(playerOne))
                    {
                        block.Update(collideableBlocks, map);
                    }
                }

                //Pause the game
                if (playerOnePad.Buttons.Start == ButtonState.Pressed &&
                    playerOnePadPrevious.Buttons.Start == ButtonState.Released)
                {
                    gameState = GameState.Paused;
                }
                else if (playerTwoPad.Buttons.Start == ButtonState.Pressed &&
                         playerTwoPadPrevious.Buttons.Start == ButtonState.Released)
                {
                    gameState = GameState.Paused;
                }

                //Check to see if the player has hit a bonus block
                for (int i = 0; i < bonusBlocks.Count; i++)
                {
                    bonusBlocks[i].Rotation += 0.1f;
                    bonusBlocks[i].Update();
                    if (playerOne.Rect.Intersects(bonusBlocks[i].Rect))
                    {
                        bonusBlocks.Remove(bonusBlocks[i]);
                        playerTwo.Blocks++;
                    }
                }

                CheckForEnd(gameTime);
            }
            else if (gameState.Equals(GameState.Paused))
            {
                if (playerOnePad.Buttons.Start == ButtonState.Pressed &&
                    playerOnePadPrevious.Buttons.Start == ButtonState.Released ||
                    playerTwoPad.Buttons.Start == ButtonState.Pressed &&
                    playerTwoPadPrevious.Buttons.Start == ButtonState.Released)
                {
                    gameState = GameState.Running;
                }
            }

            base.Update(gameTime);

            playerOnePadPrevious = playerOnePad;
            playerTwoPadPrevious = playerTwoPad;
        }

        public override void Draw(GameTime gameTime)
        {
            if (gameState.Equals(GameState.Ended))
            {
            }
            else
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, camera);

                background.Draw(spriteBatch);

                //Draw border
                foreach (Rectangle block in borderBlocks)
                {
                    spriteBatch.Draw(blocks[1], block, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                }

                //Draw interactive blocks
                foreach (GameObject block in collideableBlocks)
                {
                    block.Draw(spriteBatch);
                }

                //Draw bonus blocks
                foreach (GameObject block in bonusBlocks)
                {
                    block.Draw(spriteBatch);
                }

                //Draw start and finish
                start.Draw(spriteBatch);
                finish.Draw(spriteBatch);

                //Draw players
                playerOne.Draw(spriteBatch);
                playerTwo.Draw(spriteBatch);

                base.Draw(gameTime);

                spriteBatch.End();
            }
        }

        /// <summary>
        /// Creates a 1D array of rectangles around the edge of the map
        /// </summary>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        void DeclareBorderBlocks(int nWidth, int nHeight)
        {
            borderBlocks = new Rectangle[2 * (nWidth + nHeight + 2)];
            Point blockPoint;

            blockPoint = new Point(map.X - BlockWidth, map.Y - BlockHeight);
            int j = 0;
            for (int i = 0; i < nWidth + 2; i++, j++)
            {
                blockPoint.X = (i - 1) * BlockWidth;
                borderBlocks[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
            }
            blockPoint = new Point(map.X - BlockWidth, map.Height);
            for (int i = 0; i < nWidth + 2; i++, j++)
            {
                blockPoint.X = (i - 1) * BlockWidth;
                borderBlocks[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
            }

            blockPoint = new Point(map.X - BlockWidth, map.Y);
            for (int i = 0; i < nHeight; i++, j++)
            {
                blockPoint.Y = i * BlockHeight;
                borderBlocks[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
            }
            blockPoint.X = map.X + map.Width;
            for (int i = 0; i < nHeight; i++, j++)
            {
                blockPoint.Y = i * BlockHeight;
                borderBlocks[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
            }
        }

        void CameraUpdate()
        {
            Vector3 cameraPosition = new Vector3(-((playerOne.Position.X + playerOne.Centre.X) - (viewportRect.Width / 2)), 
                                                 -((playerOne.Position.Y + playerOne.Centre.Y) - (viewportRect.Height / 2)), 0);

            float mapWidthBorderLeft = BlockWidth;
            float mapWidthBorderRight = -(map.Width + BlockWidth - (viewportRect.Width));
            cameraPosition.X = MathHelper.Clamp(cameraPosition.X, mapWidthBorderRight, mapWidthBorderLeft);

            float mapHeightBorderTop = BlockHeight;
            float mapHeightBorderBottom = -(map.Height + BlockHeight - (viewportRect.Height));
            cameraPosition.Y = MathHelper.Clamp(cameraPosition.Y, mapHeightBorderBottom, mapHeightBorderTop);

            camera = Matrix.CreateTranslation(cameraPosition);
        }

        void Load(int levelNumber)
        {
            try
            {
                StreamReader load = new StreamReader("Levels\\level" + levelNumber.ToString() + ".txt");
                string readLine = load.ReadLine();

                while (readLine != "ENDOFCOLLIDABLEBLOCKS")
                {
                    CollidableObject loadedCollideableObject = new CollidableObject(blocks[0], Vector2.Zero, scale, Color.White, 0.8f, 0f, true);
                    string readLine1 = readLine;
                    string readLine2 = load.ReadLine();
                    bool tempBool;
                    bool.TryParse(load.ReadLine(), out tempBool);
                    loadedCollideableObject.Load(readLine1, readLine2, tempBool);
                    collideableBlocks.Add(loadedCollideableObject);
                    readLine = load.ReadLine();
                }

                readLine = load.ReadLine();

                while (readLine != "ENDOFBONUSBLOCKS")
                {
                    GameObject loadedObject = new GameObject(blocks[1], Vector2.Zero, scale, Color.White, 0.8f, 0f, false);
                    string readLine1 = readLine;
                    string readLine2 = load.ReadLine();
                    bool tempBool = false;
                    bool.TryParse(load.ReadLine(), out tempBool);
                    loadedObject.Load(readLine1, readLine2, tempBool);
                    bonusBlocks.Add(loadedObject);
                    readLine = load.ReadLine();
                }

                //Level information
                int.TryParse(load.ReadLine(), out mapWidth);
                int.TryParse(load.ReadLine(), out mapHeight);

                //Start and finish
                bool gravityTempBool = true;
                string posReadLine1 = string.Empty;
                string posReadLine2 = string.Empty;

                posReadLine1 = load.ReadLine();
                posReadLine2 = load.ReadLine();
                bool.TryParse(load.ReadLine(), out gravityTempBool);
                start.Load(posReadLine1, posReadLine2, gravityTempBool);
                start.Update();

                posReadLine1 = load.ReadLine();
                posReadLine2 = load.ReadLine();
                bool.TryParse(load.ReadLine(), out gravityTempBool);
                finish.Load(posReadLine1, posReadLine2, gravityTempBool);
                finish.Update();

                load.Close();
            }
            catch (Exception e)
            {
                string fileName = "errorLog";
                fileName += DateTime.Now.Day.ToString();
                fileName += "-";
                fileName += DateTime.Now.Month.ToString();
                fileName += "-";
                fileName += DateTime.Now.Year.ToString();
                fileName += ".txt";

                StreamWriter errorLog = new StreamWriter(fileName, false);
                errorLog.WriteLine(e.ToString());
                errorLog.WriteLine();
                errorLog.WriteLine(DateTime.Now.ToString());
                errorLog.Flush();
                errorLog.Close();
            }
        }

        void CheckForEnd(GameTime gameTime)
        {
            if (playerOne.Rect.Intersects(finish.Rect) || alarm.Update(gameTime))
            {
                gameState = GameState.Ended;
            }
        }

        /// <summary>
        /// Call this method when the player is sent back to the start of the game
        /// </summary>
        void StartAgain()
        {
        }
    }
}
