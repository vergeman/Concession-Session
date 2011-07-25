using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ConcessionSession
{

    public class Sprite_Arena
    {
        Texture2D textureImageTop;
        Texture2D textureImageBottom;
        Vector2 positionTop;
        Vector2 positionBottom;

        public Sprite_Arena(Texture2D textureImageTop, Texture2D textureImageBottom,
            Vector2 positionTop, Vector2 positionBottom)
        {
            this.textureImageTop = textureImageTop;
            this.textureImageBottom = textureImageBottom;
            this.positionTop = positionTop;
            this.positionBottom = positionBottom;
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            //Z-layer 0 (Bottom)
            spriteBatch.Draw(textureImageTop, positionTop,
                new Rectangle(0, -20, textureImageTop.Width, textureImageTop.Height),
                Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.01f);

            //Z-layer 1 (Bottom) last arg
            spriteBatch.Draw(textureImageBottom, positionBottom,
             new Rectangle(0, -20, textureImageBottom.Width, textureImageBottom.Height),
             Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .98f);

        }





        //LOGIC BUG HERE - WE JUMP DOWN ON UPPER LEFT CORNER (in both Game1 & Game2,)
        //this is kinda messy probably want to redo this but works for now
        //suggest constant bounds and refine those equations
        //ISN"T FLEXIBLE ONCE ARENA GRAPHIC GETS CHANGED
        public void Bounds(Sprite_Player thisPlayer, Vector2 old_position)
        {
            float y1;

            //this is the left diagonal boundary checks
         if (thisPlayer.Position.X >= 134 && thisPlayer.Position.X <= 250)
            {
                //this takes care of the top left diagonal
                if (thisPlayer.Position.Y < 440)
                {
                    y1 = -thisPlayer.Position.X * 148 / 116 + 468;
                    
                    if (thisPlayer.Position.Y < y1)
                    {
                        thisPlayer.Position = new Vector2(thisPlayer.Position.X - (thisPlayer.Position.X - old_position.X)*2 / 3, y1);
                    }
                }
                
                //this test takes care of the bottom left diagonal
                //this was more involved because the top and bottom diagonals are not the same size
                else if (thisPlayer.Position.X <= 180)
                {
                    y1 = thisPlayer.Position.X * 84 / 85 + 396;
                    if (thisPlayer.Position.Y > y1) 
                        thisPlayer.Position = new Vector2(thisPlayer.Position.X, y1);
                }
                else if (thisPlayer.Position.Y > 580)
                    thisPlayer.Position = new Vector2(thisPlayer.Position.X, 580);
            }

                //********this is the right diagonal boundary checks
            else if (thisPlayer.Position.X >= 980 && thisPlayer.Position.X <= 1080)
            {
                //this checks the top right boundary
                if (thisPlayer.Position.Y < 440)
                {
                    y1 = thisPlayer.Position.X * 150 / 120 - 1060;
                    if (thisPlayer.Position.Y < y1) 
                        thisPlayer.Position = new Vector2(thisPlayer.Position.X -(thisPlayer.Position.X - old_position.X)/2, y1);
                }
                
                    //this takes care of the bottom right diagonal boundary
                else
                {
                    y1 = -thisPlayer.Position.X * 60 / 100 + 1168;
                    if (thisPlayer.Position.Y > y1) 
                        thisPlayer.Position = new Vector2(thisPlayer.Position.X, y1);

                }
            }

            //This is the top arena boundary
            else if (thisPlayer.Position.Y < 150)
                thisPlayer.Position = new Vector2(thisPlayer.Position.X, 150);

            //this is the bottom arena boundary
            else if (thisPlayer.Position.Y > 580) 
                thisPlayer.Position = new Vector2(thisPlayer.Position.X, 580);

            //this is the left boundary
            else if (thisPlayer.Position.X < 134)
            {
                thisPlayer.Position = new Vector2(134, thisPlayer.Position.Y);
                
                //these two fix the glitch where the character seeed to slide upon reaching the 
                //diagonal boundaries
                if (thisPlayer.Position.Y < 340) 
                    thisPlayer.Position = new Vector2(thisPlayer.Position.X, 340);
                if (thisPlayer.Position.Y > 548) 
                    thisPlayer.Position = new Vector2(thisPlayer.Position.X, 548);
            }
            
                //this is the right boundary
            else if (thisPlayer.Position.X > 1080)
            {
                thisPlayer.Position = new Vector2(1080, thisPlayer.Position.Y);

                //these two fix the glitch where the character seed to slide upon reaching the 
                //diagonal boundaries
                if (thisPlayer.Position.Y < 290) 
                    thisPlayer.Position = new Vector2(thisPlayer.Position.X, 290);
                if (thisPlayer.Position.Y > 540) 
                    thisPlayer.Position = new Vector2(thisPlayer.Position.X, 540);
            }
           

        }

    }
}