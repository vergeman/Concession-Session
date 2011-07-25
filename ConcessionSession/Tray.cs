using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcessionSession
{
    public class Tray : PowerUp
    {
        private int newMax = 3;
        private int defaultMax = 2;
        new int timelimit = 10;

        public Tray(string name, Texture2D texture) : base (name, texture)
        {}

        //public override void applyEffect(Sprite_Player player)
        public override void applyEffect(Player player)
        {
            //player.getPlayer.Capacity = newMax;
            player.Capacity = newMax;
        }

        //public override void removeEffect(Sprite_Player player)
        public override void removeEffect(Player player)
        {
            //player.getPlayer.Capacity = defaultMax;
            player.Capacity = defaultMax;
        }

        
        public override int TimeLimit
        {
            get { return timelimit; }
            set { timelimit = value; }
        }
        
    }
}
