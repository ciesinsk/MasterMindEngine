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

            //Console.WriteLine("All placements, mulitple color usage allowed, none is allowed, color only once");

            //foreach(var p in Enumerators.GetPlacements(EnumOptions.NoneIsAllowed| EnumOptions.ColorOnlyIUsedOnce))
            //{
            //    foreach(var c in p.Code)
            //    {
            //        Console.Write(c + " ");
            //    }
            //    Console.WriteLine();
            //}


            Console.WriteLine("All possible next placements for given hint");
            var p = new Placement(new CodeColors[] { CodeColors.Red, CodeColors.Blue, CodeColors.Yellow, CodeColors.Green });
            var h = new Hint( new HintColors[] { HintColors.Black, HintColors.Black, HintColors.Black, HintColors.Black});

            Console.WriteLine($"Placement: {p}");
            Console.WriteLine($"Hint: {h}");

            var placements = Enumerators.GetPossibleNextPartialPlacements(p, h);  

            foreach(var p1 in placements)
            {
                Console.WriteLine($"{p1}");                
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
