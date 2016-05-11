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
    class GameObject
    {
        #region Variable declarations
        const float GravityModifier = 9.8f;

        protected Rectangle rect;
        public Rectangle Rect
        {
            get { return rect; }
        }

        protected Vector2 position;
        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }
        protected Vector2 drawPosition;

        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected Vector2 centre;
        public Vector2 Centre
        {
            get { return centre; }
        }

        protected Texture2D sprite;
        public Texture2D Sprite
        {
            get { return sprite; }
        }

        protected float scale;
        public float Scale
        {
            get { return scale; }
        }

        protected float rotation;
        public float Rotation
        {
            set { rotation = value; }
            get { return rotation; }
        }

        protected float depth;
        public float Depth
        {
            get { return depth; }
        }

        protected Color spriteColour;
        public Color SpriteColour
        {
            get { return spriteColour; }
        }

        protected SpriteEffects spriteEffects;

        protected bool gravity;
        public bool Gravity
        {
            get { return gravity; }
        }
        #endregion

        public GameObject(Texture2D nSprite, Vector2 nPosition, float nScale, Color nColor, float drawDepth, float nRotation, bool nGravity)
        {
            sprite = nSprite;
            UpdateRectangle();
            position = nPosition;
            scale = nScale;
            spriteColour = nColor;
            depth = drawDepth;
            rotation = nRotation;
            UpdateCentre();
            velocity = Vector2.Zero;
            gravity = nGravity;
            spriteEffects = SpriteEffects.None;

            drawPosition = Vector2.Zero;
            drawPosition.X = position.X + GetRelativeSizeFloat(sprite.Width / 2f);
            drawPosition.Y = position.Y + GetRelativeSizeFloat(sprite.Height / 2f);
        }

        public virtual void Update()
        {
            position += velocity;
            drawPosition.X = position.X + GetRelativeSizeFloat(sprite.Width / 2f);
            drawPosition.Y = position.Y + GetRelativeSizeFloat(sprite.Height / 2f);
            UpdateRectangle();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawPosition, null, spriteColour, rotation, centre, scale, spriteEffects, depth);
            //spriteBatch.Draw(sprite, rect, null, Color.Yellow, rotation, Vector2.Zero, SpriteEffects.None, depth);
        }

        protected void UpdateCentre()
        {
            centre = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }

        protected virtual void UpdateRectangle()
        {
            rect = new Rectangle((int)Math.Round(position.X),
                                 (int)Math.Round(position.Y), 
                                 GetRelativeSizeInt(sprite.Width), 
                                 GetRelativeSizeInt(sprite.Height));
        }

        protected int GetRelativeSizeInt(float number)
        {
            return (int)Math.Round(GetRelativeSizeFloat(number));
        }

        protected float GetRelativeSizeFloat(float number)
        {
            return number * scale;
        }

        protected void ApplyGravity()
        {
            if (gravity && velocity.Y < GravityModifier)
            {
                velocity.Y += GravityModifier / 1f;
            }
        }

        public void Load(string readLine1, string readLine2, bool nGravity)
        {
            if (readLine1 != string.Empty)
            {
                string outLine = string.Empty;
                for (int i = 0; i < readLine1.Length; i++)
                {
                    if (readLine1[i] == '\t')
                    {
                        if (position.X == 0f)
                        {
                            float.TryParse(outLine, out position.X);
                            outLine = string.Empty;
                        }
                    }
                    else
                    {
                        outLine += readLine1[i];
                    }
                }
                float.TryParse(outLine, out position.Y);
            }

            if (readLine2 != string.Empty)
            {
                string outLine = string.Empty;
                int hexNumber = 0;
                for (int i = 0; i < readLine2.Length; i++)
                {
                    if (readLine2[i] == '\t')
                    {
                        switch (hexNumber)
                        {
                            case 0:
                                {
                                    byte colourHex;
                                    byte.TryParse(outLine, out colourHex);
                                    spriteColour.B = colourHex;
                                    break;
                                }
                            case 1:
                                {
                                    byte colourHex;
                                    byte.TryParse(outLine, out colourHex);
                                    spriteColour.G = colourHex;
                                    break;
                                }
                            case 2:
                                {
                                    byte colourHex;
                                    byte.TryParse(outLine, out colourHex);
                                    spriteColour.R = colourHex;
                                    break;
                                }
                        }
                        outLine = string.Empty;
                        break;
                    }
                    else
                    {
                        outLine += readLine2[i];
                    }
                }
            }

            gravity = nGravity;
        }
    }
}