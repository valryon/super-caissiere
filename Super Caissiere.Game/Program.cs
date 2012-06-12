using System;

namespace Super_Caissiere
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SCGame game = new SCGame())
            {
                game.Run();
            }
        }
    }
#endif
}

