using System.Text;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    /// <summary>
    /// Class that contains the game logic for one game of MasterMind
    /// </summary>
    public class Game
    {
        private List<Turn> Turns = new List<Turn>();

        private Placement SecretCode { get; set; } = new Placement(new CodeColors[CodeLength]);

        private bool GameIsRunning { get; set; } = true;

        public void Play()
        {
            PrintGameIntroduction();
            
            SecretCode = GetPlacementFromUser("Please enter your secret code:");            

            Console.WriteLine("The secret code is set. Let's start the game!");

            while (GameIsRunning)
            {
                var candidates = CalculateCandidatePlacements(GameConfig.CodeOptions);

                if(candidates.Count() == 0)
                {
                    Console.WriteLine("I have no candidates to chose from. You must have made a mistake in your hints.");
                    break;
                }

                if(candidates.Contains(SecretCode) == false)
                {
                    Console.WriteLine("Something went wrong.");
                    break;
                }

                Console.WriteLine($"I have {candidates.Count()} candidates to chose from.");

                var placement = ChoseNextPlacement(candidates);
                
                var turn = new Turn(placement, Turns.Count + 1);
                Turns.Add(turn);                
                
                Console.WriteLine(ToString()); // print current state of the game

                if(placement.Equals(SecretCode))
                {
                    Console.WriteLine("I win!");
                    break;
                }
                else
                {
                    if(Turns.Count >= MaxTurns)
                    {
                        Console.WriteLine("I lost!");
                        Console.WriteLine($"The secret code was: {SecretCode}");
                        Console.Write($"Do you want to see all remaining codes? [y/n]");
                        var answer = Console.ReadLine();
                        if(answer.ToLower() == "y")
                        {
                            foreach (var c in candidates)
                            {
                                Console.WriteLine(c);
                            }
                        }   
                        break;
                    }
                }            
                
                
                // query the hint from the user
                var hint = GetHintFomUser("Please enter your hint:");
                turn.Hint = hint;
            }            
        }

        /// <summary>
        /// Choose next placement randomly
        /// </summary>
        /// <param name="placements"></param>
        /// <returns></returns>
        private Placement ChoseNextPlacement(IEnumerable<Placement> placements) 
        {
            var placementList = placements.ToList();

            var index = new Random().Next(placementList.Count());

            return placementList[index];
        }

        private IEnumerable<Placement> CalculateCandidatePlacements(EnumOptions enumOptions)
        {
            if(Turns.Count == 0)
            {
                var placements = Enumerators.GetPlacements(enumOptions).ToList();

                return placements;
            }


            var firstTurn = Turns.First();

            var relatedNextMoves =  Enumerators.GetPossibleNextPlacements( firstTurn.Placement, firstTurn.Hint, enumOptions);

            foreach (var turn in Turns.Skip(1))
            {
                relatedNextMoves = Enumerators.GetPossibleNextPlacements(turn.Placement, turn.Hint, enumOptions).Intersect(relatedNextMoves);                
            }          

            // it can happen the the next move is one of the moves already made - so remove them
            relatedNextMoves = relatedNextMoves.Except(Turns.Select(t => t.Placement));

            return relatedNextMoves;
        }

        private static Placement GetPlacementFromUser(string prompt)
        {
            if(String.IsNullOrEmpty(prompt) == false)
            {
                Console.WriteLine(prompt);
            }

            Placement? p = null;

            while (p == null)
            {
                try
                {
                    p = Placement.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if(p?.isValid(GameConfig.CodeOptions) == false)
                {
                    Console.WriteLine("The placement is invalid. Please try again.");
                    p = null;
                }
            }
            
            return p;
        }

        private static Hint GetHintFomUser(string prompt)
        {
            if(String.IsNullOrEmpty(prompt) == false)
            {
                Console.WriteLine(prompt);
            }

            Hint? h = null;

            while (h == null)
            {
                try
                {
                    h = Hint.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return h;
        }

        private static void PrintGameIntroduction()
        {
            Console.WriteLine("Welcome to MasterMind! Try to guess the secret code in 10 turns or less.");
            Console.WriteLine($"The code is a {GameConfig.CodeLength} digit color code.");
            Console.WriteLine("The colors are:");

            foreach (var color in GetColorValues())
            {
                Console.WriteLine(color);
            }

            if (GameConfig.CodeOptions.HasFlag(EnumOptions.ColorOnlyUsedOnce))
            {
                Console.WriteLine("You can use each color only once.");
            }
            else
            {
                Console.WriteLine("You can use each color multiple times.");
            }            
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Secret code: ");
            sb.AppendLine(SecretCode.ToString());
            sb.AppendLine("Game turns: ");
            foreach (var t in Turns)
            {
                sb.AppendLine(t.ToString());
            }

            return sb.ToString();
        }
    }
}
