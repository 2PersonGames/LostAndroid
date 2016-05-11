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

namespace LevelEditor
{
    class Editor : GameObject
    {
        public Editor(Texture2D nSprite, Vector2 nPosition, float nScale, Color nColour, float nDrawDepth, float nRotation)
            : base(nSprite, nPosition, nScale, nColour, nDrawDepth, nRotation, false)
        {
        }

        public override void Update()
        {
            velocity.X = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X * 5;
            velocity.Y = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y * 5;

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
