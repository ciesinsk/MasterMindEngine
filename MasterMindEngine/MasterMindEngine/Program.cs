namespace MasterMindEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("All normal placements");

            //foreach(var p in Enumerators.GetPlacements())
            //{
            //    foreach(var c in p.Code)
            //    {
            //        Console.Write(c + " ");
            //    }
            //    Console.WriteLine();
            //}

            
            //Console.WriteLine("All placements, mulitple color usage allowed");

            //foreach(var p in Enumerators.GetPlacements(EnumOptions.NoRestrictions))
            //{
            //    foreach(var c in p.Code)
            //    {
            //        Console.Write(c + " ");
            //    }
            //    Console.WriteLine();
            //}

            //Console.WriteLine("All placements, mulitple color usage allowed, none is allowed");

            //foreach(var p in Enumerators.GetPlacements(EnumOptions.NoneIsAllowed))
            //{
            //    foreach(var c in p.Code)
            //    {
            //        Console.Write(c + " ");
            //    }
            //    Console.WriteLine();
            //}

            Console.WriteLine("All placements, mulitple color usage allowed, none is allowed, color only once");

            foreach(var p in Enumerators.GetPlacements(EnumOptions.NoneIsAllowed| EnumOptions.ColorOnlyIUsedOnce))
            {
                foreach(var c in p.Code)
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }


            //var p1 = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.None, CodeColors.None, CodeColors.None });

            //foreach(var p in Enumerators.GetPlacements(p1))
            //{
            //    foreach(var c in p.Code)
            //    {
            //        Console.Write(c + " ");
            //    }
            //    Console.WriteLine();
            //}


        }
    }
}
