using System;

namespace RPGGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GameScreen game = new GameScreen())
            {
                game.Run();
            }
        }
    }
#endif
}

