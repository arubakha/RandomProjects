using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    public static class DupsFromList
    {
        public static void Demo()
        {
            var ages = new List<int>() { 11, 25, 11, 77, 5, 11, 5, 34};

            //Soution 1 - HashTable
            var dups = new Dictionary<int, int>();
            foreach (var item in ages)
            {
                int count;
                if (dups.TryGetValue(item, out count))
                    count++;
                else
                    count = 1;
                dups[item] = count;
            }
            var listOfDups = dups.Where(kv => kv.Value > 1).Select(kv => kv.Key);
            Console.WriteLine(string.Join(", ", listOfDups));


            //Solution 2 - LINQ
            var dupsLinq = ages
                            .GroupBy(i => i)
                            .Where(g => g.Count() > 1)
                            .Select(g => g.Key);
            Console.WriteLine(string.Join(", ", dupsLinq));


            Console.ReadLine();
        }
    }
}
