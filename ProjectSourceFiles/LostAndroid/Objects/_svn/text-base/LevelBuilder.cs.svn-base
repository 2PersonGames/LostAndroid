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
using Assignment1a.Managers;

namespace Assignment1a.Objects
{
    class LevelBuilder : CollidableObject
    {
        const int MaxItems = 3;

        PlayerIndex player;
        GamePadState previousGamePadState;
        CollidableObject activeObject;

        int blocks = 99;
        public int Blocks
        {
            get { return blocks; }
            set
            {
                blocks = value;
                if (blocks < 0)
                {
                    blocks = 0;
                }
            }
        }

        Vector2 blockDrawPosition;
        Vector2 blockStringPosition;
        Texture2D blockSprite;

        public LevelBuilder(Texture2D nSprite, Vector2 nPosition, float nScale, Color nColor, float nDrawDepth, float nRotation, 
                            PlayerIndex nPlayer, Rectangle viewportRect, Texture2D nBlockSprite)
            : base(nSprite, nPosition, nScale, nColor, nDrawDepth, nRotation, false)
        {
            player = nPlayer;

            blockSprite = nBlockSprite;
            blockDrawPosition = new Vector2(viewportRect.Width * 0.7f, viewportRect.Height * 0.11f);
            blockStringPosition = new Vector2(blockDrawPosition.X + (blockSprite.Width * 0.5f), viewportRect.Height * 0.1f);
        }

        public void Update(Rectangle border, List<CollidableObject> drawables)
        {
            GamePadState gamePadState = GamePad.GetState(player);

            velocity.X = gamePadState.ThumbSticks.Left.X * 5;
            velocity.Y = -gamePadState.ThumbSticks.Left.Y * 5;

            if (gamePadState.Buttons.A == ButtonState.Pressed &&
                previousGamePadState.Buttons.A == ButtonState.Released)
            {
                if (blocks > 0)
                {
                    activeObject = new CollidableObject(blockSprite, position, scale * 2f, Color.White, 0.9f, 0f, true);
                }
            }
            else if (gamePadState.Buttons.A == ButtonState.Released &&
                     previousGamePadState.Buttons.A == ButtonState.Pressed)
            {
                activeObject = null;
            }
            else if (gamePadState.Triggers.Left > 0 &&
                     previousGamePadState.Triggers.Left == 0)
            {
                DropBlock(drawables);
            }
            else if (gamePadState.Buttons.LeftShoulder == ButtonState.Pressed &&
                     previousGamePadState.Buttons.LeftShoulder == ButtonState.Released)
            {
                //Cycle left on items
            }
            else if (gamePadState.Buttons.RightShoulder == ButtonState.Pressed &&
                     previousGamePadState.Buttons.RightShoulder == ButtonState.Released)
            {
                //Cycle right on items
            }


            if (activeObject != null)
            {
                activeObject.Velocity = velocity;
                activeObject.Update();
            }

            base.Update(border);

            previousGamePadState = gamePadState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (activeObject != null)
            {
                activeObject.Draw(spriteBatch);
            }

            //base.Draw(spriteBatch);
        }

        void DropBlock(List<CollidableObject> drawables)
        {
            if (activeObject != null)
            {
                Point checkPoint = new Point((int)Math.Round(activeObject.Position.X), (int)Math.Round(activeObject.Position.Y));
                bool drop = true;
                foreach (GameObject block in drawables)
                {
                    if (block.Rect.Intersects(activeObject.Rect))
                    {
                        drop = false;
                        break;
                    }
                }
                if (drop)
                {
                    blocks--;
                    activeObject.Velocity = Vector2.Zero;
                    drawables.Add(activeObject);
                    activeObject = null;
                }
            }
        }

        public void DrawHUD(SpriteBatch spriteBatch, SpriteFont font1)
        {
            spriteBatch.Draw(blockSprite, blockDrawPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            DrawManager.DrawShadowedText(spriteBatch, font1, "x" + blocks.ToString(), blockStringPosition, Color.Orange, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }
    }
}
