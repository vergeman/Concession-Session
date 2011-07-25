using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ConcessionSession
{
    public class Customer
    {
        private List<Sprite_Item> order;
        private int patience;
        private int bonus;
        private int level;


        public Customer(List<Sprite_Item> newOrder, int currentLevel)
        {
            level = currentLevel;
            if (level <= 2)
            {
                if (newOrder.Count() == 1)
                    patience = newOrder.Count() * 8 * 1000 * 3;
                else
                    patience = newOrder.Count() * 8 * 1000 * 2;
            }
            else
                patience = newOrder.Count() * 8 * 1000;//5
            bonus = newOrder.Count();
            order = newOrder;
        }


        public List<Sprite_Item> Order
        {
            get { return order; }
            set { order = value; }
        }
        public int Patience
        {
            get { return patience; }
            set { patience = value; }
        }


        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }
    }
}
