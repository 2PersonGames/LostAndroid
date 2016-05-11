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
        public PlayerObject(Texture2D nSprite, Vector2 nPosition, float nScale, Color nColor, float nDrawDepth, float nRotation, bool nGravity)
            : base(nSprite, nPosition, nScale, nColor, nDrawDepth, nRotation, nGravity)
        {
            spriteEffects = SpriteEffects.None;
        }

        public void Update(List<CollidableObject> collidables, Rectangle border, GamePadState gamePadState, GamePadState previousGamePadState)
        {
            velocity.X = gamePadState.ThumbSticks.Left.X * 5;

            if (velocity.X > 0)
            {
                spriteEffects = SpriteEffects.None;
            }
            else if (velocity.X < 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            if (gamePadState.Buttons.A == ButtonState.Pressed &&
                previousGamePadState.Buttons.A == ButtonState.Released &&
                velocity.Y == 0)
            {
                jumpAcceleration = 180;
            }

            TestForJump();

            base.Update(collidables, border);
        }
    }
}
