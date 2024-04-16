using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMindEngine
{
    public class Game
    {
        public const int MaxTurns = 10;

        public List<Turn> Turns = new List<Turn>();

        private Placement SecretCode { get; set; } = new Placement(new CodeColors[Placement.Size]);

        private bool GameIsRunning { get; set; } = true;

        public void Play()
        {
            PrintGameIntroduction();

            
            var secretCode = GetPlacementFromUser("Please enter your secret code:");

            SecretCode = secretCode;

            Console.WriteLine("The secret code is set. Let's start the game!");


            while (GameIsRunning)
            {
                this.ToString();

                var guess = GetPlacementFromUser("Please enter your guess:");                
                
                var turn = new Turn(guess, Turns.Count +1);
                Turns.Add(turn);                
            }

            //for (int i = 0; i < MaxTurns; i++)
            //{
            //    var turn = new Turn();



            //    turn.Play();
            //    Turns.Add(turn);

            //    if (turn.IsCorrect)
            //    {
            //        Console.WriteLine("You win!");
            //        break;
            //    }
            //}

            //if (!Turns.Last().IsCorrect)
            //{
            //    Console.WriteLine("You lose!");
            //}
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
            }

            return p;
        }

        private static void PrintGameIntroduction()
        {
            Console.WriteLine("Welcome to MasterMind! Try to guess the secret code in 10 turns or less.");
            Console.WriteLine("The code is a 4 digit color code.");
            Console.WriteLine("The colors are:");
            foreach (var color in (CodeColors[])Enum.GetValues(typeof(CodeColors)))
            {
                if (color != CodeColors.None)
                {
                    Console.WriteLine(color);
                }
            }

            Console.WriteLine("You can use each color only once.");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Secret code:");
            sb.AppendLine(SecretCode.ToString());
            sb.AppendLine("Game turns:");
            foreach (var t in Turns)
            {
                sb.AppendLine(t.ToString());
            }

            return sb.ToString();
        }
    }
}
