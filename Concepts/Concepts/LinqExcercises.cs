using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    public static class LinqExcercises
    {
        public static void DemoListIntToString()
        {
            IList<int> sample = new List<int>() { 2, 4, 4, 4, 5, 5, 7, 9 };

            //var sampleJoined = String.Join<int>(", ", sample);
            var sampleLinqued = sample.Select(i => i.ToString()).Aggregate((a, b) => a.ToString() + ", " + b.ToString());

            Console.WriteLine("Resulting string is '{0}'.", sampleLinqued);
        }

        static public void DemoStandardDeviation()
        {
            IList<int> sample = new List<int>() { 2, 4, 4, 4, 5, 5, 7, 9 };

            double standardDeviation = CalculateStandardDeviation(sample);

            Console.WriteLine("Standard deviation of '{0}' is {1}.", String.Join<int>(", ", sample), standardDeviation);
        }

        private static double CalculateStandardDeviation(IList<int> sample)
        {
            var avg = sample.Average();
            var sum = sample.ToList().Sum(x => Math.Pow(x - avg, 2));
            var stddev = Math.Sqrt(sum / sample.Count);

            var result = Math.Sqrt(sample.ToList().Sum(x => Math.Pow(x - sample.Average(), 2)) / sample.Count);

            return stddev;
        }

        static public void DemoFibonacci()
        {
            //var fibonacci = CalculateFibonacci(10);
            var fibonacci = CalculateFibonacciLazy(10);
            Console.WriteLine("Fibonacci: {0}.", String.Join(", ", fibonacci));
        }

        private static List<int> CalculateFibonacci(int number)
        {
            List<int> fibs = new List<int>();

            int a = -1;
            int b = 1;
            for (int i = 0; i < number; i++)
            {
                int sum = a + b;
                a = b;
                b = sum;
                fibs.Add(sum);
            }

            return fibs;
        }

        private static List<int> CalculateFibonacciLazy(int number)
        {
            List<int> fibs = new List<int>();

            foreach (var item in FibonacciEnumerator(number))
            {
                fibs.Add(item);
            }

            return fibs;
        }

        private static IEnumerable<int> FibonacciEnumerator(int number)
        {
            int a = -1;
            int b = 1;
            for (int i = 0; i < number; i++)
            {
                int sum = a + b;
                a = b;
                b = sum;
                yield return sum;
            }
        }

        public static void DemoEveryOtherElementSelection()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var result = numbers.Select(n => n).Where((w, index) => index % 2 == 0);           
        }
    }
}
