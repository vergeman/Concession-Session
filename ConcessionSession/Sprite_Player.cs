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
   public class Sprite_Player : Sprite
    {
        PlayerIndex index;      //playerIndex Controls
        Player my_player;       //Player info: initialized w/ default score values (0)
        Vector2 old_position;   //old position via gamepad


        //Vector2 vScore1;
        //Vector2 vScore2;
        //Vector2 vScore3;

        SpriteFont scoreFont;
        
        GamePadState currentGPState;
        GamePadState previousGPState;

        const float SENSITIVITY = 0.15f; //gamepad movement sensitivity
        

        bool score1 = false;
        int time1 = 0;
        float rotate1 = .30f;

        bool score2 = false;
        int time2 = 0;
        float rotate2 = .30f;

        bool score3 = false;
        int time3 = 0;
        float rotate3 = .30f;

       bool scoreBonus = false;
       bool scoreSave = false;
       int timeBonus = 0;
       float rotateBonus = .30f;

       int previousItems = 0;
       int previousOrders = 0;
       int previousSaves = 0;
       int previousBonus = 0;
       int currentBonus = 0;

       int dispTime = 61;


       TimeSpan powerupstart;

        //CONSTRUCTOR (base is Sprite)
        public Sprite_Player(PlayerIndex index, Texture2D textureImage, SpriteFont playerFont,  Vector2 position,
            int collisionOffset, Point frameSize, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, SoundEffect collision_sound)
            : base(textureImage, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, millisecondsPerFrame, collision_sound)
        {
            this.index = index;
            this.scoreFont = playerFont;

            my_player = new Player(this, 0, 0, 0, 0, 0);  
            this.currentGPState = GamePad.GetState(index); 
        }


        public GamePadState GetcurrentGPState
        {
            get { return currentGPState; }
        }

        public GamePadState GetpreviousGPState
        {
            get { return previousGPState; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Player getPlayer
        {
            get { return my_player; }
        }
        public PlayerIndex PlayerIndex
        {
            get { return index; }
        }



        //calculate direction
        public override Vector2 direction
        {
            get
            {
                //Return direction based on input from keys and gamepad
                Vector2 inputDirection = Vector2.Zero;

                //KEYBOARD
                /*
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;
                */

                //GAMEPAD
                
                if (currentGPState.ThumbSticks.Left.X != 0)
                    inputDirection.X += currentGPState.ThumbSticks.Left.X;
                if (currentGPState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y -= currentGPState.ThumbSticks.Left.Y;
                
                return inputDirection * my_player.Speed;
            }
        }




        /*==COMPONENT BASED==*/

        public void Update(GameTime gameTime, Sprite_Arena arena, List<Sprite_Table> tableList, Rectangle clientBounds)
        {            
            //backup position
            old_position = position;

            //update state
            this.previousGPState = currentGPState;
            this.currentGPState = GamePad.GetState(index);
            
            
            if (my_player.Powerup != null)
            {
                

                if (currentGPState.Buttons.Y == ButtonState.Released && previousGPState.Buttons.Y == ButtonState.Pressed)
                {
                    powerupstart = gameTime.TotalGameTime;
                    my_player.Powerup.Powerup.applyEffect(my_player);
                    my_player.isPowerUpActive = true;
                }

                if (my_player.isPowerUpActive)
                {
                    my_player.Powerup.PositionX = this.position.X + (this.Dimension.X / 2 - my_player.Powerup.Dimension.X / 2);
                    my_player.Powerup.PositionY = this.Position.Y + my_player.Powerup.Dimension.Y;
                    if (gameTime.TotalGameTime.Subtract(powerupstart).Seconds == my_player.Powerup.Powerup.TimeLimit)
                    {
                        my_player.Powerup.Powerup.removeEffect(my_player);
                        my_player.Powerup = null;
                        my_player.isPowerUpActive = false;
                    }
                }
            }
            

            //Move le sprite
            position += direction;
            updateAnimation(gameTime, clientBounds);

            //ARENA BOUNDS CHECK
            arena.Bounds(this, old_position);

            //TABLE CHECK
            foreach (Sprite_Table t in tableList)
            {
                //check movement
                t.Bounds(this, old_position);
                

                //add/drop items on appropriate table
                t.ItemManager(this);

            }

            
            //UPDATE PLAYER CLASS (grabbed items, etc)
            my_player.Update();

            updateScoreOutput(dispTime);




            //base class(frame animation) - we deviate from base class here
            //because we want access to controller logic.  animation implemented in updateAnimation();
            //base.Update(gameTime, clientBounds);
        }




        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects Effect)
        {
            Color scoreColor = Color.White;
            foreach (Sprite_Item i in my_player.Carrying)
            {
                i.Draw(gameTime, spriteBatch, Effect, .99f);
            }

            if (score1 == true)
                spriteBatch.DrawString(scoreFont, "+5", new Vector2(this.Position.X + this.collisionRect.Width / 3, this.Position.Y - time1 / 2), scoreColor, rotate1, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);

            if (score2 == true)
                spriteBatch.DrawString(scoreFont, "+5", new Vector2(this.Position.X, this.Position.Y - time2 / 2), scoreColor, rotate2, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);

            if (score3 == true)
                spriteBatch.DrawString(scoreFont, "+5", new Vector2(this.Position.X + this.collisionRect.Width * 2 / 3, this.Position.Y - time3 / 2), scoreColor, rotate3, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);


            //if (scoreBonus == true && timeBonus > dispTime)
            if (scoreBonus == true)
            {
                spriteBatch.DrawString(scoreFont, "Bonus!!", new Vector2(this.Position.X + this.Dimension.X, (this.Position.Y - scoreFont.MeasureString("Bonus!!").Y / 4) - (timeBonus - dispTime) / 2), scoreColor, rotateBonus, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);
                base.Draw(gameTime, spriteBatch, Effect);
                spriteBatch.DrawString(scoreFont, "+" + currentBonus.ToString(), new Vector2(this.Position.X + this.Dimension.X + this.collisionRect.Width / 3, this.Position.Y - (timeBonus - dispTime) / 2), scoreColor, rotateBonus, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);
                base.Draw(gameTime, spriteBatch, Effect);
            }

            else if (scoreSave == true)
            {
                spriteBatch.DrawString(scoreFont, "Save!!", new Vector2(this.Position.X + this.Dimension.X, (this.Position.Y - scoreFont.MeasureString("Bonus and Save!!").Y / 4) - (timeBonus - dispTime) / 2), scoreColor, rotateBonus, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);
                base.Draw(gameTime, spriteBatch, Effect);
                spriteBatch.DrawString(scoreFont, "+" + currentBonus.ToString(), new Vector2(this.Position.X + this.Dimension.X + this.collisionRect.Width / 3, this.Position.Y - (timeBonus - dispTime) / 2), scoreColor, rotateBonus, new Vector2(2, 3), .4f, SpriteEffects.None, 1f);
                base.Draw(gameTime, spriteBatch, Effect);
            }

            
            //Draws the powerup icon on the player body 
            //NEED TO FIX
            
            if (my_player.Powerup != null)
            {
                if(my_player.isPowerUpActive == true)
           
                my_player.Powerup.Draw(gameTime, spriteBatch, Effect, 1.0f);
            }
            
            
            base.Draw(gameTime, spriteBatch, Effect);

            
        }

        public bool selectFrame(float d, float goal)
        {
            if ((goal - SENSITIVITY < d / speed.X) && (d / speed.X < goal + SENSITIVITY))
            {
                return true;
            }
            return false;
        }

        public void updateAnimation(GameTime gameTime, Rectangle clientBounds)
        {

            //Console.Out.WriteLine(direction.ToString());
            //still 
            if (direction.X == 0 && selectFrame(direction.Y, 0))
            {
                this.CurrentFrame = new Point(0, 0);
            }

            else
            {
                //up (fwd)
                if (this.CurrentFrame.Y != 0 && direction.X == 0 && selectFrame(direction.Y, -1))
                {
                    this.CurrentFrame = new Point(0, 0);
                }


                //down (bkwd)
                if (this.CurrentFrame.Y != 1 && direction.X == 0 && selectFrame(direction.Y, 1))
                {
                    this.CurrentFrame = new Point(0, 1);
                }

                //Left
                else if (this.CurrentFrame.Y != 2 && selectFrame(direction.X, -1) && selectFrame(direction.Y, 0))
                {
                    this.CurrentFrame = new Point(0, 2);
                }
                //Right
                else if (this.CurrentFrame.Y != 3 && selectFrame(direction.X, 1) && selectFrame(direction.Y, 0))
                {
                    this.CurrentFrame = new Point(0, 3);
                }
                
                    /*not working
                //Fwd-Diag
                
                else if (this.CurrentFrame.Y != 4 && selectFrame(direction.X, .5f) && selectFrame(direction.Y, .5f))
                {
                    this.CurrentFrame = new Point(4, 0);
                }

                //Back-Diag
                else if (this.CurrentFrame.Y != 5 && selectFrame(direction.X, .5f) && selectFrame(direction.Y, .5f))
                {
                    this.CurrentFrame = new Point(5, 0);
                }
                */
                base.Update(gameTime, clientBounds);
            }
        }



        public void updateScoreOutput(int displayTime)
        {
            if (my_player.ItemsServed > previousItems)
            {
                previousItems = my_player.ItemsServed;
                if (score1 == false)
                {
                    score1 = true;

                }
                else if (score2 == false)
                {
                    score2 = true;

                }
                else if (score3 == false)
                {
                    score3 = true;

                }
            }

            if (score1 == true)
            {
                time1++;
                rotate1 = rotate1 - .01f;
                if (time1 == displayTime)
                {
                    time1 = 0;
                    rotate1 = .30f;
                    score1 = false;
                }
            }
            if (score2 == true)
            {
                time2++;
                rotate2 = rotate2 - .01f;
                if (time2 == displayTime)
                {
                    time2 = 0;
                    rotate2 = .30f;
                    score2 = false;
                }
            }
            if (score3 == true)
            {
                time3++;
                rotate3 = rotate3 - .01f;
                if (time3 == displayTime)
                {
                    time3 = 0;
                    rotate3 = .30f;
                    score3 = false;
                }
            }
            if (my_player.OrdersFinished > previousOrders)
            {
                currentBonus = my_player.BonusPoints - previousBonus;
                previousBonus = my_player.BonusPoints;
                previousOrders = my_player.OrdersFinished;
                if (my_player.CustomersSaved > previousSaves)
                {
                    previousSaves = my_player.CustomersSaved;
                    scoreSave = true;
                }
                else scoreBonus = true;
            }

            if (scoreSave == true)
            {
                timeBonus++;
                rotateBonus = rotateBonus - .01f;
                if (timeBonus == displayTime)
                {
                    timeBonus = 0;
                    rotateBonus = .30f;
                    scoreSave = false;
                }
            }
            else if (scoreBonus == true)
            {
                timeBonus++;
                rotateBonus = rotateBonus - .01f;
                if (timeBonus == displayTime )
                {
                    timeBonus = 0;
                    rotateBonus = .30f;
                    scoreBonus = false;
                }
            }
        }
       
    }
}
