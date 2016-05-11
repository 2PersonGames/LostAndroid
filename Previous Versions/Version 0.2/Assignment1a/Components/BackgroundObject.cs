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
using Assignment1a.Objects;

namespace Assignment1a.Components
{
    class BackgroundObject
    {
        #region Variable Declarations

        Rectangle backgroundRect;
        Texture2D background;

        Texture2D[] clouds;
        Texture2D[] buildings;
        Random random;

        GameObject[] cloudsObjects;
        GameObject[] buildingObjects;

        float scale;

        #endregion

        public BackgroundObject(Game game, Rectangle map, float nScale)
        {
            scale = nScale;
            random = new Random();
            clouds = new Texture2D[5];
            buildings = new Texture2D[3];

            for (int i = 0; i < clouds.Length; i++)
            {
                clouds[i] = game.Content.Load<Texture2D>("Sprites\\Clouds\\Cloud" + (i + 1).ToString());
            }
            int buildingWidth = 0;
            for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i] = game.Content.Load<Texture2D>("Sprites\\Buildings\\Building" + (i + 1).ToString());
                buildingWidth += buildings[i].Width;
            }

            background = game.Content.Load<Texture2D>("Backgrounds\\background0");

            GenerateBuildings(buildingWidth, map, 1);

            backgroundRect = map;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.01f);

            //Draw clouds
            //foreach (GameObject cloud in cloudsObjects)
            //{
            //    cloud.Draw(spriteBatch);
            //}

            //Draw buildings
            foreach (GameObject building in buildingObjects)
            {
                building.Draw(spriteBatch);
            }
        }

        void GenerateBuildings(int buildingWidth, Rectangle map, int density)
        {
            buildingObjects = new GameObject[(int)Math.Round(map.Width / (float)(buildingWidth * scale * 2f)) * density];

            for (int i = 0; i < buildingObjects.Length; i++)
            {
                buildingObjects[i] = new GameObject(buildings[random.Next(0, buildings.Length)], Vector2.Zero, scale * 2f, Color.White, 0.09f, 0f, false);
                buildingObjects[i].Update();
                Vector2 position = new Vector2(0, map.Height - buildingObjects[i].Rect.Height);

                if (i > 0)
                {
                    position.X += buildingObjects[i - 1].Rect.Width;
                }

                while (position.X + buildingObjects[i].Rect.Width > map.X + map.Width)
                {
                    position.X -= map.Width;
                }

                buildingObjects[i].Position = position;
                buildingObjects[i].Update();
            }
        }
    }
}
