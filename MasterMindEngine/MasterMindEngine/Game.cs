﻿using System;
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
                var placement = CalculateNextPlacement();
                var turn = new Turn(placement, Turns.Count + 1);
                Turns.Add(turn);                

                Console.Clear();
                Console.WriteLine(ToString());

                if(placement == SecretCode)
                {
                    Console.WriteLine("I win!");
                    break;
                }                
                var hint = GetHintFomUser("Please enter your hint:");
            }            
        }

        private Placement CalculateNextPlacement()
        {
            var p = new Placement(new CodeColors[Placement.Size]);
            p.Code[0] = CodeColors.Red;
            p.Code[1] = CodeColors.Green;
            p.Code[2] = CodeColors.Blue;
            p.Code[3] = CodeColors.Yellow;

            return p;
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
