﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EntityFactory
{
    // Small class that shows instructions
    // Also contains some static methods for string manipulation
    internal class Instructions
    {
        private string controls = "Use the ARROW KEYS to move prop, MOUSE controls camera.";
        private string debug = "Press TAB to hide/show collision detection debug.";
        private float controlsX;
        private float debugX;
        private SpriteFont font;

        public Instructions(float screenWidth, SpriteFont font)
        {
            this.font = font;

            Rectangle rec = new Rectangle((int)(screenWidth - 50), 0, 30, 0);
            controlsX = AlignText(controls, font, rec, "right");
            debugX = AlignText(debug, font, rec, "right");
        }
        public static float AlignText(string text, SpriteFont font, Rectangle area, string alignment = "centered")
        {
            float textWidth = font.MeasureString(text).X;

            switch (alignment)
            {
                case "centered":
                    return (float)(area.X + (area.Width - textWidth) * 0.5);
                case "right":
                    return (float)(area.X + (area.Width - textWidth));
                case "left":
                    return area.X;
            }
            return 0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, controls, new Vector2(controlsX, 20), Color.Black);
            spriteBatch.DrawString(font, debug, new Vector2(debugX, 40), Color.Black);
            spriteBatch.End();
        }
    }
}
