using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ConcessionSession
{
    public class Sprite_Item : Sprite, IEquatable<Sprite_Item>
    {
        string itemName;

        public Sprite_Item(string itemName, Vector2 position, Texture2D itemPic, SoundEffect itemSound)
            : base(itemPic, position, new Point(itemPic.Width, itemPic.Height), 0,
            Point.Zero, Point.Zero, Vector2.Zero, 0, null)
        {
            this.itemName = itemName;
        }


        public Sprite_Item(string itemName, Vector2 position, AssetManager assetManager)
            : base(assetManager.GetTextures(itemName), position,
            new Point(assetManager.GetTextures(itemName).Width, assetManager.GetTextures(itemName).Height), 0,
            Point.Zero, Point.Zero, Vector2.Zero, 0, null)
        {
            this.itemName = itemName;
        }

        public string ItemName
        {
            get { return this.itemName; }
        }

        /*override these guys for Sprite_Customer ordercomparison() via objects*/
        public bool Equals(Sprite_Item other)
        {
            if (this.itemName.Equals(other.itemName))
                return true;
            else
                return false;
        }

        public override bool Equals(Object obj)
        {
            return this.Equals(obj as Sprite_Item);
        }
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }




        //i suspect we can override this for powerups
        public override Vector2 direction
        {
            get { return position; }
        }
    }
}
