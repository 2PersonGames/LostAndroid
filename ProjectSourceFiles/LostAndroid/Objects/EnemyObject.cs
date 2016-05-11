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

        const int ViewRange = 11;

        enum Direction { Left, Right }
        Direction direction;
         
        #endregion

        public EnemyObject(Texture2D nSprite, Vector2 nPosition, float nScale, 
                           Color nColour, float nDrawDepth, float nRotation)
            : base(nSprite, nPosition, nScale, nColour, nDrawDepth, nRotation, true)
        {
            velocity = new Vector2(10, 0);
            direction = Direction.Left;
        }

        public void Update(List<CollidableObject> collideables, Vector2 playerPosition)
        {
            //Check for direction
            if (CollisionDetection(collideables))
            {
                if (direction.Equals(Direction.Left) && velocity.X == 0)
                {
                    direction = Direction.Right;
                    velocity.X = 10;
                }
                else if (direction.Equals(Direction.Right) && velocity.X == 0)
                {
                    direction = Direction.Left;
                    velocity.X = -10;
                }
            }

            //Check to see if enemy can see the player
            Vector2 difference = new Vector2(playerPosition.X - position.X,
                                 playerPosition.Y - position.Y);
            if (direction.Equals(Direction.Left))
            {
                if (difference.X < 0 && difference.Y < ViewRange && 
                    difference.Y > -ViewRange)
                {
                    velocity.X = 20;
                }
                else
                {
                    velocity.X = 10;
                }
            }
            else if (direction.Equals(Direction.Right))
            {
                if (difference.X > 0 && difference.Y < ViewRange &&
                    difference.Y > -ViewRange)
                {
                    velocity.X = -20;
                }
                else
                {
                    velocity.X = -10;
                }
            }
        }
    }
}
