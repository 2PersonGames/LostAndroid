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

namespace Assignment1a.Objects
{
    class BorderBlockObject : CollidableObject
    {
        public BorderBlockObject(Texture2D nSprite, Vector2 nPosition, Color nColour, float drawDepth, int width, int height)
            : base(nSprite, nPosition, 1f, nColour, drawDepth, 0f, false)
        {
            rect = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public override void Update()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, rect, null, spriteColour, rotation, Vector2.Zero, SpriteEffects.None, depth);
        }
    }
}
