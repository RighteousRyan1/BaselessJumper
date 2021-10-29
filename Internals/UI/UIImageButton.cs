﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BaselessJumping.Internals.UI
{
    public class UIImageButton : UIImage
    {
        public UIImageButton(Texture2D texture, float scale) : base(texture, scale)
        {
            Texture = texture;
            Scale = scale;
        }

        public override void Draw()
        {
            base.Draw();

            Base.spriteBatch.Draw(Texture, InteractionBox.Position, null, Color.White * (MouseHovering ? 1.0f : 0.8f), Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }
    }
}
