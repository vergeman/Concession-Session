using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConcessionSession
{
    public abstract class PowerUp
    {
        protected int timelimit;

        public PowerUp(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
        }

        public string Name { get; set; }

        public Texture2D Texture { get; set; }

        public abstract int TimeLimit
        {
            get;
            set;
        }

        public abstract void applyEffect(Player player);

        public abstract void removeEffect(Player player);

    }
}
