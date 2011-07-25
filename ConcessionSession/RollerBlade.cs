using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcessionSession
{
    public class RollerBlade : PowerUp
    {
        private Vector2 newSpeed = new Vector2(9);
        private Vector2 defaultSpeed = new Vector2(6);
        new int timelimit = 5;

        public RollerBlade(string name, Texture2D texture) : base(name, texture)
        {}

        //public override void applyEffect(Sprite_Player player)
        public override void applyEffect(Player player)
        {
            player.Speed = newSpeed;
        }

        //public override void removeEffect(Sprite_Player player)
        public override void removeEffect(Player player)
        {
            player.Speed = defaultSpeed;
        }

        public override int TimeLimit
        {
            get { return timelimit; }
            set { timelimit = value; }
        }
    }
    
}
