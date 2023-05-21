
namespace TriangleNet
{
    using System;
    using System.Linq;
    using TriangleNet.Examples;

    class Program
    {
        static void Main(string[] args)
        {
            var examples = new IExample[]
            {
                new Example1(),
                new Example2(),
                new Example3(),
                new Example4(),
                new Example5(),
                new Example6(),
                new Example7(),
                new Example8(),
                new Example9(),
                new Example10(),
                new Example11()
            };

            int count = examples.Length;

            if (args.Contains("--help") || args.Contains("-h") || args.Contains("-?"))
            {
                Console.WriteLine("Usage: Triangle.Examples [i [--print]] [--help]");
                return;
            }

            bool print = args.Contains("--print");

            if (args.Length > 0 && int.TryParse(args[0], out int i))
            {
                Check($"Example {i,2}", examples[i - 1].Run(print));
            }
            else
            {
                for (i = 1; i <= count; i++)
                {
                    Check($"Example {i,2}", examples[i - 1].Run(print));
                }
            }
        }

        static void Check(string item, bool success)
        {
            var color = Console.ForegroundColor;

            Console.Write(item + " ");
            Console.ForegroundColor = success ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
            Console.WriteLine(success ? "OK" : "Failed");
            Console.ForegroundColor = color;
        }
    }
}