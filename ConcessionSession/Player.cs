using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;



namespace ConcessionSession
{
    public class Player
    {
        private const int ITEM_OFFSET = 70;
        private Vector2 defaultspeed = new Vector2(6);
        //well change this if we implement capacity powerup 
        //we need to figure out where to display other items
        private int capacity = 2;
        private Vector2 speed;

        private int items;
        private int orders;
        private int saves;
        private int bonus;
        private int score;
        
        Sprite_Player player;
        //public Rectangle position;
        private List<Sprite_Item> carrying;
        //PowerUp powerup = null;

        Sprite_PowerUp powerup = null;

        public Player(Sprite_Player player, int itemsServed, int customersSaved, int ordersFinished, int bonusPoints, int charScore)
        {
            this.player = player;
            this.items = itemsServed;
            this.orders = ordersFinished;
            this.saves = customersSaved;
            this.bonus = bonusPoints;
            this.score = charScore;

            speed = defaultspeed;
            isPowerUpActive = false;

            this.carrying = new List<Sprite_Item>();
            //this.position = charPosition;
        }


        public void AddItem(Sprite_Item my_item)
        {
            if (Carrying.Count() < capacity)
            {
                Carrying.Add(my_item);
            }
            //is tehre a "can't carry" sound?
        }


        public void Update()
        {
            //UPDATE ITEM POSITION AS PLAYER MOVES

            //LEFT
            if (Carrying.Count() == 1)
            {
                carrying[0].PositionX = player.Position.X - ITEM_OFFSET;
                carrying[0].PositionY = player.Position.Y;

            }
            //RIGHT
            else if (Carrying.Count() == 2)
            {
                carrying[0].PositionX = player.Position.X - ITEM_OFFSET;
                carrying[0].PositionY = player.Position.Y;

                carrying[1].PositionX = player.Position.X + player.Dimension.X;
                carrying[1].PositionY = player.Position.Y;
            }

            else if (Carrying.Count() == 3)
            {
                carrying[0].PositionX = player.Position.X - ITEM_OFFSET;
                carrying[0].PositionY = player.Position.Y;

                carrying[1].PositionX = player.Position.X + player.Dimension.X;
                carrying[1].PositionY = player.Position.Y;

                carrying[2].PositionX = player.Position.X + (player.Dimension.X / 2 - ITEM_OFFSET / 2);
                carrying[2].PositionY = player.Position.Y - carrying[2].Dimension.Y;
            }
        }

            //Draws the powerup in the middle of the player character
            //Looks like shit but w/e
            //changed it  to sprite player to draw it next to the score when inactive
            /*if (powerup != null)
            {
               
                powerup.PositionX = player.Position.X + (player.Dimension.X / 2 - powerup.Dimension.X / 2);
                powerup.PositionY = player.Position.Y + powerup.Dimension.Y;
            }
         
        }*/
        
        /**
         * More opimized version of update function
        public void Update()
        {
            if (Carrying.Count() > 0)
            {
                //carrying[0].PositionX = player.Position.X - ITEM_OFFSET;
                carrying[0].PositionX = player.Position.X - carrying[0].Dimension.X;
                carrying[0].PositionY = player.Position.Y;

                if (Carrying.Count() > 1)
                {
                    carrying[1].PositionX = player.Position.X + player.Dimension.X;
                    carrying[1].PositionY = player.Position.Y;

                    if (Carrying.Count > 2)
                    {
                        carrying[2].PositionX = player.Position.X + (player.Dimension.X / 2 - ITEM_OFFSET / 2);
                        carrying[2].PositionY = player.Position.Y - carrying[2].Dimension.Y;
                    }
                }
            }

            if (powerup != null)
            {
                powerup.PositionX = player.Position.X + (player.Dimension.X / 2 + powerup.Dimension.X / 2);
                powerup.PositionY = player.Position.Y - powerup.Dimension.Y;
            }
        }
        */

        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public List<Sprite_Item> Carrying
        {
            get { return carrying; }
            set { carrying = value; }
        }

        //public PowerUp Powerup
        public Sprite_PowerUp Powerup
        {
            get { return powerup; }
            set { powerup = value; }
        }

        public Sprite_Player Opponent
        {
            get;
            set;
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }

        public int CustomersSaved
        {
            get
            {
                return saves;
            }
            set
            {
                saves = value;
            }
        }

        public int BonusPoints
        {
            get
            {
                return bonus;
            }
            set
            {
                bonus = value;
            }
        }

        public int OrdersFinished
        {
            get
            {
                return orders;
            }
            set
            {
                orders = value;
            }
        }

        public int ItemsServed
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        public bool isPowerUpActive
        {
            get;
            set;
        }

    }
}

