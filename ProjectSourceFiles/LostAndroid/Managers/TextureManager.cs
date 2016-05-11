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
    public static class TextureManager
    {
		const int MaxClouds = 5;
		const int MaxBuildings = 3;
		const int MaxBlocks = 2;
		
		private static Texture2D[] clouds;
		public static Texture2D[] Clouds
		{
			get { return clouds; }
		}
		private static Texture2D[] buildings;
		public static Texture2D[] Buildings
		{
			get { return buildings; }
		}
		private static Texture2D sky;
		public static Texture2D Sky
		{
			get { return sky; }
		}
		
		private static Texture2D pausedBackground;
		public static Texture2D PausedBackground
		{
			get { return pausedBackground; }
		}
		
		private static Texture2D[] blocks;
		public static Texture2D[] Blocks
		{
			get { return blocks; }
		}

        private static Texture2D enemy;
        public static Texture2D Enemy
        {
            get { return enemy; }
        }

        private static Texture2D finish;
        public static Texture2D Finish
        {
            get { return finish; }
        }
        private static Texture2D start;
        public static Texture2D Start
        {
            get { return start; }
        }

        private static Texture2D playerStill;
        public static Texture2D PlayerStill
        {
            get { return playerStill; }
        }
        private static Texture2D playerAnimated;
        public static Texture2D PlayerAnimated
        {
            get { return playerAnimated; }
        }

        private static Texture2D playerTwo;
        public static Texture2D PlayerTwo
        {
            get { return playerTwo; }
        }

        private static Texture2D loadingBackground;
        public static Texture2D LoadingBackground
        {
            get { return loadingBackground; }
        }

        private static Texture2D buttonX;
        public static Texture2D ButtonX
        {
            get { return buttonX; }
        }
		
		public static void Load(ContentManager Content)
		{
            //Clouds
			clouds = new Texture2D[MaxClouds];
			for (int i = 0; i < clouds.Length; i++)
			{
				clouds[i] = Content.Load<Texture2D>("Sprites\\Clouds\\Cloud" + (i + 1).ToString());
			}
			
            //Buildings
			buildings = new Texture2D[MaxBuildings];
			for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i] = Content.Load<Texture2D>("Sprites\\Buildings\\Building" + (i + 1).ToString());
            }
			
            //Backgrounds
			sky = Content.Load<Texture2D>("Backgrounds\\background0");
			pausedBackground = Content.Load<Texture2D>("Backgrounds\\pausedBackground");
            loadingBackground = Content.Load<Texture2D>("Backgrounds\\loadingBackground");
			
            //Blocks
			blocks = new Texture2D[MaxBlocks];
			for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = Content.Load<Texture2D>("Sprites\\Blocks\\block" + i.ToString());
            }

            //Enemy
            enemy = Content.Load<Texture2D>("Sprites\\Player\\girlakin_standing");

            //Start
            start = Content.Load<Texture2D>("Sprites\\Blocks\\door");
            
            //Finish
            finish = Content.Load<Texture2D>("Sprites\\Blocks\\door");

            //Player
            playerStill = Content.Load<Texture2D>("Sprites\\player\\girlakin_standing");
            playerAnimated = Content.Load<Texture2D>("Sprites\\player\\girlakin_running");
            playerTwo = Content.Load<Texture2D>("Sprites\\player\\playerTwo");

            //Buttons
            buttonX = Content.Load<Texture2D>("Sprites\\Buttons\\x");
		}
    }
}
