using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/* we want to display 
 * player scores and time
 * powerups
 * 
 */

namespace ConcessionSession
{

    public class InfoPanel
    {
        //LOCATIONS OF INFO
        Vector2 vP1Score = new Vector2(290, 10);
        Vector2 vP2Score = new Vector2(850, 10);
        //Vector2 vTimeLeft = new Vector2(520, 580);
        Vector2 vTime = new Vector2(580, 5);

        //assets
        AssetManager assetManager;
        List<Sprite_Player> playerList;
        int time;
        
        public InfoPanel(List<Sprite_Player> playerList, AssetManager assetManager, int time)
        {
            this.assetManager = assetManager;
            this.playerList = playerList;
            this.time = time;
        }

        public void Update(GameTime gameTime) 
        {
            time -= gameTime.ElapsedGameTime.Milliseconds;
            if (!playerList[0].getPlayer.isPowerUpActive && playerList[0].getPlayer.Powerup != null)
            {
                playerList[0].getPlayer.Powerup.reveal();
                playerList[0].getPlayer.Powerup.PositionX = vP1Score.X - (playerList[0].getPlayer.Powerup.Dimension.X * playerList[0].getPlayer.Powerup.Scale + 20);
                playerList[0].getPlayer.Powerup.PositionY = vP1Score.Y;
            }
            if (!playerList[1].getPlayer.isPowerUpActive && playerList[1].getPlayer.Powerup != null)
            {
                playerList[1].getPlayer.Powerup.reveal();
                playerList[1].getPlayer.Powerup.PositionX = vP2Score.X - (playerList[1].getPlayer.Powerup.Dimension.X * playerList[1].getPlayer.Powerup.Scale + 20);
                playerList[1].getPlayer.Powerup.PositionY = vP2Score.Y;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects Effect)
        {
            
            //hard coded for now....
            spriteBatch.DrawString(assetManager.GetFont[0], "Score: " + playerList[0].getPlayer.Score.ToString(), vP1Score, Color.Red, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(assetManager.GetFont[0], "Score: " + playerList[1].getPlayer.Score.ToString(), vP2Score, Color.Blue, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            //spriteBatch.DrawString(assetManager.GetFont[0], "Time Left:", vTimeLeft, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(assetManager.GetFont[3], (time/1000).ToString(), vTime, Color.Black, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            if (playerList[0].getPlayer.Powerup != null)
            {
                if (playerList[0].getPlayer.isPowerUpActive == false)

                    playerList[0].getPlayer.Powerup.Draw(gameTime, spriteBatch, Effect, 1.0f);
            }
            if (playerList[1].getPlayer.Powerup != null)
            {
                if (playerList[1].getPlayer.isPowerUpActive == false)

                    playerList[1].getPlayer.Powerup.Draw(gameTime, spriteBatch, Effect, 1.0f);
            }
        }



    }
}
