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
using Assignment1a.Managers;

namespace Assignment1a.Components
{
    class Level : DrawableGameComponent
    {
        #region Variable declarations
        const int BlockWidth = 50;
        const int BlockHeight = 50;
        const bool Debug = false;

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
        Rectangle viewportRect;
        Rectangle[] border;

        Matrix camera;
        SpriteBatch spriteBatch;
        float scale;

        PlayerObject playerOne;
        LevelBuilder playerTwo;

        List<CollidableObject> collideableBlocks;
        List<GameObject> bonusBlocks;
        List<EnemyObject> enemies;

        CollidableObject start;
        CollidableObject finish;

        int levelNumber;

        AlarmObject alarm;
        public AlarmObject Alarm
        {
            get { return alarm; }
        }

        BackgroundObject background;
        HUD hud;

        Vector3 debugCamera;

        bool drawX;
        Vector2 drawXPosition;
        #endregion

        public Level(Game game, Rectangle nViewportRect, float nScale, Rectangle titleSafeAreaRect,
                     LevelBuilder nPlayerTwo, PlayerObject nPlayerOne, int nLevelNumber)
            : base(game)
        {
            hud = new HUD(game, nScale, nViewportRect, titleSafeAreaRect, nPlayerTwo);

            viewportRect = nViewportRect;

            playerOne = nPlayerOne;
            playerTwo = nPlayerTwo;

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            scale = nScale;
            bonusBlocks = new List<GameObject>();
            collideableBlocks = new List<CollidableObject>();
            enemies = new List<EnemyObject>();

            gameState = GameState.Running;

            levelNumber = nLevelNumber;

            alarm = new AlarmObject(120);

            if (Debug)
            {
                debugCamera = new Vector3(0, 0, 0);
            }

            drawX = false;
            drawXPosition = Vector2.Zero;
        }

        protected override void LoadContent()
        {
            start = new CollidableObject(TextureManager.Start, Vector2.Zero,
                                         scale * 0.1f, Color.White, 0.8f, 0f, true);
            finish = new CollidableObject(TextureManager.Finish, Vector2.Zero,
                                          scale * 0.1f, Color.White, 0.8f, 0f, true);

            List<Point> deletedBorderBlocks = new List<Point>();
            Load(levelNumber, deletedBorderBlocks);

            Vector2 playerPosition = Vector2.Zero;
            playerPosition.Y = start.Position.Y + start.Rect.Height - playerOne.Rect.Height;
            playerPosition.X = start.Position.X;
            playerOne.Position = playerPosition;
            collideableBlocks.Add(playerOne);

            map = new Rectangle(0, 0, mapWidth * BlockWidth, mapHeight * BlockHeight);

            Rectangle tileMap = new Rectangle(map.X - BlockWidth, map.Y - BlockHeight, map.Width + (BlockWidth * 2), map.Height + (BlockHeight * 2));
            background = new BackgroundObject(Game, tileMap, scale);

            //Declare an array of rectangles going around the map edge
            DeclareBorderBlocks(mapWidth, mapHeight, deletedBorderBlocks);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (gameState.Equals(GameState.Running))
            {
                if (Debug)
                {
                    debugCamera.X += playerOnePad.ThumbSticks.Right.X * 10f;
                    debugCamera.Y += -playerOnePad.ThumbSticks.Right.Y * 10f;
                }

                background.Update();

                playerOne.Update(collideableBlocks, new Rectangle(map.X, map.Y, map.Width, map.Height + playerOne.Sprite.Height + BlockHeight), 
                                 playerOnePad, playerOnePadPrevious);

                CameraUpdate();

                Rectangle playerTwoBoundary = new Rectangle(-(int)camera.Translation.X, -(int)camera.Translation.Y,
                                                            viewportRect.Width, viewportRect.Height);

                playerTwo.Update(playerTwoBoundary, collideableBlocks);

                UpdateObjects();

                CheckForBonusBlock();

                CheckForEnd(gameTime);

                hud.Update(((Game1)Game).Score, alarm);
            }
            else if (gameState.Equals(GameState.Paused))
            {
                //Updates when the game is paused
            }
            
            if (!gameState.Equals(GameState.Ended) || !gameState.Equals(GameState.Loading))
            {
                //Pause the game
                if (playerOnePad.Buttons.Start == ButtonState.Pressed &&
                    playerOnePadPrevious.Buttons.Start == ButtonState.Released)
                {
                    CheckForPause();
                }
                else if (playerTwoPad.Buttons.Start == ButtonState.Pressed &&
                         playerTwoPadPrevious.Buttons.Start == ButtonState.Released)
                {
                    CheckForPause();
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
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None, camera);

                background.Draw(spriteBatch);

                //Draw border
                foreach (Rectangle block in border)
                {
                    spriteBatch.Draw(TextureManager.Blocks[0], block, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.6f);
                }

                //Draw interactive blocks
                foreach (GameObject block in collideableBlocks)
                {
                    Type i = typeof(BorderBlockObject);
                    Type j = typeof(PlayerObject);
                    if (block.GetType().Equals(i))
                    {
                        BorderBlockObject newBlock = (BorderBlockObject)block;
                        newBlock.Draw(spriteBatch);
                    }
                    else if (!block.GetType().Equals(j))
                    {
                        block.Draw(spriteBatch);
                    }
                }

                //Draw bonus blocks
                foreach (GameObject block in bonusBlocks)
                {
                    block.Draw(spriteBatch);
                }

                //Draws all enemies
                foreach (EnemyObject enemy in enemies)
                {
                    enemy.Draw(spriteBatch);
                }

                //Draw start and finish
                start.Draw(spriteBatch);
                finish.Draw(spriteBatch);

                //Draw hints
                DrawHints();

                //Draw players
                playerOne.Draw(spriteBatch);
                playerTwo.Draw(spriteBatch);

                base.Draw(gameTime);

                spriteBatch.End();
                hud.Draw();
            }
        }

        /// <summary>
        /// Creates a 1D array of rectangles around the edge of the map
        /// </summary>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        void DeclareBorderBlocks(int nWidth, int nHeight, List<Point> deletedBorderBlocks)
        {
            border = new Rectangle[nWidth + (2 * nHeight) + 2];
            Point blockPoint;

            blockPoint = new Point(map.X - BlockWidth, map.Y - BlockHeight);
            int j = 0;
            for (int i = 0; i < nWidth + 2; i++, j++)
            {
                blockPoint.X = (i - 1) * BlockWidth;
                border[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
                //collideableBlocks.Add(new BorderBlockObject(blocks[0], new Vector2(blockPoint.X, blockPoint.Y), Color.White, 0.2f, BlockWidth, BlockHeight));
            }
            blockPoint = new Point(map.X - BlockWidth, map.Height);
            for (int i = 0; i < nWidth + 2; i++)
            {
                blockPoint.X = (i - 1) * BlockWidth;
                //border[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
                CollidableObject block = new BorderBlockObject(TextureManager.Blocks[0], new Vector2(blockPoint.X, blockPoint.Y), Color.White, 0.6f, BlockWidth, BlockHeight);
                bool add = true;
                foreach (Point point in deletedBorderBlocks)
                {
                    if (block.Rect.Contains(point))
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                {
                    collideableBlocks.Add(block);
                }
            }

            blockPoint = new Point(map.X - BlockWidth, map.Y);
            for (int i = 0; i < nHeight; i++, j++)
            {
                blockPoint.Y = i * BlockHeight;
                border[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
                //collideableBlocks.Add(new BorderBlockObject(blocks[0], new Vector2(blockPoint.X, blockPoint.Y), Color.White, 0.2f, BlockWidth, BlockHeight));
            }
            blockPoint.X = map.X + map.Width;
            for (int i = 0; i < nHeight; i++, j++)
            {
                blockPoint.Y = i * BlockHeight;
                border[j] = new Rectangle(blockPoint.X, blockPoint.Y, BlockWidth, BlockHeight);
                //collideableBlocks.Add(new BorderBlockObject(blocks[0], new Vector2(blockPoint.X, blockPoint.Y), Color.White, 0.2f, BlockWidth, BlockHeight));
            }
        }

        void CameraUpdate()
        {
            if (!Debug)
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
            else
            {
                camera = Matrix.CreateTranslation(-debugCamera);
            }
        }

        void Load(int levelNumber, List<Point> deletedBorderBlocks)
        {
            try
            {
                StreamReader load = new StreamReader("Content\\Levels\\level" + levelNumber.ToString() + ".txt");
                
                string readLine = load.ReadLine();
                while (readLine != "ENDOFCOLLIDABLEBLOCKS")
                {
                    CollidableObject loadedCollideableObject = new CollidableObject(TextureManager.Blocks[0], Vector2.Zero, scale, Color.White, 0.8f, 0f, true);
                    string readLine1 = readLine;
                    string readLine2 = load.ReadLine();
                    bool tempBool;
                    tempBool = bool.Parse(load.ReadLine());
                    loadedCollideableObject.Load(readLine1, readLine2, tempBool);
                    collideableBlocks.Add(loadedCollideableObject);
                    readLine = load.ReadLine();
                }

                readLine = load.ReadLine();
                while (readLine != "ENDOFBONUSBLOCKS")
                {
                    GameObject loadedObject = new GameObject(TextureManager.Blocks[1], Vector2.Zero, scale, Color.White, 0.8f, 0f, false);
                    string readLine1 = readLine;
                    string readLine2 = load.ReadLine();
                    bool tempBool = false;
                    tempBool = bool.Parse(load.ReadLine());
                    loadedObject.Load(readLine1, readLine2, tempBool);
                    bonusBlocks.Add(loadedObject);
                    readLine = load.ReadLine();
                }

                readLine = load.ReadLine();
                while (readLine != "ENDOFENEMIES")
                {
                    EnemyObject loadedObject = new EnemyObject(TextureManager.Enemy, Vector2.Zero, scale, Color.Black, 0.7f, 0f);
                    string readLine1 = readLine;
                    string readLine2 = load.ReadLine();
                    bool tempBool = false;
                    tempBool = bool.Parse(load.ReadLine());
                    loadedObject.Load(readLine1, readLine2, tempBool);
                    enemies.Add(loadedObject);
                    readLine = load.ReadLine();
                }

                readLine = load.ReadLine();
                while (readLine != "ENDOFDELETEDBORDERBLOCKS")
                {
                    Point deletePoint = new Point(0, 0);
                    string point = string.Empty;
                    for (int i = 0; i < readLine.Length; i++)
                    {
                        if (readLine[i] != '\t')
                        {
                            point += readLine[i];
                        }
                        else
                        {
                            if (deletePoint.X == 0)
                            {
                                deletePoint.X = int.Parse(point);
                                point = string.Empty;
                            }
                        }
                    }
                    if (deletePoint.Y == 0)
                    {
                        deletePoint.Y = int.Parse(point);
                    }
                    deletedBorderBlocks.Add(deletePoint);
                    readLine = load.ReadLine();
                }

                //Level information
                mapWidth = int.Parse(load.ReadLine());
                mapHeight = int.Parse(load.ReadLine());

                //Start and finish
                bool gravityTempBool = true;
                string posReadLine1 = string.Empty;
                string posReadLine2 = string.Empty;

                posReadLine1 = load.ReadLine();
                posReadLine2 = load.ReadLine();
                gravityTempBool = bool.Parse(load.ReadLine());
                start.Load(posReadLine1, posReadLine2, gravityTempBool);
                start.Update();

                posReadLine1 = load.ReadLine();
                posReadLine2 = load.ReadLine();
                gravityTempBool = bool.Parse(load.ReadLine());
                finish.Load(posReadLine1, posReadLine2, gravityTempBool);
                finish.Update();

                load.Close();
            }
            catch (Exception e)
            {
                //XBOX 360 will not allow streamwriter access 

#if !XBOX360
                string fileName = "errorLog";
                fileName += DateTime.Now.Day.ToString();
                fileName += "-";
                fileName += DateTime.Now.Month.ToString();
                fileName += "-";
                fileName += DateTime.Now.Year.ToString();
                fileName += ".txt";

                StreamWriter errorLog = new StreamWriter("Content\\" + fileName, false);
                errorLog.WriteLine(e.ToString());
                errorLog.WriteLine();
                errorLog.WriteLine(DateTime.Now.ToString());
                errorLog.Flush();
                errorLog.Close();
#endif

                Game.Exit();
            }
        }

        void CheckForEnd(GameTime gameTime)
        {
            if (alarm.Update(gameTime))
            {
                gameState = GameState.Ended;
                ((Game1)Game).StartAgain();
                drawX = false;
            }
            else if (playerOne.Rect.Intersects(finish.Rect))
            {
                drawXPosition.X = playerOne.Position.X + playerOne.Rect.Width;
                drawXPosition.Y = playerOne.Position.Y;
                drawX = true;

                if (playerOnePad.Buttons.X == ButtonState.Pressed &&
                    playerOnePadPrevious.Buttons.X == ButtonState.Released)
                {
                    ((Game1)Game).GoUpALevel();
                }
            }
            else if (playerOne.Rect.Intersects(start.Rect))
            {
                drawXPosition.X = playerOne.Position.X + playerOne.Rect.Width;
                drawXPosition.Y = playerOne.Position.Y;
                drawX = true;

                if (playerOnePad.Buttons.X == ButtonState.Pressed &&
                    playerOnePadPrevious.Buttons.X == ButtonState.Released)
                {
                    ((Game1)Game).GoDownALevel();
                }
            }
            else
            {
                drawX = false;
            }

            foreach (EnemyObject enemy in enemies)
            {
                if (playerOne.Rect.Intersects(enemy.Rect))
                {
                    StartAgain();
                    break;
                }
            }

            CheckPlayerOutsideBounds();
        }

        /// <summary>
        /// Call this method when the player is sent back to the start of the game
        /// </summary>
        void StartAgain()
        {
            Vector2 playerPosition = Vector2.Zero;
            playerPosition.Y = start.Position.Y + start.Rect.Height - playerOne.Rect.Height;
            playerPosition.X = start.Position.X;
            playerOne.Position = playerPosition;
        }

        /// <summary>
        /// Check to see if the player has left the map, if so they are repositioned to the start
        /// </summary>
        void CheckPlayerOutsideBounds()
        {
            Point playerPosition = new Point((int)Math.Ceiling(playerOne.Position.X), (int)Math.Ceiling(playerOne.Position.Y));

            if (!map.Contains(playerPosition))
            {
                StartAgain();
            }
        }

        void UpdateObjects()
        {
            //Update all blocks in game
            foreach (CollidableObject block in collideableBlocks)
            {
                Type i = typeof(BorderBlockObject);
                Type j = typeof(PlayerObject);
                if (!block.GetType().Equals(i) && !block.GetType().Equals(j))
                {
                    block.Update(collideableBlocks, map);
                }
            }

            //Update all enemies
            foreach (EnemyObject enemy in enemies)
            {
                enemy.Update(collideableBlocks, playerOne.Position);
            }
        }

        void CheckForPause()
        {
            if (gameState == GameState.Paused)
            {
                gameState = GameState.Running;
                hud.Unpause();
            }
            else if (gameState == GameState.Running)
            {
                gameState = GameState.Paused;
                hud.Pause(playerOnePad, playerTwoPad);
            }
        }

        void CheckForBonusBlock()
        {
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
        }

        void DrawHints()
        {
            if (drawX)
            {
                spriteBatch.Draw(TextureManager.ButtonX, drawXPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
        }
    }
}
