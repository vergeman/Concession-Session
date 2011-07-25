using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ConcessionSession
{
    public class AssetManager : Microsoft.Xna.Framework.GameComponent
    {
        Dictionary<string, Texture2D> TextureMap;
        Dictionary<string, SoundEffect> SoundMap;
        Dictionary<string, CustomerAsset> CustomerMap;
        List<Sprite_Item> ItemList;
        List<string> CustomerNames;

        List<SpriteFont> fontList;

        List<Sprite_PowerUp> powerList;

        public AssetManager(Game game)
            : base(game)
        {
            TextureMap = new Dictionary<string, Texture2D>();
            SoundMap = new Dictionary<string, SoundEffect>();
            CustomerMap = new Dictionary<string, CustomerAsset>();
            ItemList = new List<Sprite_Item>();
            CustomerNames = new List<string>();

            fontList = new List<SpriteFont>();
            fontList.Add(Game.Content.Load<SpriteFont>("SpriteFont1"));
            fontList.Add(Game.Content.Load<SpriteFont>("SpriteFont2"));
            fontList.Add(Game.Content.Load<SpriteFont>("SpriteFont3"));
            fontList.Add(Game.Content.Load<SpriteFont>("SpriteFont4"));

            powerList = new List<Sprite_PowerUp>();
        }

        public List<SpriteFont> GetFont
        {
            get { return fontList; }
        }

        public List<Sprite_Item> master_ItemList
        {
            get { return new List<Sprite_Item>(ItemList); }
        }

        public void AddTextures(string file)
        {
            TextureMap.Add(file, Game.Content.Load<Texture2D>(file));
        }


        public void AddSounds(string file)
        {
            SoundMap.Add(file, Game.Content.Load<SoundEffect>(file));
        }

        public void AddCustomerAsset(CustomerAsset asset)
        {
            CustomerMap.Add(asset.Orientation, asset);
        }

        public Texture2D GetTextures(string file)
        {
            if (TextureMap.ContainsKey(file))
            {
                return TextureMap[file];
            }
            else return null;
        }

        public SoundEffect GetSounds(string file)
        {
            if (SoundMap.ContainsKey(file))
            {
                return SoundMap[file];
            }
            else return null;
        }

        public CustomerAsset GetCustomerAsset(string file)
        {
            if (CustomerMap.ContainsKey(file))
            {
                return CustomerMap[file];
            }
            else return null;
        }

        public CustomerAsset GetCustomerAsset(int assetid)
        {
            return CustomerMap.ElementAt(assetid).Value;
        }

        /*BATCH*/
        public void LoadTextures()
        {

            /*ARENA*/
            AddTextures("arenaTop");
            AddTextures("arenaBottom");

            /*PLAYER*/
            AddTextures("character_sheet");
            AddTextures("character_sheet2");
            AddTextures("character_sheet3");
            AddTextures("character_sheet4");
            
            /*TABLES*/
            AddTextures("popcornTable");
            AddTextures("popcornTableGlow");

            AddTextures("candyTable");
            AddTextures("candyTableGlow");

            AddTextures("nachosTable");
            AddTextures("nachosTableGlow");

            AddTextures("sodaTable");
            AddTextures("sodaTableGlow");

            AddTextures("garbage");
            AddTextures("garbageGlow");

            /*ITEMS*/
            AddTextures("chocolate");
            AddTextures("gummyBears");

            AddTextures("nachosSmall");
            AddTextures("nachosLarge");

            AddTextures("popcornSmall");
            AddTextures("popcornMedium");
            AddTextures("popcornLarge");

            AddTextures("sodaSmall");
            AddTextures("sodaMedium");
            AddTextures("sodaLarge");

            /*POWERUPS*/
            AddTextures("mystery_box");
            AddTextures("rollerBlades");
            AddTextures("tray");
            AddTextures("turtle");
            AddTextures("slipnfall");

            //victory sign
            AddTextures("victory");

            //threw this in for Sprite_Customer.Order() generation....
            ItemList.Add(new Sprite_Item("chocolate", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("gummyBears", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("nachosSmall", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("nachosLarge", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("popcornSmall", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("popcornMedium", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("popcornLarge", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("sodaSmall", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("sodaMedium", new Vector2(0, 0), this));
            ItemList.Add(new Sprite_Item("sodaLarge", new Vector2(0, 0), this));

            /*BUBBLES*/
            AddTextures("bubble");
            AddTextures("roundBubbleDown");
            AddTextures("roundBubbleUp");
            AddTextures("bubbleBlue");
            AddTextures("roundBubbleDownBlue");
            AddTextures("roundBubbleUpBlue");
            AddTextures("bubbleRed");
            AddTextures("roundBubbleDownRed");
            AddTextures("roundBubbleUpRed");


            /*CUSTOMER*/
            AddTextures("superman");
            AddTextures("supermanBlue");
            AddTextures("supermanRed");
            AddTextures("homer");
            AddTextures("homerBlue");
            AddTextures("homerRed");
            AddTextures("mrSlave");
            AddTextures("mrSlaveBlue");
            AddTextures("mrSlaveRed");
            AddTextures("donkey");
            AddTextures("donkeyBlue");
            AddTextures("donkeyRed");
            AddTextures("jackie");
            AddTextures("jackieBlue");
            AddTextures("jackieRed");
            AddTextures("arnold");
            AddTextures("arnoldBlue");
            AddTextures("arnoldRed");
            AddTextures("ironMan");
            AddTextures("ironManBlue");
            AddTextures("ironManRed");
            AddTextures("leeloo");
            AddTextures("leelooBlue");
            AddTextures("leelooRed");
            AddTextures("mickey");
            AddTextures("mickeyBlue");
            AddTextures("mickeyRed");
            AddTextures("Scarlett");
            AddTextures("ScarlettBlue");
            AddTextures("ScarlettRed");

            AddTextures("customer_female");
            AddTextures("customer_male");


        }

        public void LoadSounds()
        {
            //TABLE SOUNDS
            AddSounds("candy");
            AddSounds("nachos");
            AddSounds("popcorn");
            AddSounds("soda");
            AddSounds("recycle");
            AddSounds("CashRegister");
            AddSounds("wrong item");

            //CUSTOMER SOUNDS
            AddSounds("donesy");
            AddSounds("jesus");
            AddSounds("doh");
            AddSounds("Woohoo");
            AddSounds("supermanBored");
            AddSounds("upAway");
            AddSounds("winDonkey");
            AddSounds("feelrightDonkey");
            AddSounds("jackieokay");
            AddSounds("thewordsJackie");
            AddSounds("hastalavistaArnold");
            AddSounds("ilbeback");
            AddSounds("IAmIronMan");
            AddSounds("run");
            AddSounds("multipassLeeloo");
            AddSounds("badaboom");
            AddSounds("looseMickey");
            AddSounds("winMickey");
            AddSounds("watchScarlett");
            AddSounds("goodThingsScarlett");

        }
        public string GetCustomerName(int i)
        {
            return CustomerNames[i];
        }
        public void LoadCustomerNames()
        {
            CustomerNames.Add("superman");  //0
            CustomerNames.Add("homer");
            CustomerNames.Add("mrSlave");
            CustomerNames.Add("donkey");
            CustomerNames.Add("jackie");
            CustomerNames.Add("arnold");    //5
            CustomerNames.Add("ironMan");
            CustomerNames.Add("leeloo");
            CustomerNames.Add("mickey");
            CustomerNames.Add("Scarlett");
            
            CustomerNames.Add("customer_male"); //10
            CustomerNames.Add("customer_female");

        }

        public void LoadCustomerAssets()
        {
            //WE SHOULD REALLY ADD AN AXIS: X x Y accordingly
            //string orientation, string bubble_file, Vector2 customer, Vector2 bubble, Vector2 order, Vector2 countdown)
            AddCustomerAsset(new CustomerAsset("topLeft", "bubble", new Vector2(280, 90), new Vector2(20, 80), new Vector2(40, 95), new Vector2(360, 90)));   //new Vector2(350, 50)

            AddCustomerAsset(new CustomerAsset("topCenter", "bubble", new Vector2(668, 90), new Vector2(400, 80), new Vector2(430, 95), new Vector2(748, 90)));  //new Vector2(738, 50)
            AddCustomerAsset(new CustomerAsset("topRight", "bubble", new Vector2(870, 90), new Vector2(930, 80), new Vector2(970, 95), new Vector2(850, 90)));  //new Vector2(900, 50)

            AddCustomerAsset(new CustomerAsset("Left", "roundBubbleDown", new Vector2(26, 370), new Vector2(0, 190), new Vector2(30, 210), new Vector2(0, 420)));
            AddCustomerAsset(new CustomerAsset("Right", "roundBubbleDown", new Vector2(1180, 370), new Vector2(1090, 190), new Vector2(1120, 210), new Vector2(1245, 420)));

            AddCustomerAsset(new CustomerAsset("bottomLeft", "roundBubbleUp", new Vector2(26, 500), new Vector2(0, 550), new Vector2(30, 580), new Vector2(0, 500)));
            AddCustomerAsset(new CustomerAsset("bottomRight", "roundBubbleUp", new Vector2(1180, 500), new Vector2(1090, 550), new Vector2(1120, 580), new Vector2(1245, 500)));
        }        
    }


    //internal class to manage files and vectors assocaited w/ customers
    public class CustomerAsset
    {
        string orientation;     //i.e. topLeft
        string bubble_file;     //bubble used
        Vector2 customer;       //customer location
        Vector2 bubble;         //bubble location
        Vector2 order;          //order location
        Vector2 countdown;      //"patience"

        public CustomerAsset(string orientation, string bubble_file,
            Vector2 customer, Vector2 bubble, Vector2 order, Vector2 countdown)
        {
            this.orientation = orientation;
            this.bubble_file = bubble_file;
            this.customer = customer;
            this.bubble = bubble;
            this.order = order;
            this.countdown = countdown;
        }

        public string Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }
        public string Bubble_file
        {
            get { return bubble_file; }
            set { bubble_file = value; }
        }
        public Vector2 vCustomer
        {
            get { return customer; }
            set { customer = value; }
        }
        public Vector2 vBubble
        {
            get { return bubble; }
            set { bubble = value; }
        }
        public Vector2 vOrder
        {
            get { return order; }
            set { order = value; }
        }
        public Vector2 vCountdown
        {
            get { return countdown; }
            set { countdown = value; }
        }
    }
}
