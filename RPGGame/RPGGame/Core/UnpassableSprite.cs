using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RPGGame.Core
{
    class UnpassableSprite : Sprite
    {
        public UnpassableSprite(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            Passable = false;
        }
    }
}
