using System;
using System.Collections.Generic;
using System.Text;

namespace Pharmacy
{
    internal class ConsoleEx
    {
        public static void WriteLine(ConsoleColor color, string message, params object[] arg)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message, arg);
            Console.ForegroundColor = oldColor;
        }

        public static void Write(ConsoleColor color, string message, params object[] arg)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message, arg);
            Console.ForegroundColor = oldColor;
        }

    }
}
