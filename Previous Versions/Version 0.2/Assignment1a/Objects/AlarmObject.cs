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
    class AlarmObject
    {
        TimeSpan alarm;
        TimeSpan current;

        public AlarmObject(int alarmPeriodInSeconds)
        {
            current = TimeSpan.Zero;
            alarm = new TimeSpan(0, 0, alarmPeriodInSeconds);
        }

        public bool Update(GameTime gameTime)
        {
            current += gameTime.ElapsedGameTime;

            if (current >= alarm)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public new string ToString()
        {
            TimeSpan now = alarm - current;
            return now.Minutes.ToString() + ":" + now.Seconds.ToString();
        }
    }
}
