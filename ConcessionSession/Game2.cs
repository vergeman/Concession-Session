using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace ConcessionSession
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Game2 : Microsoft.Xna.Framework.Game
    {

        //XNA
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //menu
        string[] menuItems = { "Beginner", "Advanced" };
        int selectedIndex;

        Color normal = Color.Gray;
        Color hilite = Color.Red;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        GamePadState gamePadState;
        GamePadState oldGamePadState;

        Vector2 position;
        float width = 0f;
        float height = 0f;

        SpriteFont menuFont; // = Content.Load<SpriteFont>("menufont");
        SpriteFont titleFont;

        //Sprite Manager
        SpriteManager spriteManager;

        //background
        Texture2D backgroundTexture;
        Texture2D backgroundTextureMenu;

        //States
        enum GameState { Start, Game, End };
        GameState myGameState = GameState.Start;

        //TIME: used for end game
        const int TIMELIMIT = 120 * 1000;  //millsec
        int time = TIMELIMIT;

        //PlayerList for scoring
        List<Sprite_Player> playerList;

        public int Time
        {
            get { return time; }
            set
            {
                time = value;
                if (time <= 0)
                {
                    myGameState = GameState.End;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                }
            }

        }
        //Audio
        Random randomNum = new Random();
        Song alice;
        Song soReal;
        Song dare;
        Song jiggy;
        Song funky;
        Song stromae;
        Song entertainer;

        //good lord..sigh
        Rectangle bigRectangleRect;
        Rectangle bigMenuRectangleRect;
        Texture2D bigRectangleRedTexture;
        Texture2D bigRectangleBlueTexture;
        Texture2D bigRectangleWhiteTexture;
        Texture2D bigRectangleTexture;
        
        

        string whoWon;
        Color winner;

        Vector2 v6 = new Vector2(370, 100);
        Vector2 v7 = new Vector2(400, 620);
        Vector2 vStat1 = new Vector2(100, 200);
        Vector2 vStat2 = new Vector2(700, 200);
        Vector2 vItemsServed1 = new Vector2(100, 270);
        Vector2 vItemsServed2 = new Vector2(700, 270);
        Vector2 vOrdersFinished1 = new Vector2(100, 320);
        Vector2 vOrdersFinished2 = new Vector2(700, 320);
        Vector2 vSaves1 = new Vector2(100, 370);
        Vector2 vSaves2 = new Vector2(700, 370);
        Vector2 vBonusPoints1 = new Vector2(100, 420);
        Vector2 vBonusPoints2 = new Vector2(700, 420);
        Vector2 vScore1 = new Vector2(100, 470);
        Vector2 vScore2 = new Vector2(700, 470);
        Player player1;
        Player player2;
        SpriteFont font;
        SpriteFont fontOver;

        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);

            //Game Window Size
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            //Root Content Directory
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        protected override void Initialize()
        {
            //WE ATTACH THE SPRITEMANAGER COMPONENT - WHERE ALL THE GAME ACTION HAPPENS
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            spriteManager.Enabled = false;
            spriteManager.Visible = false;
          

            base.Initialize();
        }



        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// *Add components here
        /// </summary>
        protected override void LoadContent()
        {
            //SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //menu
            bigMenuRectangleRect = new Rectangle(0, 200, 1280, 520);
            backgroundTextureMenu = Content.Load<Texture2D>("sample start Screen Short");
            menuFont = Content.Load<SpriteFont>("menufont");
            titleFont = Content.Load<SpriteFont>("titlefont");


            //Background
            backgroundTexture = Content.Load<Texture2D>("background");


            //Audio Load
            alice = Content.Load<Song>("alice");
            soReal = Content.Load<Song>("soReal");
            dare = Content.Load<Song>("dare");
            jiggy = Content.Load<Song>("Will Smith - Getting Jiggy With It");
            funky = Content.Load<Song>("Wild Cherry - Play That Funky Music White Boy");
            stromae = Content.Load<Song>("Stromae - Alors On Dance");
            entertainer = Content.Load<Song>("weapon of choice");

            //end screen
            bigRectangleRedTexture = Content.Load<Texture2D>("bigRectangleRed");
            bigRectangleBlueTexture = Content.Load<Texture2D>("bigRectangleBlue");
            bigRectangleWhiteTexture = Content.Load<Texture2D>("bigRectangleWhite");
            bigRectangleRect = new Rectangle(40, 10, 1200, 700);
            font = Content.Load<SpriteFont>("SpriteFont1");
            fontOver = Content.Load<SpriteFont>("SpriteFont2");

            

            MediaPlayer.Volume = .3f;
            SoundEffect.MasterVolume = .3f; //1f
        }




        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update code here

           

            switch (myGameState)
            {
                case GameState.Start:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                    //menu
                    keyboardState = Keyboard.GetState();
                    gamePadState = GamePad.GetState(PlayerIndex.One);

                    MediaPlayer.Stop();

                    if (CheckKey(Keys.Down) || CheckButton(Buttons.DPadDown))
                    {
                        selectedIndex++;
                        if (selectedIndex == menuItems.Length)
                            selectedIndex = 0;
                    }
                    if (CheckKey(Keys.Up) || CheckButton(Buttons.DPadUp))
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                            selectedIndex = menuItems.Length - 1;
                    }
                      if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                    {
                        if (selectedIndex == 0)
                            spriteManager.setLevel(1);
                        else spriteManager.setLevel(5);
                        myGameState = GameState.Game;   //game on
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    
                    oldKeyboardState = keyboardState;
                    oldGamePadState = gamePadState;                  
                    keyboardState = Keyboard.GetState();
                    break;



                case GameState.Game:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        myGameState = GameState.Start;
                    //Audio Select
                    if (!MediaPlayer.State.Equals(MediaState.Playing))
                    {
                        chooseSong(randomNum.Next(1, 8));
                    }

                    playerList = spriteManager.getPlayers();
                    player1 = playerList[0].getPlayer;
                    player2 = playerList[1].getPlayer;


                    break;



                case GameState.End:
                    MediaPlayer.Stop();

                    Restart(playerList);
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;

                    break;

            }

            //exit
            if (playerList != null)
            {
                foreach (Sprite_Player p in playerList)
                {
                    if (GamePad.GetState(p.PlayerIndex).Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                }
            }
                base.Update(gameTime);
            
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            switch (myGameState)
            {
                case GameState.Start:
                //SPLASH SCREEN HERE
                    GraphicsDevice.Clear(Color.White);
                    MeasureMenu();
                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
                    {
                        spriteBatch.Draw(backgroundTextureMenu, bigMenuRectangleRect, Color.White);

                        Vector2 titleposition = new Vector2(350, 10);//

                        spriteBatch.DrawString(titleFont, "Concession Session", titleposition, Color.Blue);
            
                        Vector2 location = position;
                        Color tint;
                        //chosen level variable
                        for (int i = 0; i < menuItems.Length; i++)
                        {
                            if (i == selectedIndex)
                            {
                                tint = hilite;
                                //change to level 1 setting here
                            }
                            else
                            {
                                tint = normal;
                                //change to level 3 setting here
                            }
                            spriteBatch.DrawString(menuFont, menuItems[i],  location, tint);
                            location.Y += menuFont.LineSpacing + 5;
                        }
                    }

                    spriteBatch.End();
                    break;
                   

                case GameState.Game:

                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    //FronttoBack - 0 is back, 1 is front
                    //Layering: base calls are order of Z layers
                    //i.e. SpriteManager will be layered on top of this Draw() function


                    spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
                    {
                        //Draw Background
                        spriteBatch.Draw(backgroundTexture,
                            new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                            null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0f);
                        
                    }
                    

                    spriteBatch.End();
                    break;

                case GameState.End:

                   spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);
                    {
                    
                    EndScreen();
                    }

                    spriteBatch.End();
                    break;
            }
            //pause


            base.Draw(gameTime); //makes call to sub-components, hence they will be drawn later and on "top"

        }

        public void StartScreen()
        {
            
           // spriteBatch.Draw(bigRectangleStartScreenTexture, bigMenuRectangleRect, Color.White);
        }

        public void EndScreen()
        {
            //we can refactor this later - rushing just to get something out



            if (player1.Score > player2.Score)
            {
                whoWon = "PLAYER ONE WINS!!";
                bigRectangleTexture = bigRectangleRedTexture;
                winner = Color.Red;
            }

            else if (player1.Score < player2.Score)
            {
                whoWon = "PLAYER TWO WINS";
                bigRectangleTexture = bigRectangleBlueTexture;
                winner = Color.Blue;
            }
            else
            {
                whoWon = "THE GAME IS TIED!!";
                bigRectangleTexture = bigRectangleWhiteTexture;
                winner = Color.White;
            }
            spriteBatch.Draw(bigRectangleTexture, bigRectangleRect, Color.White);
            spriteBatch.DrawString(fontOver, whoWon, v6, winner, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            spriteBatch.DrawString(font, "Player One Stats:", vStat1, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Player Two Stats:", vStat2, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Items Served: " + player1.ItemsServed, vItemsServed1, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Items Served: " + player2.ItemsServed, vItemsServed2, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Orders Finished: " + player1.OrdersFinished, vOrdersFinished1, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Orders Finished: " + player2.OrdersFinished, vOrdersFinished2, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Customers Saved: " + player1.CustomersSaved, vSaves1, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Customers Saved: " + player2.CustomersSaved, vSaves2, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Bonus Points: " + player1.BonusPoints, vBonusPoints1, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Bonus Points: " + player2.BonusPoints, vBonusPoints2, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Total Score: " + player1.Score, vScore1, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Total Score: " + player2.Score, vScore2, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(font, "Press Start to continue", v7, Color.White,0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

        }



        public void chooseSong(int songNum)
        {
            if (songNum == 1)
                MediaPlayer.Play(alice);
            else if (songNum == 2)
                MediaPlayer.Play(dare);
            else if (songNum == 3)
                MediaPlayer.Play(soReal);
            else if (songNum == 4)
                MediaPlayer.Play(funky);
            else if (songNum == 5)
                MediaPlayer.Play(entertainer);
            else if (songNum == 6)
                MediaPlayer.Play(stromae);
            else if (songNum == 7)
                MediaPlayer.Play(jiggy);
        }

        //Restart game
        public void Restart(List<Sprite_Player> playerList)
        {
            
            foreach (Sprite_Player p in playerList)
            {
                if (GamePad.GetState(p.PlayerIndex).Buttons.Start == ButtonState.Pressed)
                {
                    Time = TIMELIMIT;
                    Initialize();
                    myGameState = GameState.Start;
                    return;
                }
            }
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                    oldKeyboardState.IsKeyDown(theKey);
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if (selectedIndex < 0)
                    selectedIndex = 0;
                if (selectedIndex >= menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = menuFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;
                height += menuFont.LineSpacing + 5;
            }
            position = new Vector2(
                        ( this.Window.ClientBounds.Width - width) / 2, 
                        (200 - height));//
        }

        
        private bool CheckButton(Buttons button)
        {
            return gamePadState.IsButtonUp(button) &&
            oldGamePadState.IsButtonDown(button);
        }


    }
}