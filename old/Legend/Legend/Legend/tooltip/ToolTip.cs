﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Legend.tooltip;

namespace Legend
{
    public class ToolTip
    {
        Texture2D texture;
        public bool enabled = false;
        public Vector2 endposition = new Vector2(10, 270);
        Vector2 targetposition;
        public Vector2 startingposition = new Vector2(10, 360);
        public Vector2 position;
        List<ToolTipObj> objects = new List<ToolTipObj>();

        public Vector2 velocity = new Vector2(0, 0);

        float mass = 100f; //mass
        float k = 1f; //spring constant
        float c = 8f; //dampening coefficient

        public ToolTip(Texture2D texture, List<ToolTipObj> objects)
        {
            this.texture = texture;
            position = startingposition;
            targetposition = startingposition;
            this.objects = objects;
        }

        public void Update(GameTime gameTime)
        {
            //               restoring force                   dampening force
            Vector2 force = k * (targetposition - position) - c * velocity;

            // acceleration changes velocity
            velocity += force / mass;
            // velocity changes position
            position += velocity;

            if (enabled)
            {
                targetposition = endposition;
                k = 1f;
                c = 8f;
            }
            else
            {
                targetposition = startingposition;
                k = 0.4f;
                c = 2f;
            }

            foreach(ToolTipObj obj in objects)
            {
                obj.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //new Point((int)(position.X + .5f), (int)(position.Y + .5f)) != new Point((int)startingposition.X, (int)startingposition.Y)
            if (new Vector2(position.X - startingposition.X, position.Y - startingposition.Y).Length() > 3)
            {
                spriteBatch.Draw(texture, position * Settings.Scale, null, Color.White, 0f, Vector2.Zero, 1.3f * Settings.Scale, SpriteEffects.None, .9f);
                foreach (ToolTipObj obj in objects)
                {
                    obj.Draw(spriteBatch, position);
                }
            }
        }
    }
}