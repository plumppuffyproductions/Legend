﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Legend.characters;
using Microsoft.Xna.Framework;
using Legend.levels.functions;
using Legend.weapons;
using Legend.levels.objects;
using Legend.inventory;
using Microsoft.Xna.Framework.Media;
using Legend.particles;
using Legend.tooltip;

namespace Legend.levels.sublevels
{
    public class L1 : Level
    {
        Texture2D _grass;
        Texture2D _grassbarrier;
        Texture2D _foamsword;
        ItemOnFloor sword;
        ParticleSystem particleSystems;
        ToolTip movetip;
        ToolTip pickuptip;
        KeyAnimation movetipkeyanim;
        KeyAnimation pickuptipkeyanim;

        public L1(Texture2D playermove, Texture2D playerattack, Texture2D grass, Texture2D grassbarrier, Texture2D foamsword, Texture2D portal, Song song, Texture2D tooltiptxture, SpriteFont font, Texture2D keytxture, Texture2D keydown)
            :base(playermove, portal, song)
        {
            _grassbarrier = grassbarrier;
            _grass = grass;
            _foamsword = foamsword; 
            sword = new ItemOnFloor(Items.GetItem("Foam Sword"), new Vector2(150, 30), 0.5f);
            particleSystems = new ParticleSystem(sword.item.texture, 0f, 1f, Color.White, new Vector2(-1, 1), new Vector2(-1, 1), new TimeSpan(0, 0, 3), 1f, 1f, 1f, 1f, new Vector2(150, 30), new TimeSpan(1000, 0, 1, 0, 0), true, 0.00002f);
            int dy = 50;
            float layerDepth = .2f;
            for (int i = 0; i < 8; i++)
            {
                grassBarriers.Add(new Sprite(_grassbarrier, new Vector2(75, dy), null, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth, Color.White, _grassbarrier.Width/2, _grassbarrier.Height/2));
                dy += 25;
                layerDepth += .001f;
            }

            dy = 50;
            layerDepth = .2f;
            for (int i = 0; i < 8; i++)
            {
                grassBarriers.Add(new Sprite(_grassbarrier, new Vector2(200, dy), null, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, layerDepth, Color.White, _grassbarrier.Width/2, _grassbarrier.Height/2));
                dy += 25;
                layerDepth += .001f;
            }

            background = new Background(_grass);
            player = new Player(playermove, playerattack, new Vector2(145, 250));
            portalobj = new Portal(portal, new Vector2(155, 250));
            spinning = false;

            //movetip\\
            List<ToolTipObj> objects = new List<ToolTipObj>();
            objects.Add(new Text(.3f, new Vector2(110, 17), .91f, font, "Use WASD to move"));
            List<Key> keys = new List<Key>();
            keys.Add(new Key(.3f, new Vector2(50, 25), .911f, font, 'W', keytxture, keydown));
            keys.Add(new Key(.3f, new Vector2(50, 25), .91f, font, 'A', keytxture, keydown));
            keys.Add(new Key(.3f, new Vector2(50, 25), .91f, font, 'S', keytxture, keydown));
            keys.Add(new Key(.3f, new Vector2(50, 25), .91f, font, 'D', keytxture, keydown));
            ToolTipPlayer tooltipPlayer = new ToolTipPlayer(playermove, playerattack, new Vector2(80, 25), .91f, 1f/5.5f, Direction.Down);
            objects.Add(tooltipPlayer);
            movetipkeyanim = new KeyAnimation(keys, tooltipPlayer);
            foreach (Key thekey in keys)
            {
                objects.Add(thekey);
            }
            movetip = new ToolTip(tooltiptxture, objects);
            movetip.enabled = !movetip.enabled;
            movetip.velocity = new Vector2(0, 0f);

            //pickuptip\\
            List<ToolTipObj> pickupobjects = new List<ToolTipObj>();
            pickupobjects.Add(new Text(.25f, new Vector2(85, 20), .91f, font, "Press K to pick up items and drops"));
            List<Key> pickupkeys = new List<Key>();
            pickupkeys.Add(new Key(.3f, new Vector2(40, 25), .91f, font, 'K', keytxture, keydown));
            pickupkeys.Add(new Key(.3f, new Vector2(40, 25), .91f, font, 'K', keytxture, keydown));
            pickuptipkeyanim = new KeyAnimation(pickupkeys);
            foreach (Key thekey in pickupkeys)
            {
                pickupobjects.Add(thekey);
            }
            pickuptip = new ToolTip(tooltiptxture, pickupobjects);
            pickuptip.enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            sword.Update(gameTime);
            movetip.Update(gameTime);
            pickuptip.Update(gameTime);
            portalobj.Update();
            Portal(gameTime, Color.OrangeRed);
            if (player.DidPickUpWeapon(sword) && sword.State != ItemOnGroundState.GettingPickedUp)
            {
                player.State = PlayerState.Interacting;
                sword.State = ItemOnGroundState.GettingPickedUp;
            }
            if (sword.State == ItemOnGroundState.DoneAnimating)
            {
                sword.State = ItemOnGroundState.OnGround;
                player.State = PlayerState.Moving;
                portalobj.hidden = false;
                for (int i = 0; i < 50; i++)
                {
                    particleSystems.addParticle();
                }
                pickuptip.velocity = new Vector2(0, -4f);
                pickuptip.enabled = false;
            }
            particleSystems.Update(gameTime);
            movetipkeyanim.Update(gameTime);
            pickuptipkeyanim.Update(gameTime);
            if (movetip.enabled && player.State == PlayerState.Moving)
            {
                movetip.velocity = new Vector2(0, -4f);
                movetip.enabled = false;
            }
            if (player.Hitbox.Intersects(sword.HitBox) && portalobj.hidden)
            {
                pickuptip.enabled = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            movetip.Draw(spriteBatch);
            pickuptip.Draw(spriteBatch);
            background.Draw(spriteBatch);
            if (portalobj.hidden)
            {
                sword.Draw(spriteBatch);
            }
            particleSystems.Draw(spriteBatch);
            player.Draw(spriteBatch);
            portalobj.Draw(spriteBatch);
            for (int i = 0; i < grassBarriers.Count; i++)
            {
                grassBarriers[i].Draw(spriteBatch);
            }
        }

    }
}
