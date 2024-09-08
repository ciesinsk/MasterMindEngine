using System.Diagnostics;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    internal class Program
    {
        private const int NUMBER_OF_GAMES = 10;

        static void Main(string[] args)
        {
            Game game = new Game();
            
            Console.WriteLine("Welcome to MasterMind! Press return to start.");
            Console.ReadLine();
            var gameStats = new List<double>();
            var allclock = Stopwatch.StartNew();

            var nog = NUMBER_OF_GAMES;
            while(nog > 0)
            {
                var gameClock = Stopwatch.StartNew();
                GameConfig.SetConfig(10, 11, 5, GameConfig.EnumOptions.NoRestrictions, autoPlay: true);
                //GameConfig.SetConfig(10, 11, 5, EnumOptions.NoRestrictions, autoPlay: false);
                var secretCode = new Placement(new CodeColors[] { CodeColors.Blue, CodeColors.Red, CodeColors.Blue, CodeColors.Green, CodeColors.Red });
                //var secretCode = Enumerators.GetRandomPlacement();
                game.Play(secretCode);
                //game.Play();
                gameClock.Stop();

                var gameDuration = gameClock.Elapsed;
                Console.WriteLine($"Game finished in {gameDuration.TotalSeconds:0.#}");
                gameStats.Add(gameDuration.TotalSeconds);
                nog --;
            }

            Console.WriteLine($"{gameStats.Count} Games finished in {allclock.Elapsed.TotalSeconds:0.#}");
            Console.WriteLine($"(Average per game: {gameStats.Average():0.##})");
        }
    }
}
