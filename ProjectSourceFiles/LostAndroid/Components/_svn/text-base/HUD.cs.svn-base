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
using Assignment1a.Managers;

namespace Assignment1a.Components
{
    class HUD
    {
        #region Variable Declarations

        public GameState gameState;

        float scale;
        SpriteBatch spriteBatch;

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

        LevelBuilder playerTwo;

        Texture2D backgroundPaused;

        Rectangle viewportRect;
        #endregion

        public HUD(Game game, float nScale, Rectangle nViewportRect, Rectangle titleSafeAreaRect, LevelBuilder nPlayerTwo)
        {
            scale = nScale;
            viewportRect = nViewportRect;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
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

        public void Update(int score, AlarmObject alarm)
        {
            if (gameState.Equals(GameState.Running))
            {
                time = alarm.ToString();
                timeCentre.X = FontManager.Font1.MeasureString(time).X / 2f;

                scoreString = "";
                int padLeft = 11 - score.ToString().Length;

                for (int i = 0; i < padLeft; i++)
                {
                    scoreString += "0";
                }

                scoreString += score.ToString();
                scoreCentre.X = FontManager.Font1.MeasureString(scoreString).X / 2f;
            }
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            if (gameState.Equals(GameState.Running))
            {
                DrawManager.DrawShadowedText(spriteBatch, FontManager.Font1, time, timePosition, Color.Orange, 0f, timeCentre, scale, SpriteEffects.None, 1f);
                DrawManager.DrawShadowedText(spriteBatch, FontManager.Font1, scoreString, scorePosition, Color.Orange, 0f, scoreCentre, scale, SpriteEffects.None, 1f);
                playerTwo.DrawHUD(spriteBatch, FontManager.Font1);
            }
            else if (gameState.Equals(GameState.Paused))
            {
                spriteBatch.Draw(TextureManager.PausedBackground, viewportRect, Color.White);
                DrawManager.DrawShadowedText(spriteBatch, FontManager.Font1, "Paused by", pausedPosition1, Color.DarkGray, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f); 
                DrawManager.DrawShadowedText(spriteBatch, FontManager.Font1, pausedString, pausedPosition2, pausedColour, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
            }

            spriteBatch.End();
        }

        public void Pause(GamePadState playerOnePad, GamePadState playerTwoPad)
        {
            gameState = GameState.Paused;

            if (playerOnePad.Buttons.Start == ButtonState.Pressed)
            {
                pausedString = "Player One";
                pausedColour = Color.Blue;
            }
            else if (playerTwoPad.Buttons.Start == ButtonState.Pressed)
            {
                pausedString = "Player Two";
                pausedColour = Color.Red;
            }

#if !XBOX360
            pausedString = "DEBUG";
            pausedColour = Color.Purple;
#endif
        }

        public void Unpause()
        {
            gameState = GameState.Running;
        }
    }
}
