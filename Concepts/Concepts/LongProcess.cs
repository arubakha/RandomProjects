using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts
{
    public static class LongProcess
    {
        public static void Demo()
        {
            Console.WriteLine("Before long task.");

            Task task = Task.Factory.StartNew(() => DoSomething());
            task.ContinueWith(t => ContinueWithCallBack());

            Console.WriteLine("After long task.");

            Console.ReadLine();
        }

        private static void ContinueWithCallBack()
        {
            Console.WriteLine("Continue With Callback");
        }

        private static void DoSomething()
        {
            Console.WriteLine("Do Something - Start\t" + DateTime.Now);

            //int count = int.MaxValue;
            //int updateCycles = 100000000;
            int count = 100000;
            int updateCycles = 10;

            int fraction = count / updateCycles;
            for (long l = 0; l < count; l++)
            {
                if (l % updateCycles == 0)
                { 
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("                                      ");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write((l + updateCycles) / updateCycles);
                }
                if (l % (updateCycles / 10) == 0)
                    Console.Write('.');
            }
            Console.Write(Environment.NewLine);

            Console.WriteLine("Do Something - End\t" + DateTime.Now);
        }
    }
}
