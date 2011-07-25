using System;

namespace ConcessionSession
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game2 game = new Game2())    //OOP
            //using (Game1 game = new Game1())  //EAP
            {
                game.Run();
            }
        }
    }
}

