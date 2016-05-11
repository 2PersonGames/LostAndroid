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
    class CollidableObject : GameObject
    {
        protected float jumpAcceleration;
        protected float jumpCounter;

        public CollidableObject(Texture2D nSprite, Vector2 nPosition, float nScale, Color nColor, float drawDepth, float nRotation, bool nGravity)
            : base(nSprite, nPosition, nScale, nColor, drawDepth, nRotation, nGravity)
        {
            jumpCounter = 0f;
            jumpAcceleration = 0f;
        }

        public virtual void Update(List<CollidableObject> collidables, Rectangle border)
        {
            ApplyGravity();

            CollisionDetection(collidables);
            BorderDetection(border);

            base.Update();
        }

        public virtual void Update(Rectangle border)
        {
            ApplyGravity();

            BorderDetection(border);

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected bool CollisionDetection(List<CollidableObject> collidables)
        {
            bool collided = false;
            if (velocity != Vector2.Zero)
            {
                Rectangle nextPositionX = new Rectangle((int)Math.Round(position.X + velocity.X), (int)Math.Round(position.Y), rect.Width, rect.Height);
                Rectangle nextPositionY = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y + velocity.Y), rect.Width, rect.Height);
                foreach (GameObject collidable in collidables)
                {
                    if (rect != collidable.Rect)
                    {
                        if (collidable.Rect.Intersects(nextPositionX))
                        {
                            //Colliding on the horizontal axis
                            if (velocity.X > 0)
                            {
                                position.X = collidable.Rect.X - rect.Width;
                            }
                            else if (velocity.X < 0)
                            {
                                position.X = collidable.Rect.X + collidable.Rect.Width;
                            }
                            velocity.X = 0;
                            nextPositionX = new Rectangle((int)Math.Round(position.X + velocity.X), (int)Math.Round(position.Y), rect.Width, rect.Height);
                            nextPositionY = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y + velocity.Y), rect.Width, rect.Height);
                            collided = true;
                        }
                        if (collidable.Rect.Intersects(nextPositionY))
                        {
                            //Colliding on the vertical axis
                            if (velocity.Y > 0)
                            {
                                position.Y = collidable.Rect.Y - rect.Height;
                            }
                            else if (velocity.Y < 0)
                            {
                                position.Y = collidable.Rect.Y + collidable.Rect.Height;
                            }
                            velocity.Y = 0;
                            nextPositionX = new Rectangle((int)Math.Round(position.X + velocity.X), (int)Math.Round(position.Y), rect.Width, rect.Height);
                            nextPositionY = new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y + velocity.Y), rect.Width, rect.Height);
                            collided = true;
                        }
                    }
                }
            }
            return collided;
        }

        void BorderDetection(Rectangle border)
        {
            if (position.X + GetRelativeSizeInt(sprite.Width) + velocity.X >= border.Width + border.X)
            {
                position.X = border.Width - GetRelativeSizeInt(sprite.Width) + border.X;
                velocity.X = 0;
            }
            else if (position.X + velocity.X <= border.X)
            {
                position.X = border.X;
                velocity.X = 0;
            }
            if (position.Y + GetRelativeSizeInt(sprite.Height) + velocity.Y >= border.Height + border.Y)
            {
                position.Y = border.Height - GetRelativeSizeInt(sprite.Height) + border.Y;
                velocity.Y = 0;
            }
            else if (position.Y + velocity.Y <= border.Y)
            {
                position.Y = border.Y;
                velocity.Y = 0;
            }
        }

        protected void TestForJump()
        {
            if (jumpAcceleration > 0)
            {
                velocity.Y = -((float)Math.Round(Math.Pow(jumpAcceleration, 2)) / 1900f);
                jumpAcceleration--;
            }
        }
    }
}
