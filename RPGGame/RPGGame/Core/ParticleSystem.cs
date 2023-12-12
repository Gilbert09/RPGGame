using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace RPGGame.Core
{
    public static class ParticleSystem
    {
        private static GraphicsDevice graphics;
        private static Camera camera;
        private static List<Triple<int, Particle, int>> particles = new List<Triple<int, Particle, int>>();
        private static List<Triple<int, Particle, int>> addParticles = new List<Triple<int, Particle, int>>();
        private static List<Triple<int, Particle, int>> removeParticles = new List<Triple<int, Particle, int>>();

        public static void Init(GraphicsDevice graphicsDevice, Camera cam)
        {
            graphics = graphicsDevice;
            camera = cam;
        }

        public static void Update(GameTime gameTime)
        {
            foreach (Triple<int, Particle, int> t in addParticles) particles.Add(t);
            addParticles.Clear();

            foreach (Triple<int, Particle, int> p in particles)
            {
                p.Third--;
                if (p.Third <= 0)
                {
                    removeParticles.Add(p);
                    continue;
                }
                p.Second.Update(gameTime);
            }

            foreach (Triple<int, Particle, int> t in removeParticles) particles.Remove(t);
            removeParticles.Clear();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Triple<int, Particle, int> p in particles)
            {
                p.Second.Draw(spriteBatch, graphics, camera);
            }
        }

        public static void AddParticle(Particle particle)
        {
            addParticles.Add(new Triple<int, Particle, int>(0, particle, 18));
        }

        public static void AddLongLifeParticle(Particle particle)
        {
            addParticles.Add(new Triple<int, Particle, int>(0, particle, 60));
        }
    }

    public class Particle {

        public Vector2 Position { get; set; }
        public Color[] Colors { get; set; }

        private List<Triple<int, Vector2, Vector2>> particles = new List<Triple<int, Vector2, Vector2>>();

        public Particle(Vector2 position, Color[] colors)
        {
            Position = position;
            Colors = colors;
            
            int degrees = (int)Math.Floor((double)360 / (double)colors.Length / 3);

            for (int i = 0; i < colors.Length / 3; i++)
            {
                int raidans = degrees * i;
                particles.Add(new Triple<int, Vector2, Vector2>(i, position, new Vector2((float)Math.Sin(raidans), (float)Math.Cos(raidans))));
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Second += particles[i].Third / 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics, Camera camera)
        {
            for (int i = 0; i < Colors.Length / 3; i++)
            {
                Triple<int, Vector2, Vector2> t = particles[i];
                spriteBatch.Draw(ResourceHolder.SinglePixel, new Vector2(t.Second.X, t.Second.Y) - camera.Position, Colors[i]);
            }
        }
    }

    class Triple<T, R, E>
    {
        public T First { get; set; }
        public R Second { get; set; }
        public E Third { get; set; }

        public Triple(T first, R second, E third) {
            First = first;
            Second = second;
            Third = third;
        }
    }
}
