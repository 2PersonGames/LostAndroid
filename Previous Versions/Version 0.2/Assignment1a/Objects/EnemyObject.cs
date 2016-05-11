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
    class EnemyObject : CollidableObject
    {
        #region Variable Declarations

        int health;
        bool alive;

        #endregion

        public EnemyObject(Texture2D nSprite, Vector2 nPosition, float nScale, 
                           Color nColour, float nDrawDepth, float nRotation)
            : base(nSprite, nPosition, nScale, nColour, nDrawDepth, nRotation, true)
        {
            health = 1;
            alive = true;
        }

        public override void Update()
        {
            if (alive)
            {
                base.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                base.Draw(spriteBatch);
            }
        }

        void Kill()
        {
            alive = false;
        }
    }
}
