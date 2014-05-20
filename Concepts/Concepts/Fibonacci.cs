using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    static class Fibonacci
    {
        public static void Demo()
        {
            int N = 13;

            //Solution 1
            List<int> display = new List<int>();
            foreach (var item in GetFibAsEnumerable(N))
            {
                display.Add(item);
            }
            Console.WriteLine(string.Join(", ", display));

            
            //Solution 2
            var list = GetFibAsList(N);
            Console.WriteLine(string.Join(", ", list));


            Console.ReadLine();
        }

        private static IEnumerable<int> GetFibAsEnumerable(int N)
        {
            int pp = 0;
            int p = 0;

            for (int i = 0; i < N; i++)
            {
                int fib;

                if (i == 0)
                {
                    fib = 0;
                }
                else if (i == 1)
                {
                    fib = 1;
                    p = 1;
                }
                else
                {
                    fib = pp + p;
                    pp = p;
                    p = fib;
                }

                yield return fib;
            }
        }

        private static IList<int> GetFibAsList(int N)
        {
            List<int> list = new List<int>();

            int pp = 0;
            int p = 0;

            for (int i = 0; i < N; i++)
            {
                int fib;

                if (i == 0)
                {
                    fib = 0;
                }
                else if (i == 1)
                {
                    fib = 1;
                    p = 1;
                }
                else
                {
                    fib = pp + p;
                    pp = p;
                    p = fib;
                }

                list.Add(fib);
            }

            return list;
        }
    }
}
