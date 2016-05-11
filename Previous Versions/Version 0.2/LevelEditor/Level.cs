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


namespace LevelEditor
{
    class Level : DrawableGameComponent
    {
        #region Variable declarations
        const int BlockWidth = 50;
        const int BlockHeight = 50;
        const int MaxNumberOfBlocks = 2;
        enum GameState { Running, Paused, Ended }

        Enum gameState;

        GamePadState playerOnePad;
        public GamePadState PlayerOnePad
        {
            set { playerOnePad = value; }
        }
        GamePadState playerOnePadPrevious;

        int mapWidth;
        int mapHeight;
        Rectangle map;
        Rectangle[] borderBlocks;
        Rectangle viewportRect;
        Texture2D[] blocks;

        Rectangle[,] tiledBackground;
        Texture2D background;

        Matrix camera;
        SpriteBatch spriteBatch;
        float scale;

        Editor playerOne;

        List<CollidableObject> collideableBlocks;
        List<GameObject> bonusBlocks;

        CollidableObject start;
        CollidableObject finish;

        Texture2D startSprite;
        Texture2D finishSprite;
        #endregion

        public Level(Game game, Rectangle nViewportRect, int nWidth, int nHeight, float nScale, Rectangle titleSafeAreaRect)
            : base(game)
        {
            mapWidth = nWidth;
            mapHeight = nHeight;

            viewportRect = nViewportRect;
            map = new Rectangle(0, 0, nWidth * BlockWidth, nHeight * BlockHeight);

            //Declare an array of rectangles going around the map edge
            DeclareBorderBlocks(nWidth, nHeight);

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            scale = nScale;
            bonusBlocks = new List<GameObject>();
            collideableBlocks = new List<CollidableObject>();
            blocks = new Texture2D[MaxNumberOfBlocks];

            TileBackground(nWidth, nHeight);

            gameState = GameState.Running;
        }

        protected override void LoadContent()
        {
            for (int i = 0; i < MaxNumberOfBlocks; i++)
            {
                string textureLocation = "Sprites\\Blocks\\block" + i.ToString();
                blocks[i] = Game.Content.Load<Texture2D>(textureLocation);
            }

            background = Game.Content.Load<Texture2D>("Backgrounds\\background0");

            playerOne = new Editor(Game.Content.Load<Texture2D>("Sprites\\Blocks\\block0"), Vector2.Zero, scale, Color.White, 0.9f, 0f);

            startSprite = Game.Content.Load<Texture2D>("Sprites\\Blocks\\door");
            finishSprite = startSprite;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            playerOnePad = GamePad.GetState(PlayerIndex.One);

            playerOne.Update();

            //Controls for saving
            if (playerOnePad.Buttons.Start == ButtonState.Pressed &&
                playerOnePadPrevious.Buttons.Start == ButtonState.Released)
            {
                if (start != null && finish != null)
                {
                    SaveLevel();
                    Game.Exit();
                }
            }
            else if (playerOnePad.Buttons.RightShoulder == ButtonState.Pressed &&
                     playerOnePadPrevious.Buttons.RightShoulder == ButtonState.Released)
            {
                //Controls for adding items
                if (playerOnePad.Buttons.X == ButtonState.Pressed)
                {
                    AddBlock();
                }
                else if (playerOnePad.Buttons.B == ButtonState.Pressed)
                {
                    AddBonus();
                }
                else if (playerOnePad.Buttons.A == ButtonState.Pressed)
                {
                    AddStart();
                }
                else if (playerOnePad.Buttons.Y == ButtonState.Pressed)
                {
                    AddFinish();
                }
            }

            foreach (CollidableObject block in collideableBlocks)
            {
                block.Update(collideableBlocks, map);
            }
            foreach (GameObject block in bonusBlocks)
            {
                block.Update();
            }

            if (start != null)
            {
                start.Update(collideableBlocks, map);
            }
            if (finish != null)
            {
                finish.Update(collideableBlocks, map);
            }

            CameraUpdate();

            playerOnePadPrevious = playerOnePad;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.None, camera);

            //Draw background
            foreach (Rectangle tile in tiledBackground)
            {
                spriteBatch.Draw(background, tile, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            }

            //Draw border
            foreach (Rectangle block in borderBlocks)
            {
                spriteBatch.Draw(blocks[1], block, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            }

            //Draw interactive blocks
            foreach (CollidableObject block in collideableBlocks)
            {
                block.Draw(spriteBatch);
            }

            //Draw bonus blocks
            foreach (GameObject block in bonusBlocks)
            {
                block.Draw(spriteBatch);
            }

            if (playerOnePad.Buttons.B == ButtonState.Pressed)
            {
                spriteBatch.Draw(blocks[1], playerOne.Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
            else if (playerOnePad.Buttons.X == ButtonState.Pressed)
            {
                spriteBatch.Draw(blocks[0], playerOne.Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }
            else if (playerOnePad.Buttons.A == ButtonState.Pressed && start == null)
            {
                spriteBatch.Draw(startSprite, playerOne.Position, null, Color.White, 0f, Vector2.Zero, scale * 0.1f, SpriteEffects.None, 0.8f);
            }
            else if (playerOnePad.Buttons.Y == ButtonState.Pressed && finish == null)
            {
                spriteBatch.Draw(finishSprite, playerOne.Position, null, Color.White, 0f, Vector2.Zero, scale * 0.1f, SpriteEffects.None, 0.8f);
            }

            if (start != null)
            {
                start.Draw(spriteBatch);
            }
            if (finish != null)
            {
                finish.Draw(spriteBatch);
            }

            base.Draw(gameTime);

            spriteBatch.End();
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

        /// <summary>
        /// Draws background tiled
        /// </summary>
        /// <param name="spriteBatch"></param>
        void TileBackground(int nWidth, int nHeight)
        {
            int horizontalTiles = (int)Math.Ceiling(map.Width / (float)viewportRect.Width);
            int verticalTiles = (int)Math.Ceiling(map.Height / (float)viewportRect.Height);

            tiledBackground = new Rectangle[horizontalTiles, verticalTiles];
            for (int i = 0; i < horizontalTiles; i++)
            {
                for (int j = 0; j < verticalTiles; j++)
                {
                    int x = map.X + (viewportRect.Width * i);
                    int y = map.Y + (viewportRect.Height * j);
                    tiledBackground[i, j] = new Rectangle(x, y, viewportRect.Width, viewportRect.Height);
                }
            }
        }

        void CameraUpdate()
        {
            Vector3 cameraPosition = new Vector3(-((playerOne.Position.X) - (viewportRect.Width / 2)), 
                                                 -((playerOne.Position.Y) - (viewportRect.Height / 2)), 0);

            camera = Matrix.CreateTranslation(cameraPosition);
        }

        void SaveLevel()
        {
            StreamWriter save = new StreamWriter("level.txt", false);

            try
            {
                foreach (CollidableObject block in collideableBlocks)
                {
                    block.Save(save);
                }
                save.WriteLine("ENDOFCOLLIDABLEBLOCKS");

                foreach (GameObject block in bonusBlocks)
                {
                    block.Save(save);
                }
                save.WriteLine("ENDOFBONUSBLOCKS");

                //Map size
                save.WriteLine(mapWidth.ToString());
                save.WriteLine(mapHeight.ToString());

                //Start and finish
                start.Save(save);
                finish.Save(save);

                save.Flush();
                save.Close();
            }
            finally
            {
                Game.Exit();
            }
        }

        void AddBlock()
        {
            bool gravity = false;
            if (playerOnePad.Triggers.Left > 0)
            {
                gravity = true;
            }
            CollidableObject newObject = new CollidableObject(blocks[0], playerOne.Position, scale, Color.White, 0.8f, 0f, gravity);

            bool add = true;
            foreach (CollidableObject block in collideableBlocks)
            {
                if (newObject.Rect.Contains(block.Rect) || newObject.Rect.Intersects(block.Rect))
                {
                    add = false;
                    break;
                }
            }
            foreach (GameObject block in bonusBlocks)
            {
                if (newObject.Rect.Contains(block.Rect) || newObject.Rect.Intersects(block.Rect))
                {
                    add = false;
                    break;
                }
            }
            if (add && map.Contains(newObject.Rect))
            {
                collideableBlocks.Add(newObject);
            }
        }

        void AddBonus()
        {
            GameObject newObject = new GameObject(blocks[1], playerOne.Position, scale, Color.White, 0.8f, 0f, false);

            bool add = true;
            foreach (CollidableObject block in collideableBlocks)
            {
                if (newObject.Rect.Contains(block.Rect) || newObject.Rect.Intersects(block.Rect))
                {
                    add = false;
                    break;
                }
            }
            foreach (GameObject block in bonusBlocks)
            {
                if (newObject.Rect.Contains(block.Rect) || newObject.Rect.Intersects(block.Rect))
                {
                    add = false;
                    break;
                }
            }
            if (add && map.Contains(newObject.Rect))
            {
                bonusBlocks.Add(newObject);
            }
        }

        void AddStart()
        {
            if (start == null)
            {
                start = new CollidableObject(startSprite, playerOne.Position, scale * 0.1f, Color.White, 0.8f, 0f, true);
            }
        }

        void AddFinish()
        {
            if (finish == null)
            {
                finish = new CollidableObject(finishSprite, playerOne.Position, scale * 0.1f, Color.White, 0.8f, 0f, true);
            }
        }
    }
}
