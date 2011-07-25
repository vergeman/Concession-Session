using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace ConcessionSession
{
    public class Sprite_Customer : Sprite
    {
        //this is for Scaling CONSTANTS....
        //Rectangle RECT_CUSTOMER = new Rectangle(0, 0, 105, 140);
        Rectangle RECT_CUSTOMER = new Rectangle(0, 0, 76, 116);
        Rectangle RECT_ORDER = new Rectangle(0, 0, 60, 75);
        Rectangle RECT_BUBBLE = new Rectangle(0, 0, 300, 100);
        const int LINE = 2;  //newline / column for position of customer items

        string name;
        CustomerAsset asset;
        Customer my_customer;
        AssetManager assetManager;
        int level;
        bool soundPlayed = false;



        //we use bubble as our "active" sprite

        public Sprite_Customer(string name, CustomerAsset asset, AssetManager assetManager, int level,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, SoundEffect collision_sound)
            //bubble
            : base(assetManager.GetTextures(asset.Bubble_file), asset.vBubble,
            new Point(assetManager.GetTextures(asset.Bubble_file).Width, assetManager.GetTextures(asset.Bubble_file).Height),
            collisionOffset, currentFrame, sheetSize, speed, 0, collision_sound)
        {
            this.name = name;
            this.asset = asset;
            this.assetManager = assetManager;
            this.level = level;
            this.my_customer = new Customer(this.Order(), level);

            //we use this because assets require scaling....
            RECT_CUSTOMER.X = (int)asset.vCustomer.X;
            RECT_CUSTOMER.Y = (int)asset.vCustomer.Y;
            RECT_ORDER.X = (int)asset.vOrder.X;
            RECT_ORDER.Y = (int)asset.vOrder.Y;
            RECT_BUBBLE.X = (int)asset.vBubble.X;
            RECT_BUBBLE.Y = (int)asset.vBubble.Y;

        }

        public CustomerAsset Asset
        {
            get { return this.asset; }
        }

        public bool isExpired()
        {
            if (my_customer.Patience < 0 || my_customer.Order.Count == 0)
            {
                return true;
            }
            return false;
        }
        //Generate Order
        //hmm this seems like a real odd function...

        //this function generates an order by using randomly generated numbers for the 
        //number of items in an order, picking an item from the item list and he number of that item to
        //be ordered and returns that list.
        public List<Sprite_Item> Order()
        {
            int items;
            int Ofeach;
            int itemNumber;
            Random random = new Random();
            List<Sprite_Item> available = assetManager.master_ItemList;    //foodKinds
            List<int> used = new List<int>();
            List<Sprite_Item> finalList = new List<Sprite_Item>();

            if (level < 4)
            {
                items = random.Next(1, level + 2);
            }
            else
            {

                items = random.Next(2, 5);
            }
            while (items != 0)
            {
                random = new Random();
                itemNumber = random.Next(0, available.Count());
                if (used.Contains(itemNumber)) { }
                else
                {
                    used.Add(itemNumber);
                    Ofeach = random.Next(1, items);
                    items = items - Ofeach;
                    for (int i = 1; i <= Ofeach; i++)
                    {

                        finalList.Add(new Sprite_Item(available[itemNumber].ItemName, Vector2.Zero, assetManager));
                    }
                }
            }
            return finalList;
        }




        public void Update(List<Sprite_Player> playerList, GameTime gameTime, Rectangle clientBounds)
        {

            //Bubble Color
            base.Texture = assetManager.GetTextures(asset.Bubble_file);


            foreach (Sprite_Player p in playerList)
            {
                if (p.collisionRectImage.Intersects(this.collisionRect))
                {
                    switch (p.PlayerIndex)
                    {
                        case PlayerIndex.One:
                            base.Texture = assetManager.GetTextures(asset.Bubble_file + "Red");
                            break;
                        case PlayerIndex.Two:
                            base.Texture = assetManager.GetTextures(asset.Bubble_file + "Blue");
                            break;
                        case PlayerIndex.Three:
                            base.Texture = assetManager.GetTextures(asset.Bubble_file + "Blue");
                            break;
                        case PlayerIndex.Four:
                            base.Texture = assetManager.GetTextures(asset.Bubble_file + "Blue");
                            break;
                    }

                    //CHECK & SERVE ITEMS
                    orderComparison(p.getPlayer, this.my_customer, p.GetpreviousGPState, p.GetcurrentGPState);


                }
            }

            //ITEMS POSITION UPDATE (for draw)
            updateItemPos();

            //Countdown
            my_customer.Patience -= gameTime.ElapsedGameTime.Milliseconds;

            //countdown: sound - might want a concurrency check to prevent overloading
            if (10 == (int)my_customer.Patience / 1000)
            {
                timeEndingSounds(this.name);
            }

            //update non-implemented animations
            base.Update(gameTime, clientBounds);
        }






        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects Effect)
        {
            if (asset.Orientation.Contains("Right"))
            {
                Effect = SpriteEffects.FlipHorizontally;
            }

            //Draw Customer - autoscale
            spriteBatch.Draw(assetManager.GetTextures(name), RECT_CUSTOMER, null, Color.White,
                0, Vector2.Zero, Effect, 0.0f);

            //Draw Items - these all use the internal scaling constructor
            foreach (Sprite_Item s in my_customer.Order)
            {
                s.Draw(spriteBatch, RECT_ORDER);
            }


            //Bubble Draw
            base.Draw(gameTime, spriteBatch, Effect, .99f);

            //Countdown Draw
            if (my_customer.Patience / 1000 < 10)
            {
                spriteBatch.DrawString(assetManager.GetFont[1],
                    (my_customer.Patience / 1000).ToString(), this.asset.vCountdown, Color.Yellow,
                    0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            }


        }



        public void updateItemPos()
        {
            //Very messy....bleh
            int count = 0;
            int tweak = 10;
            RECT_ORDER.X = (int)asset.vOrder.X + tweak;
            RECT_ORDER.Y = (int)asset.vOrder.Y + 5;

            foreach (Sprite_Item s in my_customer.Order)
            {
                if (asset.Orientation.Contains("top"))
                {
                    s.Position = new Vector2((int)RECT_ORDER.X, (int)RECT_ORDER.Y);
                    RECT_ORDER.X = RECT_ORDER.X + RECT_ORDER.Width - 5;

                }
                else
                {
                    if (count == LINE)
                    {
                        RECT_ORDER.X = (int)asset.vOrder.X + tweak;
                        RECT_ORDER.Y = RECT_ORDER.Y + RECT_ORDER.Height;
                    }
                    s.Position = new Vector2((int)RECT_ORDER.X, (int)RECT_ORDER.Y);
                    RECT_ORDER.X = RECT_ORDER.X + RECT_ORDER.Width - 5;

                }
                count++;
            }
        }


        private void orderComparison(Player player, Customer current, GamePadState previousState, GamePadState padNow)
        {

            if (player.Carrying.Count() == 1 &&
                (padNow.Buttons.LeftShoulder == ButtonState.Released &&
                previousState.Buttons.LeftShoulder == ButtonState.Pressed
                    || padNow.Triggers.Left < .5 && previousState.Triggers.Left > .5))
            {
                updateOrder(player, current, 0);
            }
            else if (player.Carrying.Count() == 2)
            {
                //while most button presses occur after the button is released the right shoulder button
                //is processes here when the button is pressed. This solves the problem of pressing both
                //shoulder buttons at the same time and the left item being used
                if (padNow.Buttons.RightShoulder == ButtonState.Pressed || padNow.Triggers.Right > .5)
                {
                    updateOrder(player, current, 1);
                }
                if (padNow.Buttons.LeftShoulder == ButtonState.Released
                && previousState.Buttons.LeftShoulder == ButtonState.Pressed
                || padNow.Triggers.Left < .5 && previousState.Triggers.Left > .5)
                {
                    updateOrder(player, current, 0);
                }
            }
            else if (player.Carrying.Count() == 3)
            {
                if (padNow.Buttons.RightShoulder == ButtonState.Pressed || padNow.Triggers.Right > .5)
                {
                    updateOrder(player, current, 1);
                }
                if (padNow.Buttons.LeftShoulder == ButtonState.Released
                && previousState.Buttons.LeftShoulder == ButtonState.Pressed
                || padNow.Triggers.Left < .5 && previousState.Triggers.Left > .5)
                {
                    updateOrder(player, current, 0);
                }
                if (padNow.DPad.Up == ButtonState.Released && previousState.DPad.Up == ButtonState.Pressed)
                {
                    updateOrder(player, current, 2);
                }
            }
        }

        public void updateOrder(Player player, Customer current, int item)
        {
            //SCORING
            if (current.Order.Contains(player.Carrying[item]))
            {
                current.Order.Remove(player.Carrying[item]);
                player.Carrying.RemoveAt(item);
                player.ItemsServed = player.ItemsServed + 1;
                player.Score = player.Score + 5;

                assetManager.GetSounds("CashRegister").Play();
                //serveSound.Play();

                if (current.Order.Count == 0)
                {
                    doneSounds(this.name);
                    player.OrdersFinished = player.OrdersFinished + 1;
                    

                    if (current.Patience/1000 < 5)
                    {
                        player.CustomersSaved = player.CustomersSaved + 1;
                        current.Bonus = current.Bonus + 3;
                    }
                    player.BonusPoints = player.BonusPoints + current.Bonus;
                    player.Score += current.Bonus;
                }

            }
            else
            {
                assetManager.GetSounds("wrong item").Play();
                //wrongItemSound.Play();
            }
        }

        //play on completion of order
        public void doneSounds(string name)
        {
            switch (name)
            {
                case "mrSlave":
                    assetManager.GetSounds("donesy").Play();
                    break;
                case "homer":
                    assetManager.GetSounds("Woohoo").Play();
                    break;
                case "superman":
                    assetManager.GetSounds("upAway").Play();
                    break;
                case "donkey":
                    assetManager.GetSounds("winDonkey").Play();
                    break;
                case "jackie":
                    assetManager.GetSounds("jackieokay").Play();
                    break;
                case "arnold":
                    assetManager.GetSounds("ilbeback").Play();
                    break;
                case "ironMan":
                    assetManager.GetSounds("IAmIronMan").Play();
                    break;
                case "leeloo":
                    assetManager.GetSounds("multipassLeeloo").Play();
                    break;
                case "mickey":
                    assetManager.GetSounds("winMickey").Play();
                    break;
                case "Scarlett":
                    assetManager.GetSounds("goodThingsScarlett").Play();
                    break;
            }
        }

        //to be played < 10 seconds
        public void timeEndingSounds(string name)
        {
            if (this.soundPlayed == false)
            {
                switch (name)
                {
                    case "mrSlave":
                        assetManager.GetSounds("jesus").Play();
                        break;
                    case "homer":
                        assetManager.GetSounds("doh").Play();
                        break;
                    case "superman":
                        assetManager.GetSounds("supermanBored").Play();
                        break;
                    case "donkey":
                        assetManager.GetSounds("feelrightDonkey").Play();
                        break;
                    case "jackie":
                        assetManager.GetSounds("thewordsJackie").Play();
                        break;
                    case "arnold":
                        assetManager.GetSounds("hastalavistaArnold").Play();
                        break;
                    case "ironMan":
                        assetManager.GetSounds("run").Play();
                        break;
                    case "leeloo":
                        assetManager.GetSounds("badaboom").Play();
                        break;
                    case "mickey":
                        assetManager.GetSounds("looseMickey").Play();
                        break;
                    case "Scarlett":
                        assetManager.GetSounds("watchScarlett").Play();
                        break;
                }
            }
            this.soundPlayed = true;
        }


        public override Vector2 direction
        {
            get { return position; }
        }
    }
}
