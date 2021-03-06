﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Xml;

namespace Legend
{
    public static class GameContent
    {
        public static Texture2D button;
        public static Texture2D buttonhover;
        public static Texture2D mouse;
        public static Texture2D grass;
        public static Texture2D playermove;
        public static Texture2D playerattack;
        public static Texture2D grassbarrier;
        public static Texture2D foamsword;
        public static Texture2D portal;
        public static Texture2D skyportal;
        public static Texture2D invtxture;
        public static Texture2D selectedinventory;
        public static Texture2D tshirt;
        public static Texture2D fourpixels;
        public static Texture2D logo;
        public static Texture2D glob;
        public static Texture2D hitparticle;
        public static Texture2D slimeparticle;
        public static Texture2D gameovertexture;
        public static Texture2D gold;
        public static Texture2D tooltip;
        public static Texture2D keytxture;
        public static Texture2D keydown;

        public static Song cantina_theme;
        public static Song eightbit;
        public static Song arcade;

        public static SoundEffect typewriter;
        public static SoundEffect spacebar;

        public static SpriteFont normalfont;
        public static SpriteFont descriptionsfont;


        public static void LoadContent(ContentManager content){

            GameContent.mouse = content.Load<Texture2D>("mouse/mouse");
            GameContent.tshirt = content.Load<Texture2D>("armour/tshirt");
            GameContent.foamsword = content.Load<Texture2D>("weapons/foam sword");
            GameContent.normalfont = content.Load<SpriteFont>("fonts/normal");
            GameContent.descriptionsfont = content.Load<SpriteFont>("fonts/descriptions");
            GameContent.button = content.Load<Texture2D>("buttons/button");
            GameContent.buttonhover = content.Load<Texture2D>("buttons/button hover");
            GameContent.grass = content.Load<Texture2D>("background/grass");
            GameContent.playermove = content.Load<Texture2D>("player/playermove");
            GameContent.playerattack = content.Load<Texture2D>("player/playerattack");
            GameContent.grassbarrier = content.Load<Texture2D>("obstacles/grassbarrier");
            GameContent.portal = content.Load<Texture2D>("objects/portal");
            GameContent.skyportal = content.Load<Texture2D>("objects/skyportal");
            GameContent.invtxture = content.Load<Texture2D>("guis/inventory");
            GameContent.selectedinventory = content.Load<Texture2D>("buttons/selectedinventory");
            GameContent.fourpixels = content.Load<Texture2D>("fourpixels");
            GameContent.logo = content.Load<Texture2D>("logo");
            GameContent.glob = content.Load<Texture2D>("monsters/glob");
            GameContent.hitparticle = content.Load<Texture2D>("player/hitparticle");
            GameContent.gameovertexture = content.Load<Texture2D>("guis/gameovertexture");
            GameContent.slimeparticle = content.Load<Texture2D>("monsters/slimeparticle");
            GameContent.gold = content.Load<Texture2D>("items/gold");
            GameContent.tooltip = content.Load<Texture2D>("guis/tooltip");
            GameContent.keytxture = content.Load<Texture2D>("buttons/key");
            GameContent.keydown = content.Load<Texture2D>("buttons/keydown");

            GameContent.eightbit = content.Load<Song>("music/8bit");
            GameContent.cantina_theme = content.Load<Song>("music/cantina_theme");
            GameContent.arcade = content.Load<Song>("music/arcade");
            GameContent.typewriter = content.Load<SoundEffect>("sound effects/typewriter");
            GameContent.spacebar = content.Load<SoundEffect>("sound effects/typewriter-space");
        }

        public static void Loadxml(ContentManager Content)
        {

            Game1.xmlDoc = new XmlDocument();
            Game1.xmlDoc.Load(Game1.saveFile);
        }
    }
}
