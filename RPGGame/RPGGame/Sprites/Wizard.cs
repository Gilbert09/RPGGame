using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGGame.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RPGGame.Entities;

namespace RPGGame.Sprites
{
    public class Wizard : PlayableCharacter, IHurtable
    {
        public int Level { get; set; }
        public int MaxHealth { get; set; }
        public int MaxExperiance { get; set; }
        public int Mana { get; set; }
        public int MaxMana { get; set; }

        public int Defense { get; set; }
        public int Wisdom { get; set; }
        public int Agility { get; set; }

        private float shootRate = 20;
        private float combatRate = 120;

        private bool shootCoolDown = false;
        private float shootCoolDownRate;

        private bool specialShootCoolDown = false;
        private float specialShootCoolDownRate;

        private bool inCombat = false;
        private float inCombatCoolDownRate;

        private float bulletSpeed = 2f;
        private int frame = 0;

        public Wizard(Texture2D texture, int frameCount, int xStart, int yStart, int frameWidth, int frameHeight, Vector2 position, int startingDirection, AnimationState state, bool incStill, Camera camera)
            : base(texture, frameCount, xStart, yStart, frameWidth, frameHeight, position, startingDirection, state, incStill, camera)
        {
            shootCoolDownRate = specialShootCoolDownRate = shootRate;
            inCombatCoolDownRate = combatRate;

            speed = 0.6f;
            Health = MaxHealth = 150;
            Mana = MaxMana = 100;
            Damage = 20;
            Level = 1;
            Experiance = 0;
            MaxExperiance = 100;

            Defense = 0;
            Wisdom = 0;
            Agility = 0;
            
            if (Settings.GetValue("health") != null) Health = float.Parse(Settings.GetValue("health"));
            if (Settings.GetValue("maxhealth") != null) MaxHealth = Convert.ToInt32(Settings.GetValue("maxhealth"));
            if (Settings.GetValue("exp") != null) Experiance = Convert.ToInt32(Settings.GetValue("exp"));
            if (Settings.GetValue("maxexp") != null) MaxExperiance = Convert.ToInt32(Settings.GetValue("maxexp"));
            if (Settings.GetValue("mana") != null) Mana = Convert.ToInt32(Settings.GetValue("mana"));
            if (Settings.GetValue("maxmana") != null) MaxMana = Convert.ToInt32(Settings.GetValue("maxmana"));
            if (Settings.GetValue("damage") != null) Damage = float.Parse(Settings.GetValue("damage"));
            if (Settings.GetValue("level") != null) Level = Convert.ToInt32(Settings.GetValue("level"));
            if (Settings.GetValue("x") != null) X = float.Parse(Settings.GetValue("x"));
            if (Settings.GetValue("y") != null) Y = float.Parse(Settings.GetValue("y"));
            if (Settings.GetValue("defense") != null) Defense = Convert.ToInt32(Settings.GetValue("defense"));
            if (Settings.GetValue("wisdom") != null) Wisdom = Convert.ToInt32(Settings.GetValue("wisdom"));
            if (Settings.GetValue("agility") != null) Agility = Convert.ToInt32(Settings.GetValue("agility"));
        }

        public override void Collided(Sprite sprite)
        {
            if (sprite is EnemyProjectile)
            {
                inCombat = true;
                inCombatCoolDownRate = combatRate;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            frame++;
            if (Experiance >= MaxExperiance) LevelUp();

            if (shootCoolDown)
            {
                shootCoolDownRate--;
                if (shootCoolDownRate <= 0)
                {
                    shootCoolDownRate = shootRate - ((shootRate / 100) * Agility);
                    shootCoolDown = false;
                }
            }

            if (specialShootCoolDown)
            {
                specialShootCoolDownRate--;
                if (specialShootCoolDownRate <= 0)
                {
                    specialShootCoolDownRate = shootRate - ((shootRate / 100) * Agility);
                    specialShootCoolDown = false;
                }
            }

            if (inCombat)
            {
                inCombatCoolDownRate--;
                if (inCombatCoolDownRate <= 0)
                {
                    inCombatCoolDownRate = combatRate - ((float)((float)combatRate / 100f) * Agility);
                    inCombat = false;
                }
            }
            else
            {
                if ((int)(frame % (10 - ((float)(10f / 100f) * Wisdom))) == 0)
                {
                    Health = MathHelper.Clamp(Health + 1, 0, MaxHealth);
                    Mana = (int)MathHelper.Clamp(Mana + 1, 0, MaxMana);
                }
            }

            if (Input.LeftMouseDown() && !shootCoolDown)
            {
                Vector2 movement = (new Vector2(Input.MousePosition.X, Input.MousePosition.Y - (Height * 8 / 2)) + (camera.Position * 8)) - new Vector2(X * 8, Y * 8);
                if (movement != Vector2.Zero) movement.Normalize();

                int newDamage = (int)(Damage + ((Damage / 100) * Wisdom));
                Projectile projectile = new Projectile(ResourceHolder.BlueBolt, new Vector2(X + (Width / 2), Y + (Height / 2)), movement * bulletSpeed, 60, true, this, newDamage);
                Children.Add(projectile);
                SpriteManager.AddSprite(projectile);

                inCombat = true;
                inCombatCoolDownRate = combatRate - ((float)((float)combatRate / 100f) * Agility);
                shootCoolDown = true;
            }

            if (Input.KeyDown(Keys.Space) && !specialShootCoolDown && Mana >= 60)
            {
                Mana -= 60;
                Color[] colors = ResourceHolder.TextureColorData(ResourceHolder.ManaSpecial);
                ParticleSystem.AddLongLifeParticle(new Particle(Position + new Vector2(Width / 2, Height / 2), colors));
                int newDamage = (int)(Damage + ((Damage / 100) * Wisdom));
                foreach (Sprite s in SpriteManager.OnScreenEnemies(camera))
                {
                    s.Hurt(newDamage);

                    if (s.Health <= 0)
                    {
                        SpriteManager.RemoveSprite(s);
                        Color[] enemyColors = ResourceHolder.TextureColorData(s.Texture);
                        ParticleSystem.AddParticle(new Particle(s.Position + new Vector2(s.Width / 2, s.Height / 2), enemyColors));
                        if (s is Enemy)
                        {
                            Enemy e = (Enemy)s;
                            Experiance += e.ExperiancePoints;
                            FloatingText.AddExp("+" + e.ExperiancePoints + " exp", new Vector2(X + (Width / 2) - 3f, Y));
                        }
                    }
                }
                inCombat = true;
                inCombatCoolDownRate = combatRate - ((float)((float)combatRate / 100f) * Agility);
                specialShootCoolDown = true;
            }
        }

        public void LevelUp()
        {
            Color[] colors = ResourceHolder.TextureColorData(ResourceHolder.LevelUp);
            ParticleSystem.AddParticle(new Particle(Position + new Vector2(Width / 2, Height / 2), colors));
            FloatingText.AddLevelUp("Level Up!", Position + new Vector2(Width / 2, Height / 2));

            Experiance -= MaxExperiance;
            MaxExperiance += (int)((MaxExperiance / 100f) * 50f);
            Health = MaxHealth = (int)(MaxHealth + ((float)((float)MaxHealth / 100f) * 15));
            Mana = MaxMana = (int)(MaxMana + ((float)((float)MaxMana / 100f) * 15));
            Damage = (int)(Damage + ((float)((float)Damage / 100f) * 5));
            Level++;
            Defense++;
            Wisdom++;
            Agility++;
            if (Experiance >= MaxExperiance) LevelUp();
        }

        public void Reload(bool loadSettings)
        {
            if (loadSettings)
            {
                if (Settings.GetValue("health") != null) Health = float.Parse(Settings.GetValue("health"));
                else Health = 150;
                if (Settings.GetValue("maxhealth") != null) MaxHealth = Convert.ToInt32(Settings.GetValue("maxhealth"));
                else MaxHealth = 150;
                if (Settings.GetValue("exp") != null) Experiance = Convert.ToInt32(Settings.GetValue("exp"));
                else Experiance = 0;
                if (Settings.GetValue("maxexp") != null) MaxExperiance = Convert.ToInt32(Settings.GetValue("maxexp"));
                else MaxExperiance = 100;
                if (Settings.GetValue("mana") != null) Mana = Convert.ToInt32(Settings.GetValue("mana"));
                else Mana = 100;
                if (Settings.GetValue("maxmana") != null) MaxMana = Convert.ToInt32(Settings.GetValue("maxmana"));
                else MaxMana = 100;
                if (Settings.GetValue("damage") != null) Damage = float.Parse(Settings.GetValue("damage"));
                else Damage = 20;
                if (Settings.GetValue("level") != null) Level = Convert.ToInt32(Settings.GetValue("level"));
                else Level = 1;
                if (Settings.GetValue("x") != null) X = float.Parse(Settings.GetValue("x"));
                else X = 1024;
                if (Settings.GetValue("y") != null) Y = float.Parse(Settings.GetValue("y"));
                else Y = 1024;
                if (Settings.GetValue("defense") != null) Defense = Convert.ToInt32(Settings.GetValue("defense"));
                else Defense = 0;
                if (Settings.GetValue("wisdom") != null) Wisdom = Convert.ToInt32(Settings.GetValue("wisdom"));
                else Wisdom = 0;
                if (Settings.GetValue("agility") != null) Agility = Convert.ToInt32(Settings.GetValue("agility"));
                else Agility = 0;

                shootCoolDownRate = specialShootCoolDownRate = shootRate;
                inCombatCoolDownRate = combatRate;
                speed = 0.6f;
            }
            else
            {
                shootCoolDownRate = specialShootCoolDownRate = shootRate;
                inCombatCoolDownRate = combatRate;

                speed = 0.6f;
                Health = MaxHealth = 150;
                Mana = MaxMana = 100;
                Damage = 20;
                Level = 1;
                Experiance = 0;
                MaxExperiance = 100;
                Position = new Vector2(1024, 1024);
                Defense = 0;
                Wisdom = 0;
                Agility = 0;
            }
            
            camera.LockToSprite(this);
        }
    }
}