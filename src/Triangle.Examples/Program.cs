
namespace TriangleNet
{
    using System;
    using TriangleNet.Examples;

    class Program
    {
        static void Main(string[] args)
        {
            Check("Example  1", Example1.Run());
            Check("Example  2", Example2.Run());
            Check("Example  3", Example3.Run());
            Check("Example  4", Example4.Run());
            Check("Example  5", Example5.Run());
            Check("Example  6", Example6.Run());
            Check("Example  7", Example7.Run());
            Check("Example  8", Example8.Run());
            Check("Example  9", Example9.Run());
            Check("Example 10", Example10.Run());
            Check("Example 11", Example10.Run());
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