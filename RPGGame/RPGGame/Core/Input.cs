using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    public static class Input
    {
        public static KeyboardState KeyboardState { get; set; }

        public static KeyboardState LastKeyboardState { get; set; }

        public static MouseState MouseState { get; set; }

        public static Point MousePosition { get { return new Point(MouseState.X, MouseState.Y); } }

        public static bool KeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        public static bool LeftMouseDown()
        {
            return MouseState.LeftButton == ButtonState.Pressed ? true : false;
        }

        public static bool KeyPress(Keys key)
        {
            if (LastKeyboardState.IsKeyDown(key) && !KeyboardState.IsKeyDown(key)) return true;
            else return false;
        }
    }
}
