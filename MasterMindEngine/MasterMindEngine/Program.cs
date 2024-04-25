using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            //GameConfig.SetConfig(10, 6, 4, GameConfig.EnumOptions.NoRestrictions);
            GameConfig.SetConfig(10, 8, 5, EnumOptions.NoRestrictions, autoPlay: true);
            game.Play();

        }
    }
}
