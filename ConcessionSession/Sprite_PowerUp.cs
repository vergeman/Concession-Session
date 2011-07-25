using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ConcessionSession
{
    public class Sprite_PowerUp : Sprite_Item
    {
        //PowerUp powerup;
        int collisionOffset = 20;
        new int scale = 1;
        Texture2D Ptexture;

        public Sprite_PowerUp(PowerUp powerup, Vector2 position, AssetManager assetManager) 
            : base("mystery_box", position, assetManager)
        {
            //we can replace the "mystery box" base arg back to powerup.Name arg if we decide
            //the mystery isn't useful.  If both can visually see the powerup (if powerup numbers are limited)
            //it may foster more competition.
            Ptexture = powerup.Texture;
            Powerup = powerup;
            
        }

        public PowerUp Powerup
        {
            get;
            set;
        }

        public void reveal()
        {
            base.scale = 1.0f;
            base.frameSize = new Point(Powerup.Texture.Width, Powerup.Texture.Height);
            base.Texture = Powerup.Texture;


        }

        new public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)(position.X + (collisionOffset * scale)),
                (int)(position.Y + (collisionOffset * scale)),
                (int)((frameSize.X - (collisionOffset * 2)) * scale),
                (int)((frameSize.Y - (collisionOffset * 2)) * scale));
            }
        }
    }
}
