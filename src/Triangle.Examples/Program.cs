
namespace TriangleNet
{
    using System;
    using System.Linq;
    using TriangleNet.Examples;

    class Program
    {
        static void Main(string[] args)
        {
            bool print = args.Contains("--print");

            Check("Example  1", Example1.Run(print));
            Check("Example  2", Example2.Run(print));
            Check("Example  3", Example3.Run(print));
            Check("Example  4", Example4.Run(print));
            Check("Example  5", Example5.Run(print));
            Check("Example  6", Example6.Run(print));
            Check("Example  7", Example7.Run(print));
            Check("Example  8", Example8.Run(print));
            Check("Example  9", Example9.Run());
            Check("Example 10", Example10.Run(print));
            Check("Example 11", Example11.Run(print));
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