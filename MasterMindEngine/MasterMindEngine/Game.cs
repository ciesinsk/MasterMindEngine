using System.Text;
using static MasterMindEngine.GameConfig;

namespace MasterMindEngine
{
    /// <summary>
    /// Class that contains the game logic for one game of MasterMind
    /// </summary>
    public class Game
    {
        private Dictionary<int, List<Placement>> m_nextPlacementsCache = new Dictionary<int, List<Placement>>();

        private List<Turn> Turns = new List<Turn>();

        private Placement SecretCode { get; set; } = new Placement(new CodeColors[CodeLength]);

        private bool GameIsRunning { get; set; } = true;

        /// <summary>
        /// Play with a predefined secret code
        /// </summary>
        /// <param name="secretCode"></param>
        public void Play(Placement secretCode)
        {
            SecretCode = secretCode;
            Play();
        }

        /// <summary>
        /// Play the game with the current configuration
        /// </summary>
        public void Play()
        {            
            PrintGameIntroduction();
            
            if(SecretCode.isValid(GameConfig.CodeOptions) == false)
            {
                SecretCode = GetPlacementFromUser("Please enter your secret code:");            
            }            

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
                Hint hintSuggestion = Enumerators.AutoGenerateHint(placement, SecretCode);
                Hint hint;
                if (AutoPlay)
                {
                    Console.WriteLine($"The hint for the last placement is: {hintSuggestion}");
                    turn.Hint = hintSuggestion;
                }
                else
                {
                    var hintIsCorrect = false;
                    while(hintIsCorrect == false)
                    {
                        hint = GetHintFomUser($"Please enter your hint ({hintSuggestion.ToString()}):");
                        if(hint.Equals(hintSuggestion) == false)
                        {
                            Console.WriteLine("The hint does not match the calculated hint. Please try again.");
                        }
                        else
                        {
                            hintIsCorrect = true;
                            turn.Hint = hint;
                        }                    
                    }
                }                
            }            
        }


        private Random m_random = new Random(Guid.Parse("B1FC0F92-5A68-45D5-AE13-62BE095B7016").GetHashCode());

        /// <summary>
        /// Choose next placement randomly
        /// </summary>
        /// <param name="placements"></param>
        /// <returns></returns>
        private Placement ChoseNextPlacement(IEnumerable<Placement> placements) 
        {            
            var placementList = placements.ToList();

            var index = m_random.Next(placementList.Count());

            return placementList[index];
        }

        private IEnumerable<Placement> CalculateCandidatePlacements(EnumOptions enumOptions)
        {
            if (Turns.Count == 0)
            {
                var placements = Enumerators.GetPlacements(enumOptions).ToList();

                return placements;
            }

            IEnumerable<Placement> candidatePlacements;
            var firstTurn = Turns.First();
            candidatePlacements = GetNextPlacementsWithCache(enumOptions, firstTurn);

            Console.WriteLine($"Generated {candidatePlacements.Count()} candidate placements.");

            foreach (var turn in Turns.Skip(1))
            {
                candidatePlacements = GetNextPlacementsWithCache(enumOptions, turn).Intersect(candidatePlacements); 
                // Enumerators.GetPossibleNextPlacements(turn.Placement, turn.Hint, enumOptions).Intersect(candidatePlacements);
            }

            // it can happen that the next move is one of the moves already made - so remove them
            candidatePlacements = candidatePlacements.Except(Turns.Select(t => t.Placement));

            Console.WriteLine($"Keeping {candidatePlacements.Count()} placements that fit each turns and hints.");

            // check the constraints of incomplete hint turns
            candidatePlacements = ApplyColorChangeContraints(candidatePlacements);

            Console.WriteLine($"Keeping {candidatePlacements.Count()} placements that fit color constaints of each turns and hints.");

            // generate and apply additional knowledge
            candidatePlacements = ApplyForbiddenPlacementKnowledge(candidatePlacements);

            Console.WriteLine($"Keeping {candidatePlacements.Count()} placements after applying additional heuristics for each turn.");

            return candidatePlacements;
        }

        private IEnumerable<Placement> GetNextPlacementsWithCache(EnumOptions enumOptions, Turn turn)
        {
            IEnumerable<Placement> candidatePlacements;
            if (m_nextPlacementsCache.ContainsKey(turn.TurnNumber))
            {
                Console.WriteLine($"Using cached next placements for turn {turn.TurnNumber}");
                candidatePlacements = m_nextPlacementsCache[turn.TurnNumber];
            }
            else
            {
                Console.WriteLine($"Calculating next placements for turn {turn.TurnNumber}");
                candidatePlacements = Enumerators.GetPossibleNextPlacements(turn.Placement, turn.Hint, enumOptions);
                m_nextPlacementsCache[turn.TurnNumber] = candidatePlacements.ToList();
            }

            return candidatePlacements;
        }

        private IEnumerable<Placement> ApplyForbiddenPlacementKnowledge(IEnumerable<Placement> candidatePlacements)
        {
            var forbiddenPlacements = Enumerators.GenerateForbiddenPlacements(Turns);            

            // remove forbidden placements from list of candidate placemments
            candidatePlacements = candidatePlacements.Where(p=>p.FitsAny(forbiddenPlacements) == false);            
                      
            return candidatePlacements;
        }

        private IEnumerable<Placement> ApplyColorChangeContraints(IEnumerable<Placement> candidatePlacements)
        {
            foreach(var p in candidatePlacements)
            {
                var noneFittingTurns = Turns.Where(t=>p.FitsColorChangeContstraint(t) == false).ToList();

                if(noneFittingTurns.Any() == false)
                {
                    yield return p;
                }
                else
                {
                    if(p.Equals(SecretCode))
                    {
                        throw new Exception("The secret code is not in the list of candidates #1. Something went wrong.");
                    }
                }
            }                                  
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
