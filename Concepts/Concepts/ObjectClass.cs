using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    public class Sample
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Other { get; set; }

        public Sample(int id, string name, int other)
        {
            ID = id;
            Name = name;
            Other = other;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", ID, Name, Other);
        }
        
        public override int GetHashCode()
        {
            Console.WriteLine("GetHashCode: " + this.ToString());
            return ID; //*** Only for demo, since GetHashCode should be the same id two objects are "Equals"; i.e. "return Name.GetHashCode();" would be the right way in this case.
            //return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Console.WriteLine("Equals: " + this.ToString());
            return ((Sample)obj).Name == this.Name;
        }

    }

    public static class ObjectClass
    {
        public static void Demo()
        {
            //1. Different ID -> different GetHashCode -> different objects in a dictionary
            Console.WriteLine("Case 1 (different hash codes, name is irrelevant):");
            Sample x = new Sample(500, "First", 1);
            Sample y = new Sample(6000, "Second", 2);
            Dictionary<Sample, Sample> dictXY = new Dictionary<Sample, Sample>();
            dictXY[x] = x;
            dictXY[y] = y;
            Console.WriteLine("Result - {0} items.", dictXY.Count);
            Console.WriteLine();

            //2. Different ID -> different GetHashCode -> different objects in a dictionary
            Console.WriteLine("Case 1 (different hash codes, name is irrelevant - II):");
            Sample m = new Sample(500, "SAME", 1);
            Sample n = new Sample(6000, "SAME", 2);
            Dictionary<Sample, Sample> dictMN = new Dictionary<Sample, Sample>();
            dictMN[m] = m;
            dictMN[n] = n;
            Console.WriteLine("Result - {0} items.", dictMN.Count);
            Console.WriteLine();

            //3. Same ID -> same GetHashCode -> Equals it used (Equals compares Names) -> different objects in a dictionary
            Console.WriteLine("Case 2 (same hash codes but different names):");
            Sample pp1 = new Sample(222, "First", 1);
            Sample pp2 = new Sample(222, "Second", 2);
            Dictionary<Sample, Sample> dictPP = new Dictionary<Sample, Sample>();
            dictPP[pp1] = pp1;
            dictPP[pp2] = pp2;
            Console.WriteLine("Result - {0} items.", dictPP.Count);
            Console.WriteLine();

            //4. Same ID -> same GetHashCode -> same Names, so Equals is true -> one object in a dictionary
            Console.WriteLine("Case 3 (same hash codes, same names):");
            Sample a = new Sample(222, "First", 1);
            Sample b = new Sample(222, "First", 2);
            Dictionary<Sample, Sample> dictAB = new Dictionary<Sample, Sample>();
            dictAB[a] = a;
            dictAB[b] = b;
            Console.WriteLine("Result - {0} items.", dictAB.Count);
            Console.WriteLine();


            Console.ReadLine();
        }
    }
}
