﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaselessJumping.Internals.UI
{
    public class UIText : UIElement
    {
        public string Text { get; set; }

        public SpriteFont Font { get; set; }

        public float Scale { get; set; }

        public Color Color { get; set; }

        public UIText(string text, SpriteFont font, Color color, float scale = 1f)
        {
            Text = text;
            Font = font;
            Color = color;
            Scale = scale;
        }

        public override void Draw()
        {
            base.Draw();

            Base.spriteBatch.DrawString(Font, Text, InteractionBox.Position, Color, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}
