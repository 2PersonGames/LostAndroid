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
        }

        public virtual void Update()
        {
            position += velocity;
            UpdateRectangle();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, spriteColour, rotation, Vector2.Zero, scale, SpriteEffects.None, depth);
        }

        protected void UpdateCentre()
        {
            centre = new Vector2(GetRelativeSizeFloat(sprite.Width) / 2, GetRelativeSizeFloat(sprite.Height) / 2);
        }

        protected void UpdateRectangle()
        {
            rect = new Rectangle((int)position.X, (int)position.Y, GetRelativeSizeInt(sprite.Width), GetRelativeSizeInt(sprite.Height));
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

        public void Save(StreamWriter save)
        {
            string writeLine;
            
            //Position
            writeLine = string.Empty;
            writeLine += position.X;
            writeLine += '\t';
            writeLine += position.Y;
            save.WriteLine(writeLine);

            //Sprite Colour
            writeLine = string.Empty;
            writeLine += spriteColour.B.ToString();
            writeLine += '\t';
            writeLine += spriteColour.G.ToString();
            writeLine += '\t';
            writeLine += spriteColour.R.ToString();
            save.WriteLine(writeLine);

            //Gravity
            save.WriteLine(gravity);
        }
    }
}