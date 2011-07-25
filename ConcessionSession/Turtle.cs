using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcessionSession
{
    public class Turtle : PowerUp
    {
        private Vector2 newSpeed = new Vector2(4);
        private Vector2 defaultSpeed = new Vector2(6);
        new int timelimit = 5;

        public Turtle(string name, Texture2D texture)
            : base(name, texture)
        { }

        public override void applyEffect(Player player)
        {
            player.Opponent.getPlayer.Speed = newSpeed;
        }

        public override void removeEffect(Player player)
        {
            player.Opponent.getPlayer.Speed = defaultSpeed;
        }

        public override int TimeLimit
        {
            get { return timelimit; }
            set { timelimit = value; }
        }
    }
}
