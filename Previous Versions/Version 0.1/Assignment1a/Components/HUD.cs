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
using Assignment1a.Objects;

namespace Assignment1a.Components
{
    class HUD : DrawableGameComponent
    {
        #region Variable Declarations

        public GameState gameState;

        float scale;
        SpriteBatch spriteBatch;

        SpriteFont scoreFont;
        Vector2 scorePosition;
        Vector2 scoreCentre;
        string scoreString;

        Vector2 timePosition;
        Vector2 timeCentre;
        string time;

        Vector2 pausedPosition1;
        Vector2 pausedPosition2;
        string pausedString;
        Color pausedColour;

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

        LevelBuilder playerTwo;

        Texture2D backgroundPaused;

        Rectangle viewportRect;
        #endregion

        public HUD(Game game, float nScale, Rectangle nViewportRect, Rectangle titleSafeAreaRect, LevelBuilder nPlayerTwo)
            : base(game)
        {
            scale = nScale;
            viewportRect = nViewportRect;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            gameState = GameState.Running;

            scorePosition = new Vector2(viewportRect.Width / 2f, titleSafeAreaRect.Y * 1.5f);
            scoreCentre = Vector2.Zero;
            scoreString = string.Empty;

            playerTwo = nPlayerTwo;

            pausedPosition1 = new Vector2(viewportRect.Width / 2.2f, viewportRect.Height / 2.4f);
            pausedPosition2 = new Vector2(viewportRect.Width / 2.2f, viewportRect.Height / 2.0f);

            timePosition = new Vector2(viewportRect.Width / 2f, titleSafeAreaRect.Y);
            timeCentre = Vector2.Zero;
            time = string.Empty;
        }

        protected override void LoadContent()
        {
            scoreFont = Game.Content.Load<SpriteFont>("Fonts\\scoreFont");
            backgroundPaused = Game.Content.Load<Texture2D>("Backgrounds\\pausedBackground");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (gameState.Equals(GameState.Running))
            {
                time = ((Game1)Game).GetAlarm();
                timeCentre.X = scoreFont.MeasureString(time).X / 2f;

                scoreString = "";
                int score = ((Game1)Game).Score;
                int padLeft = 11 - score.ToString().Length;

                for (int i = 0; i < padLeft; i++)
                {
                    scoreString += "0";
                }

                scoreString += score.ToString();
                scoreCentre.X = scoreFont.MeasureString(scoreString).X / 2f;

                if (playerOnePad.Buttons.Start == ButtonState.Pressed &&
                    playerOnePadPrevious.Buttons.Start == ButtonState.Released)
                {
                    pausedString = "Player One";
                    pausedColour = Color.Blue;
                    gameState = GameState.Paused;
                }
                else if (playerTwoPad.Buttons.Start == ButtonState.Pressed &&
                         playerTwoPadPrevious.Buttons.Start == ButtonState.Released)
                {
                    pausedString = "Player Two";
                    pausedColour = Color.Red;
                    gameState = GameState.Paused;
                }
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
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            if (gameState.Equals(GameState.Running))
            {
                spriteBatch.DrawString(scoreFont, time, timePosition, Color.Black, 0f, timeCentre, scale, SpriteEffects.None, 1f);
                spriteBatch.DrawString(scoreFont, scoreString, scorePosition, Color.Black, 0f, scoreCentre, scale, SpriteEffects.None, 1f);
                playerTwo.DrawHUD(spriteBatch, scoreFont);
            }
            else if (gameState.Equals(GameState.Paused))
            {
                spriteBatch.Draw(backgroundPaused, viewportRect, Color.White);
                spriteBatch.DrawString(scoreFont, "Paused by", pausedPosition1, Color.Black);
                spriteBatch.DrawString(scoreFont, pausedString, pausedPosition2, pausedColour);
            }

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
