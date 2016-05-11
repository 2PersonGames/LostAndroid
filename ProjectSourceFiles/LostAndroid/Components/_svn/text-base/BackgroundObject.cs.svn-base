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
using Assignment1a.Managers;

namespace Assignment1a.Components
{
    class BackgroundObject
    {
        #region Variable Declarations

        const int MaxCloudSpeed = -20;

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
                clouds[i] = TextureManager.Clouds[i];
            }
            int buildingWidth = 0;
            for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i] = TextureManager.Buildings[i];
                buildingWidth += buildings[i].Width;
            }

            background = TextureManager.Sky;

            GenerateBuildings(map, 0.007f);
            GenerateClouds(map, 0.007f);

            backgroundRect = map;
        }

        public void Update()
        {
            //Move clouds
            foreach (GameObject cloud in cloudsObjects)
            {
                cloud.Update();
                if (cloud.Position.X + cloud.Rect.Width < backgroundRect.X)
                {
                    Vector2 position = new Vector2(backgroundRect.Width + backgroundRect.X, random.Next(backgroundRect.Y, backgroundRect.Y + backgroundRect.Height));
                    Vector2 velocity = new Vector2(random.Next(MaxCloudSpeed, 0), 0);

                    cloud.Position = position;
                    cloud.Velocity = velocity;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundRect, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.01f);

            //Draw clouds
            foreach (GameObject cloud in cloudsObjects)
            {
                cloud.Draw(spriteBatch);
            }

            //Draw buildings
            foreach (GameObject building in buildingObjects)
            {
                building.Draw(spriteBatch);
            }
        }

        void GenerateBuildings(Rectangle map, float density)
        {
            if (density > 1)
            {
                density = 1;
            }
            else if (density < 0)
            {
                density = 0;
            }

            int numberOfBuildings = (int)Math.Round(map.Width * density);
            buildingObjects = new GameObject[numberOfBuildings];

            Vector2 position = Vector2.Zero;
            for (int i = 0; i < numberOfBuildings; i++)
            {
                GameObject building = new GameObject(buildings[random.Next(0, buildings.Length)], Vector2.Zero, 1f, Color.White, (float)(random.NextDouble() * 0.1f) + 0.3f, 0f, false);
                building.Update();

                position.Y = (map.Y / 2f) + map.Height - building.Sprite.Height;
                position.Y -= random.Next(0, 11);
                building.Position = position;
                building.Update();

                buildingObjects[i] = building;

                position.X += building.Rect.Width;

                if (position.X > (map.Width + map.X))
                {
                    position.X -= (map.Width + map.X);
                }
            }
        }

        void GenerateClouds(Rectangle map, float density)
        {
            if (density > 1)
            {
                density = 1;     
            }
            else if (density < 0)
            {
                density = 0;
            }

            int numberOfClouds = (int)Math.Round(map.Width * density);

            cloudsObjects = new GameObject[numberOfClouds];

            for (int i = 0; i < cloudsObjects.Length; i++)
            {
                GameObject cloud = new GameObject(clouds[random.Next(0, clouds.Length)], Vector2.Zero, scale, Color.White, (float)(random.NextDouble() * 0.1f) + 0.3f, 0f, false);
                Vector2 position = new Vector2(random.Next(map.X, map.X + map.Width), random.Next(map.Y, map.Y + map.Height));
                Vector2 velocity = new Vector2(random.Next(MaxCloudSpeed, 0), 0);

                cloud.Position = position;
                cloud.Velocity = velocity;

                cloudsObjects[i] = cloud;
            }
        }
    }
}
