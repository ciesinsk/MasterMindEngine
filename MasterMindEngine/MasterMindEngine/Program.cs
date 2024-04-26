using System.Diagnostics;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            
            Console.WriteLine("Welcome to MasterMind! Press return to start.");
            Console.ReadLine();

            var nog = 10; // number of games

            while(nog > 0)
            {
                var clock = Stopwatch.StartNew();
                //GameConfig.SetConfig(10, 6, 4, GameConfig.EnumOptions.NoRestrictions);
                GameConfig.SetConfig(10, 11, 5, EnumOptions.NoRestrictions, autoPlay: true);
                //var secretCode = new Placement(new CodeColors[] { CodeColors.Blue, CodeColors.Red, CodeColors.Blue, CodeColors.Green, CodeColors.Red });
                var secretCode = Enumerators.GetRandomPlacement();
                game.Play(secretCode);
                //game.Play();
                clock.Stop();
                Console.WriteLine($"Game finished in {clock.Elapsed}");

                nog --;
            }
        }
    }
}
