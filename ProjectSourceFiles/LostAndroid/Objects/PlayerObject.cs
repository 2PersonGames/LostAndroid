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

namespace Assignment1a.Objects
{
    class PlayerObject : CollidableObject
    {
        const int AnimationSpeed = 2;
        const int JumpAcceleration = 180;

        Rectangle sourceRect;
        Texture2D runningSprite;
        bool running;
        int animationCounter;

        public PlayerObject(Texture2D nSprite, Texture2D nRunningSprite, Vector2 nPosition, float nScale, Color nColor, float nDrawDepth, float nRotation, bool nGravity)
            : base(nSprite, nPosition, nScale, nColor, nDrawDepth, nRotation, nGravity)
        {
            animationCounter = 0;
            running = false;
            runningSprite = nRunningSprite;
            spriteEffects = SpriteEffects.None;
            sourceRect = new Rectangle(0, 0, (int)Math.Round(runningSprite.Width / 5f), (int)Math.Round(runningSprite.Height / 2f));
        }

        public void Update(List<CollidableObject> collidables, Rectangle border, GamePadState gamePadState, GamePadState previousGamePadState)
        {
            velocity.X = gamePadState.ThumbSticks.Left.X * 5;

#if !XBOX
            if (gamePadState.ThumbSticks.Left.X == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    velocity.X = 10;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    velocity.X = -10;
                }
                else
                {
                    velocity.X = 0;
                }
            }
#endif

            //Check to see for sprite direction
            if (velocity.X > 0)
            {
                spriteEffects = SpriteEffects.None;
            }
            else if (velocity.X < 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            //Check for jump
            if (gamePadState.Buttons.A == ButtonState.Pressed &&
                previousGamePadState.Buttons.A == ButtonState.Released &&
                velocity.Y == 0)
            {
                jumpAcceleration = JumpAcceleration;
            }

#if !XBOX
            if (Keyboard.GetState().IsKeyDown(Keys.Space) &&
                velocity.Y == 0)
            {
                jumpAcceleration = JumpAcceleration;
            }
#endif

            TestForJump();

            base.Update(collidables, border);

            //Check for running
            if (velocity.X != 0 && velocity.Y == 0)
            {
                running = true;
                UpdateAnimation();
            }
            else if (velocity.Y != 0)
            {
                running = true;
                sourceRect.X = sourceRect.Width * 2;
            }
            else
            {
                sourceRect.Y = 0;
                sourceRect.X = 0;
                running = false;
                sourceRect.X = 0;
                sourceRect.Y = 0;
                animationCounter = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (running)
            {
                spriteBatch.Draw(runningSprite, drawPosition, sourceRect, spriteColour, 0f, centre, scale, spriteEffects, depth);
            }
            else
            {
                base.Draw(spriteBatch);
            }
        }

        void UpdateAnimation()
        {
            if (animationCounter == 0)
            {
                sourceRect.X += sourceRect.Width;

                if (sourceRect.X == runningSprite.Width)
                {
                    sourceRect.X = 0;
                    sourceRect.Y += sourceRect.Height;
                }
                if (sourceRect.Y == runningSprite.Height)
                {
                    sourceRect.Y = 0;
                }
            }
            animationCounter++;
            if (animationCounter > AnimationSpeed)
            {
                animationCounter = 0;
            }
        }

        protected override void UpdateRectangle()
        {
            int newWidth = GetRelativeSizeInt(((sprite.Width) / 3f) *2f);
            int newHeight = GetRelativeSizeInt(sprite.Height);
            Point newPosition = new Point((int)Math.Round(position.X + (GetRelativeSizeFloat(sprite.Width) / 3f)), 
                                          (int)Math.Round(position.Y));

            rect = new Rectangle(newPosition.X, newPosition.Y, newWidth, newHeight);
        }
    }
}
