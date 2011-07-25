using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ConcessionSession
{
    /*Abstract Class Sprite:
     * inheriting classes: *_Sprite
     * contains basic "Sprite" notions:
     * drawing, updating, moving & animation (we don't use spritesheets..yet)
     * 
     */

    public abstract class Sprite
    {
        //region declarations
        //image
        protected Texture2D texture;

        //movement
        protected Vector2 position;
        protected Vector2 speed;

        //collision
        int collisionOffset;        //X/Y specific offset
        protected Point frameSize;  //size of sprite

        //Animation (we'll likely use this with the player, and if there's time possibly for other sprites)
        Point currentFrame;         //spritesheet: index of frame
        Point sheetSize;            //spritesheet: size of sheet (i.e. (2, 5) = 2 horiztonal, 5 down, 10 total animations
        int timeSinceLastFrame = 0; //spritesheet: toggle to indicate movement to next spritesheet
        int millisecondsPerFrame;   //spritesheet: duration of single frame before moving to next in spritesheet
        const int defaultMillisecondsPerFrame = 16; //default aprox. with 60fps
        
        protected float scale = 1;    //for rendering: scale just in case

        //#endregion
        //float zPosition;

        //CONSTRUCTOR
        public Sprite(Texture2D texture, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, SoundEffect collision_sound)
        {
            this.texture = texture;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        //Audio cue name for collisions
        public string collisionCueName { get; private set; }

        /*==ACCESSORS==*/
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        //we will override this in the instance classes
        public abstract Vector2 direction
        {
            get;
        }

        public float Scale { get { return scale; } set { scale = value; } }
            
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //OK this is ugly but i don't want to think about it..
        public float PositionX
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float PositionY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }


        public int CollisionOffset
        {
            get { return collisionOffset; }
            set { collisionOffset = value; }
        }

        public Point Dimension
        {
            get { return new Point(frameSize.X, frameSize.Y); }
        }

        public Point CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        /*
        public void setSpeed(float newspeed)
        {
            speed = newspeed;
        }

        public void resetSpeed()
        {
            speed = originalSpeed;
        }
        */

        /*==POSITION & COLLISION==*/

        //ORIGINAL RECT
        public Rectangle collisionRect
        {
           /*
            * rectangles start at upper left
            * 1st/2nd args: location we shrink & shift x,y accordingly (to make smaller)
            * 3rd & 4th args specify "size" rectangle
           */
            get
            {
                return new Rectangle(
                (int)(position.X + (collisionOffset * scale)),
                (int)(position.Y + (collisionOffset * scale)),
                (int)((frameSize.X - (collisionOffset * 2)) * scale),
                (int)((frameSize.Y - (collisionOffset * 2)) * scale));
            }
        }
        //collsion rectangle set to image
        public Rectangle collisionRectImage
        {
            get
            {
                return new Rectangle(
                (int)(position.X),
                (int)(position.Y),
                (int)(frameSize.X),
                (int)(frameSize.Y));
            }
        }

        //DEPTH ADJUSTED Y (this is how we maintain our perspective via collision
        public Rectangle collisionRectY
        {
            get
            {
                
                return new Rectangle(
                (int)(position.X + (0 * scale)),
                (int)(position.Y + (collisionOffset * scale)),
                (int)((frameSize.X - (0 * 2)) * scale),
                (int)((frameSize.Y - (collisionOffset * 2)) * scale));
            }
        }
        




        /*==COMPONENT BASED==*/

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //Update animation frame
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;

                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    //++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }

        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects Effect, float zPosition)
        {
            //Draw the sprite (post constructor)
            spriteBatch.Draw(texture, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                scale, Effect, zPosition);
            //to layer Zdepth: position.Y/GraphicsDeviceManager.DefaultBackBufferHeight
            //the secondary args are used for image rotation and stuff...we probably won't use them.

        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects Effect)
        {
            //Draw the sprite (post constructor)
            spriteBatch.Draw(texture, position,
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, Effect, position.Y / GraphicsDeviceManager.DefaultBackBufferHeight);
            //to layer Zdepth: position.Y/GraphicsDeviceManager.DefaultBackBufferHeight
            //the secondary args are used for image rotation and stuff...we probably won't use them.

        }

        //We use this one for autoscaling while maintaining z-order
        //myRectangle is destination
        //2nd args: destination, 3rd arg: source

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle myRectangle)
        {
            spriteBatch.Draw(texture, 
                new Rectangle((int)Position.X, (int)Position.Y, myRectangle.Width, myRectangle.Height),
                new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);


        }

    }
}
