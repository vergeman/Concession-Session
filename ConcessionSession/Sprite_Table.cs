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


    public class Sprite_Table : Sprite
    {
        const int OFFSET = 10;    //offset for bounds

        string name;
        AssetManager assetManager;
        Texture2D textureImage;
        Texture2D regularImage;
        Texture2D glowImage;
        SoundEffect grab_sound;


        public Sprite_Table(string name, string textureImage, string glowImage, string grab_sound, AssetManager assetManager,
            Vector2 position, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(assetManager.GetTextures(textureImage), position,
            new Point(assetManager.GetTextures(textureImage).Width, assetManager.GetTextures(textureImage).Height),
            collisionOffset, currentFrame, sheetSize, speed, 0, null)
        {
            this.assetManager = assetManager;

            this.textureImage = assetManager.GetTextures(textureImage);
            this.regularImage = assetManager.GetTextures(textureImage);
            this.glowImage = assetManager.GetTextures(glowImage);

            this.grab_sound = assetManager.GetSounds(grab_sound);
            this.name = name;

        }


        public void Update(List<Sprite_Player> playerList, GameTime gameTime, Rectangle clientBounds)
        {
            base.Texture = regularImage;

            //update GLOW graphic - inefficent but highlights multiplayer problem w/ polling architecture
            //i.e. glow toggles on one table for one player, may be toggled off on considering another player
            foreach (Sprite_Player p in playerList)
            {
                if (p.collisionRect.Intersects(this.collisionRectY))
                {
                    base.Texture = glowImage;
                }
            }

            base.Update(gameTime, clientBounds);
        }


        public void Bounds(Sprite_Player player, Vector2 old_position)
        {
            //we want it to glow (indicatioin to player) but still be able to move
            //problem occurs if we set bounds equal; then we never toggle an intersection as we always
            //move to the old position.  So this is a fudge for visualization.

            if (player.collisionRect.Intersects(
                new Rectangle(this.collisionRectY.X + OFFSET, this.collisionRectY.Y + OFFSET,
                    this.collisionRectY.Width - 2 * OFFSET, this.collisionRectY.Height - 2 * OFFSET)))
            {
                if (player.collisionRect.Right < (this.collisionRectY.Left + OFFSET + player.direction.X + 1))
                    player.PositionX = old_position.X;
                else if (player.collisionRect.Left > (this.collisionRectY.Right - OFFSET + player.direction.X - 1))
                    player.PositionX = old_position.X;
                else if (player.collisionRect.Top < (this.collisionRectY.Y + OFFSET - player.direction.Y + 1))
                    player.PositionY = old_position.Y;
                //player.Position = old_position;
            }

        }


        /* WHY HARDCORD IMAGES TO TABLES?
         * we can generate them dynamically 
         * and overlay the items on each table....oh well
         */


        public void ItemManager(Sprite_Player player)
        {

            //hmm can't figure out why texture2d.name isn't working....that would be nice
            //instead of hardcoding...bleh

            //addItem checks for valid # of items so we don't bother checking here
            if (player.collisionRect.Intersects(this.collisionRectY))
            {

                //name is "table name" passed on instantiation..argh static it be i guess..
                switch (name)
                {

                    case "candy":
                        //string itemName, Texture2D itemPic, SoundEffect itemSound
                        if (IsButtonPressed(player, "X"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("chocolate", player.Position, assetManager));
                        }
                        else if (IsButtonPressed(player, "A"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("gummyBears", player.Position, assetManager));
                        }
                        break;


                    case "nachos":
                        if (IsButtonPressed(player, "X"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("nachosLarge", player.Position, assetManager));
                        }
                        else if (IsButtonPressed(player, "A"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("nachosSmall", player.Position, assetManager));
                        }
                        break;


                    case "popcorn":
                        if (IsButtonPressed(player, "X"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("popcornLarge", player.Position, assetManager));
                        }
                        else if (IsButtonPressed(player, "A"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("popcornMedium", player.Position, assetManager));
                        }
                        else if (IsButtonPressed(player, "B"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("popcornSmall", player.Position, assetManager));
                        }
                        break;


                    case "soda":
                        if (IsButtonPressed(player, "X"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("sodaLarge", player.Position, assetManager));
                        }
                        else if (IsButtonPressed(player, "A"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("sodaMedium", player.Position, assetManager));
                        }
                        else if (IsButtonPressed(player, "B"))
                        {
                            player.getPlayer.AddItem(new Sprite_Item("sodaSmall", player.Position, assetManager));
                        }
                        break;

                    case "garbage":
                        if (player.getPlayer.Carrying.Count() > 0)
                        {
                            if (player.getPlayer.Carrying.Count() == 1 &&
                                (IsButtonPressed(player, "LS") ||
                                player.GetcurrentGPState.Triggers.Left < .5 && player.GetpreviousGPState.Triggers.Left > .5))
                            {
                                player.getPlayer.Carrying.RemoveAt(0);
                            }
                            else if (player.getPlayer.Carrying.Count() == 2)
                            {

                                //weird but ok might as well....
                                //see ordercomparison function for explanation of why bellow code was commented out
                                if (player.GetcurrentGPState.Buttons.RightShoulder == ButtonState.Pressed ||
                                    player.GetcurrentGPState.Triggers.Right > .5)
                                {
                                    //&& previousState.Buttons.RightShoulder == ButtonState.Pressed)
                                    player.getPlayer.Carrying.RemoveAt(1);
                                    PlaySound(true);
                                }
                                if ((IsButtonPressed(player, "LS") ||
                                player.GetcurrentGPState.Triggers.Left < .5 && player.GetpreviousGPState.Triggers.Left > .5))
                                {
                                    player.getPlayer.Carrying.RemoveAt(0);
                                }
                            }
                            else if (player.getPlayer.Carrying.Count() == 3)
                            {
                                if (player.GetcurrentGPState.Buttons.RightShoulder == ButtonState.Pressed ||
                                    player.GetcurrentGPState.Triggers.Right > .5)
                                {
                                    //&& previousState.Buttons.RightShoulder == ButtonState.Pressed)
                                    player.getPlayer.Carrying.RemoveAt(1);
                                    PlaySound(true);
                                }
                                if ((IsButtonPressed(player, "LS") ||
                                player.GetcurrentGPState.Triggers.Left < .5 && player.GetpreviousGPState.Triggers.Left > .5))
                                {
                                    player.getPlayer.Carrying.RemoveAt(0);
                                }
                                if (player.GetcurrentGPState.DPad.Up == ButtonState.Released && player.GetpreviousGPState.DPad.Up == ButtonState.Pressed)
                                {
                                    player.getPlayer.Carrying.RemoveAt(2);
                                }
                            }
                        }

                        break;

                    default:

                        break;

                }

            }

        }


        public bool IsButtonPressed(Sprite_Player player, string button)
        {
            //switches are kinda weird in c#....can't short circuit return?
            bool value = false;
            switch (button)
            {
                case "X":
                    if (player.GetcurrentGPState.Buttons.X == ButtonState.Released &&
                        player.GetpreviousGPState.Buttons.X == ButtonState.Pressed)
                        value = true;
                    break;

                case "Y":
                    if (player.GetcurrentGPState.Buttons.Y == ButtonState.Released &&
                        player.GetpreviousGPState.Buttons.Y == ButtonState.Pressed)
                        value = true;
                    break;

                case "A":
                    if (player.GetcurrentGPState.Buttons.A == ButtonState.Released &&
                        player.GetpreviousGPState.Buttons.A == ButtonState.Pressed)
                        value = true;
                    break;

                case "B":
                    if (player.GetcurrentGPState.Buttons.B == ButtonState.Released &&
                        player.GetpreviousGPState.Buttons.B == ButtonState.Pressed)
                        value = true;
                    break;

                case "LS":
                    if (player.GetcurrentGPState.Buttons.LeftShoulder == ButtonState.Released &&
                        player.GetpreviousGPState.Buttons.LeftShoulder == ButtonState.Pressed)
                        value = true;
                    break;

                default:
                    value = false;
                    break;

            }
            //sound here..cuts down on repeat code puttig here
            PlaySound(value);

            return value;
        }

        public void PlaySound(bool toggle)
        {
            if (toggle)
            {
                this.grab_sound.Play();
            }
        }

        //DUMMY FUNC
        public override Vector2 direction
        {
            get { return position; }
        }
    }
}
