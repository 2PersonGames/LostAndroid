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

namespace Assignment1a.Managers
{
    public struct DrawManager
    {
        public static void DrawShadowedText(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color colour,
                                            float rotation, Vector2 centre, float scale, SpriteEffects spriteEffects, float depth)
        {
            spriteBatch.DrawString(font, text, new Vector2(position.X - 1, position.Y - 1), Color.Black, rotation, centre, scale, spriteEffects, depth - 0.0001f);
            spriteBatch.DrawString(font, text, position, colour, rotation, centre, scale, spriteEffects, depth);
        }
    }
}
