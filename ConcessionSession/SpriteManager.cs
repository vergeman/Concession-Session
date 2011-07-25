using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConcessionSession
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        /*SETTINGS*/
        Vector2 player_speed = new Vector2(6, 6);


        /*GLOBALS*/
        AssetManager assetManager;
        SpriteBatch spriteBatch;
        Sprite_Arena arena;


        Random randomNum = new Random();
        int level = 5;


        //these are the variables used for the customer manager function
        //intervals refer to time between customers
        int currentInterval = 0;
        int intervalCounter = 0;

        //Number of customers to have on the screen as a goal
        int numberToMeet = 3;
        int lowestGoal;
        int counter;
        bool met = false;

        TimeSpan powerGen;

        List<Sprite_Table> tableList = new List<Sprite_Table>();
        List<Sprite_Player> playerList = new List<Sprite_Player>();
        List<Sprite_Customer> customerList = new List<Sprite_Customer>();
        
        List<PowerUp> powerList = new List<PowerUp>();
        List<Sprite_PowerUp> testList = new List<Sprite_PowerUp>();
        //Current Power-Ups in the Arena
        Sprite_PowerUp[] powlist = new Sprite_PowerUp[4];
        //Position for the powerups. The length of powlist[] and powerpositions[] must be the same.
        Vector2[] powerpositions = new Vector2[4];
       
        
        InfoPanel iPanel;

        //note: positions start (0,0) upper left corner
        /*PLAYER POSITIONS*/
        readonly Vector2 PLAYER1_POS = new Vector2(529, 306);
        readonly Vector2 PLAYER2_POS = new Vector2(689, 306);
        readonly Vector2 PLAYER3_POS = new Vector2(529, 256);   //not tested
        readonly Vector2 PLAYER4_POS = new Vector2(689, 256);   //not tested

        /*TABLE POSITIONS*/
        readonly Vector2 TBL_CANDY_POS = new Vector2(556, 280);
        readonly Vector2 TBL_NACHOS_POS = new Vector2(556, 540);//556 465
        readonly Vector2 TBL_POPCORN_POS = new Vector2(853, 396);
        readonly Vector2 TBL_SODA_POS = new Vector2(259, 385);
        readonly Vector2 TBL_GARBAGE_POS = new Vector2(595, 420);

        /*CUSTOMER POSITIONS*/
        //SEE AssetManager.cs / "LoadCustomerAssets" for hardcoded positions...for now.

        /*Power Up Positions*/
        readonly Vector2 POWERUP1 = new Vector2(350, 305);
        readonly Vector2 POWERUP2 = new Vector2(925, 305);
        readonly Vector2 POWERUP3 = new Vector2(350, 555);
        readonly Vector2 POWERUP4 = new Vector2(925, 555);
        const int SCORE_TOLERANCE = 30;
        const int POWERUP_GENTIME = 5;

        public SpriteManager(Game game)
            : base(game)
        {
            assetManager = new AssetManager(game);
            
        }




        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            powerpositions[0] = POWERUP1;
            powerpositions[1] = POWERUP2;
            powerpositions[2] = POWERUP3;
            powerpositions[3] = POWERUP4;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Vector2 center = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);


            //==LOADASSETS==
            assetManager.LoadTextures();
            assetManager.LoadSounds();
            assetManager.LoadCustomerNames();
            //see AssetManager for definitions - we have a class with static positions
            //perhaps we can change later, kept for conformity sake
            assetManager.LoadCustomerAssets();
            //assetManager.LoadPowerUps();


            //==LOADARENA()==
            arena = new Sprite_Arena(assetManager.GetTextures("arenaTop"), assetManager.GetTextures("arenaBottom"),
                new Vector2(0, 100), new Vector2(0, Game.Window.ClientBounds.Height + 100 - assetManager.GetTextures("arenaBottom").Height));


            //==LOADPLAYERS()==
            LoadPlayers();

            //==LOAD TABLES()==
            LoadTables();

            LoadPowerups();

            //==CUSTOMERS() ==
            //CustomerManager();

            //==INFOPANEL==
            iPanel = new InfoPanel(playerList, assetManager, ((Game2)Game).Time);

            base.LoadContent();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //update info panel
            counter = counter + 1;
            ((Game2)Game).Time -= gameTime.ElapsedGameTime.Milliseconds;    //will determine end of game here
            iPanel.Update(gameTime);

            foreach (Sprite_Player p in playerList)
            {
                if (p.getPlayer.Powerup == null)
                {
                    //foreach (Sprite_PowerUp pu in powerList)
                    int i = 0;
                    foreach (Sprite_PowerUp pu in powlist)
                    {
                        if (pu != null)
                        {
                            if (p.collisionRect.Intersects(pu.collisionRect))
                            {
                                p.getPlayer.Powerup = pu;
                                powlist[i] = null;
                                break;
                            }
                        }
                        i++;
                    }
                }
                //we do iterate through tables internally here 
                //but its here due to player movement purposes
                p.Update(gameTime, arena, tableList, Game.Window.ClientBounds);

            }

            //Table Glow
            foreach (Sprite_Table t in tableList)
            {
                t.Update(playerList, gameTime, Game.Window.ClientBounds);
            }

            //UPDATE CUSTOMERS
            CustomerManager();  //spawn if necessary
            //not sure about timing implementation here -i think i botched it

            //Updated & Clean expired
            for (int x = 0; x < customerList.Count; x++)
            {
                customerList[x].Update(playerList, gameTime, Game.Window.ClientBounds);

                if (customerList[x].isExpired())
                {
                    customerList.RemoveAt(x);
                    x--;
                }
            }

            
                generatePowerUps(gameTime);
           

            base.Update(gameTime);
        }




        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
              SpriteSortMode.FrontToBack, SaveStateMode.None);
            {

                

                //draw time & score
                iPanel.Draw(gameTime, spriteBatch, SpriteEffects.None);

                //DRAW TABLES
                foreach (Sprite_Table t in tableList)
                {
                    t.Draw(gameTime, spriteBatch, SpriteEffects.None);
                }

                //DRAW POWERUPS
                foreach (Sprite_PowerUp p in powlist)
                //foreach (Sprite_PowerUp p in testList)
                {
                    if (p != null)
                    {
                        p.Draw(gameTime, spriteBatch, SpriteEffects.None);
                    }
                }

                //DRAW PLAYERS (uses base class Sprite - watch on layerdepth)
                foreach (Sprite_Player p in playerList)
                {
                    p.Draw(gameTime, spriteBatch, SpriteEffects.None);
                }

                //DRAW ARENA
                arena.Draw(spriteBatch);

                //DRAW CUSTOMERS
                foreach (Sprite_Customer c in customerList)
                {
                    c.Draw(gameTime, spriteBatch, SpriteEffects.None);
                }


                if (playerList[0].getPlayer.Score > playerList[1].getPlayer.Score)
                    spriteBatch.Draw(assetManager.GetTextures("victory"), new Rectangle((int)playerList[0].PositionX - 55, (int)playerList[0].PositionY - 50,
                                76,60), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
                else if (playerList[1].getPlayer.Score > playerList[0].getPlayer.Score)
                    spriteBatch.Draw(assetManager.GetTextures("victory"), new Rectangle((int)playerList[1].PositionX - 55, (int)playerList[1].PositionY - 50,
                                76, 60), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);


                base.Draw(gameTime);
            }
            spriteBatch.End();

        }


        public List<Sprite_Player> getPlayers()
        {
            return playerList;
        }


        /*====LOADING HELPERS====*/


        public void CustomerManager()
        {
            switch (level)
            {
                case 1:
                    CustomerSpawn(4, 4, 2, 3);
                    break;
                case 2:
                    CustomerSpawn(2, 4, 2, 5);
                    break;
                case 3:
                case 4:
                    CustomerSpawn(2, 4, 3, 5);
                    break;
                default:
                   CustomerSpawn(2, 3, 3, 5);
                    break;
            }
        }


        public void CustomerSpawn(int lowerInterval, int upperInterval, int toMeetLower, int toMeetUpper)
        {
            int random;

            int temp;

            if (counter == 61) counter = 0;

            if (met == true)
            {
                if (customerList.Count < lowestGoal)
                {
                    numberToMeet = randomNum.Next(toMeetLower, toMeetUpper + 1);
                    met = false;
                    currentInterval = randomNum.Next(lowerInterval, upperInterval + 1);
                    intervalCounter = 0;
                }
            }

            else
            {
                if (customerList.Count < numberToMeet)
                {
                    if (counter == 60) intervalCounter++;
                    
                    if (intervalCounter == currentInterval)
                    {
                        intervalCounter = 0;
                        currentInterval = randomNum.Next(lowerInterval, upperInterval + 1);
                        temp = customerList.Count + 1;
                        while (temp != customerList.Count)
                        {
                            random = randomNum.Next(0, 7);
                            if (!IsCustomerExist(assetManager.GetCustomerAsset(random).Orientation))
                            {
                                //customerList.Add(new Sprite_Customer(assetManager.GetCustomerName(randomNum.Next(0, 10)),
                                customerList.Add(new Sprite_Customer(assetManager.GetCustomerName(randomNum.Next(10, 12)),  
                                assetManager.GetCustomerAsset(random), assetManager, level,
                                    0, Point.Zero, Point.Zero, Vector2.Zero, 0, null));
                            }
                        }
                    }
                }
                else if (customerList.Count == numberToMeet)
                {
                    met = true;
                    lowestGoal = numberToMeet;
                }
            }
            /*while (customerList.Count < limit)
            {
                random = randomNum.Next(0, 7);
                if (!IsCustomerExist(assetManager.GetCustomerAsset(random).Orientation))
                {
                    customerList.Add(new Sprite_Customer(assetManager.GetCustomerName(randomNum.Next(0, 9)),
                        assetManager.GetCustomerAsset(random), assetManager, level,
                        0, Point.Zero, Point.Zero, Vector2.Zero, 0, null));
                }

            }*/

        }

        public void setLevel(int currentLevel)
        {
            level = currentLevel;
        }


        public bool IsCustomerExist(string orientation)
        {
            foreach (Sprite_Customer c in customerList)
            {
                if (c.Asset.Orientation.Equals(orientation))
                {
                    return true;
                }
            }
            return false;
        }


        /*generates powerup position on the board
         * this is limited to one right now
         * 
        */

        private void generatePowerUps(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Subtract(powerGen).Seconds >= POWERUP_GENTIME)
            {
                //asset list
                PowerUp[] parray = powerList.ToArray();

                //populate available powerups on field - "i" in loop below sets quantity
                //of powerups to be generated
                for (int i = 0; i < 1; i++)
                {
                    if (powlist[i] == null)
                    {
                        //Sprite_PowerUp(which powerup, where placed, asset looks
                        powlist[i] = new Sprite_PowerUp(parray[randomNum.Next(parray.Length)],
                            powerpositions[genPowerUpPos(playerList)], assetManager);
                        
                        break;
                    }
                }
                powerGen = gameTime.TotalGameTime;
            }
            /*
                    for (int i = 0; i < 4; i++)
                    {
                
                        if (powlist[i] == null)
                        {
                            int j = randomNum.Next(parray.Length);
                            //testList.Add(new Sprite_PowerUp(parray[j], powerpositions[i], assetManager));
                            powlist[i] = new Sprite_PowerUp(parray[j], powerpositions[i], assetManager);
                        }
                
                    }
                     */
        }

        /*this selects a powerup location closest to the the losing player
         *help the little guy hurt the man
         */

        private int genPowerUpPos(List<Sprite_Player> playerList)
        {
            int player = 0, min_pos = 0, x = 0;

            Vector2 min = new Vector2(10000, 10000);

            if (playerList[0].getPlayer.Score - playerList[1].getPlayer.Score > SCORE_TOLERANCE)
            {
                player = 1;
            }
            else if (playerList[1].getPlayer.Score - playerList[0].getPlayer.Score > SCORE_TOLERANCE)
            {
                player = 0;
            }
            else
            {
                return randomNum.Next(powerList.ToArray().Length);
            }

            for (x = 0; x < powerpositions.Length; x++)
            {
                if (Vector2.Distance(playerList[player].Position, powerpositions[x]) < Vector2.Distance(min, powerpositions[x]))
                {
                    min = powerpositions[x];
                    min_pos = x;
                }
            }

            return min_pos;
        }

        /*=====LOAD ASSETS (PRE GAME)=====*/
        public void LoadPowerups()
        {
            powerList.Add(new RollerBlade("rollerBlades", assetManager.GetTextures("rollerBlades")));
            powerList.Add(new Tray("tray", assetManager.GetTextures("tray")));
            powerList.Add(new Turtle("turtle", assetManager.GetTextures("turtle")));
            powerList.Add(new Slip("slipnfall", assetManager.GetTextures("slipnfall")));

            //generatePowerUps();
        }

        public void LoadTables()
        {
            //name, table, tableglow, sound, assetmanager, start position
            //our asset filename kinda restricts these to being static...alas...
            tableList.Add(new Sprite_Table("popcorn", "popcornTable", "popcornTableGlow", "popcorn", assetManager,
                TBL_POPCORN_POS, (int)playerList[0].Dimension.Y / 2, Point.Zero, Point.Zero,
                Vector2.Zero, 0));

            tableList.Add(new Sprite_Table("candy", "candyTable", "candyTableGlow", "candy", assetManager,
                TBL_CANDY_POS, (int)playerList[0].Dimension.Y / 2, Point.Zero, Point.Zero,
                Vector2.Zero, 0));

            tableList.Add(new Sprite_Table("nachos", "nachosTable", "nachosTableGlow", "nachos", assetManager,
                TBL_NACHOS_POS, (int)playerList[0].Dimension.Y / 2, Point.Zero, Point.Zero,
                Vector2.Zero, 0));

            tableList.Add(new Sprite_Table("soda", "sodaTable", "sodaTableGlow", "soda", assetManager,
                TBL_SODA_POS, (int)playerList[0].Dimension.Y / 2, Point.Zero, Point.Zero,
                Vector2.Zero, 0));

            //dimension / 3 is arbitrary since the garbage is smaller..
            tableList.Add(new Sprite_Table("garbage", "garbage", "garbageGlow", "recycle", assetManager,
                TBL_GARBAGE_POS, (int)playerList[0].Dimension.Y * 2 / 5, Point.Zero, Point.Zero,
                Vector2.Zero, 0));

        }



        public void LoadPlayers()
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                Sprite_Player player1 = new Sprite_Player(PlayerIndex.One, assetManager.GetTextures("character_sheet2"), assetManager.GetFont[2], PLAYER1_POS,
                        0, new Point(76, 116), new Point(0, 0), new Point(3, 6), player_speed, 160, null);

                Sprite_Player player2 = new Sprite_Player(PlayerIndex.Two, assetManager.GetTextures("character_sheet"), assetManager.GetFont[2], PLAYER2_POS,
                    0, new Point(76, 116), new Point(0, 0), new Point(3, 6), player_speed, 160, null);

                player1.getPlayer.Opponent = player2;
                player2.getPlayer.Opponent = player1;

                playerList.Add(player1);
            
                playerList.Add(player2);
            }
            //WE CAN ADD MORE PLAYERS HERE max 4? once we get les sprites could be cool
            
            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
            {
                playerList.Add(new Sprite_Player(PlayerIndex.Three, assetManager.GetTextures("character_sheet3"), assetManager.GetFont[2], PLAYER3_POS,
                     0, new Point(76, 116), new Point(0, 0), new Point(3, 6), player_speed, 160, null));
            }
            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
            {
                playerList.Add(new Sprite_Player(PlayerIndex.Four, assetManager.GetTextures("character_sheet4"), assetManager.GetFont[2], PLAYER4_POS,
                     0, new Point(76, 116), new Point(0, 0), new Point(3, 6), player_speed, 160, null));
            }
 

        }


    }
}