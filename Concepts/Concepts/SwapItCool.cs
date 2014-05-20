using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    public static class SwapItCool
    {
        public static void Demo()
        {
            int First = 221;
            int Second = 100;

            Console.WriteLine(First);
            Console.WriteLine(Second);
            Console.WriteLine("Swapping...");

            First = First ^ Second;
            Second = First ^ Second;
            First = First ^ Second;

            Console.WriteLine(First);
            Console.WriteLine(Second);
            Console.ReadLine();
        }
    }
}
