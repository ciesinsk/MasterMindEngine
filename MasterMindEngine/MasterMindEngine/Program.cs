namespace MasterMindEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            foreach(var p in Enumerators.GetPlacements())
            {
                foreach(var c in p.Code)
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
