using System;
using System.Linq;

namespace AutoServiceAdmin.Utils
{
    public static class ConsoleUiHelper
    {
        public static void PrintHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string line = new string('═', title.Length + 8);
            Console.WriteLine($"╔{line}╗");
            Console.WriteLine($"║   {title.ToUpper()}   ║");
            Console.WriteLine($"╚{line}╝");
            Console.ResetColor();
        }

        public static void PrintTableHeader(params string[] columns)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('-', columns.Sum(c => c.Length + 4) + columns.Length + 1));
            Console.Write("|");
            foreach (var col in columns)
            {
                Console.Write($" {col.PadRight(18)}|");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', columns.Sum(c => c.Length + 4) + columns.Length + 1));
            Console.ResetColor();
        }

        public static void PrintTableRow(params string[] values)
        {
            Console.Write("|");
            foreach (var val in values)
            {
                Console.Write($" {val.PadRight(18)}|");
            }
            Console.WriteLine();
        }

        public static void PrintTableFooter()
        {
            Console.WriteLine(new string('-', 80));
        }

        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            WaitForInput();
        }

        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            WaitForInput();
        }

        public static void WaitForInput()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}