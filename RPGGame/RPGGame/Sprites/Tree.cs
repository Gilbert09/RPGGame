using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework;

namespace RPGGame.Sprites
{
    class Tree : UnpassableSprite, IHurtable
    {
        public Tree(Vector2 position)
            :base (ResourceHolder.Tree, position)
        {
            Health = 100;
        }
    }
}
