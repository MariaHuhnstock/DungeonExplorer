using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonExplorerV5.UI.Utils
{
    public static class ConsoleHelper
    {
        // === COLORED WRITE ===
        public static void WriteLineColored(string text, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = old;
        }

        public static void WriteColored(string text, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = old;
        }

        // === SEPARATOR ===
        public static void Separator()
        {
            WriteLineColored(new string('─', 100), ConsoleColor.DarkGray);
        }

        // === WAIT FOR KEY ===
        public static void WaitForKey()
        {
            WriteLineColored("\nPress any key to continue...", ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        // === GENERIC HEALTH BAR ===
        // Used internally by player and enemy bars
        private static void DrawGenericHealthBar(int current, int max, ConsoleColor healthColor)
        {
            int width = 25;

            if (max <= 0) max = 1;
            if (current < 0) current = 0;
            if (current > max) current = max;

            float pct = current / (float)max;
            int filled = (int)(pct * width);

            Console.Write("[");

            var old = Console.ForegroundColor;
            Console.ForegroundColor = healthColor;

            for (int i = 0; i < filled; i++)
                Console.Write("█");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = filled; i < width; i++)
                Console.Write("░");

            Console.ForegroundColor = old;
            Console.Write("]");
        }

        // === PLAYER HEALTH BAR ===
        public static void DrawPlayerHealthBar(int current, int max)
        {
            double pct = (double)current / max;

            ConsoleColor col =
                pct > 0.5 ? ConsoleColor.Green :
                pct > 0.25 ? ConsoleColor.Yellow :
                ConsoleColor.Red;

            WriteColored("HP ", ConsoleColor.Cyan);
            DrawGenericHealthBar(current, max, col);
            WriteLineColored($" {current}/{max}", ConsoleColor.White);
        }

        // === ENEMY HEALTH BAR ===
        public static void DrawEnemyHealthBar(string name, int current, int max)
        {
            double pct = (double)current / max;

            ConsoleColor col =
                pct > 0.5 ? ConsoleColor.Green :
                pct > 0.25 ? ConsoleColor.Yellow :
                ConsoleColor.Red;

            WriteLineColored($"{name} HP", ConsoleColor.Cyan);
            DrawGenericHealthBar(current, max, col);
            WriteLineColored($" {current}/{max}", ConsoleColor.White);
            Console.WriteLine();
        }

        // === UNIVERSAL HEALTH BAR (BACKWARD COMPATIBILITY) ===
        public static void DrawHealthBar(int current, int max)
        {
            DrawPlayerHealthBar(current, max);
        }
    }
}
