using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcessionSession
{
    public class Slip : PowerUp
    {
        new int timelimit = 0;

        public Slip(string name, Texture2D texture)
            : base(name, texture)
        { }

        public override void applyEffect(Player player)
        {
            player.Opponent.getPlayer.Carrying.Clear();
        }

        
        public override void removeEffect(Player player)
        {
        }

        public override int TimeLimit
        {
            get { return timelimit; }
            set { timelimit = value; }
        }
        
    }
}
