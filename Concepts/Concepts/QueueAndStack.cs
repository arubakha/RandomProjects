using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Concepts
{
    public static class QueueAndStack
    {
        public static void Demo()
        {
            DemoQueue();

            Console.WriteLine();

            DemoStack();

            Console.ReadLine();
        }

        private static void DemoQueue()
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue("first");
            queue.Enqueue("second");
            queue.Enqueue("third");
            queue.Enqueue("fourth");
            queue.Enqueue("fifth");

            Console.WriteLine(string.Join(", ", queue.ToArray<string>()));

            Console.WriteLine();
            Console.WriteLine("Enqueue:");
            foreach (var item in queue)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("Dequeue:");
            while (queue.Count > 0)
            {
                Console.WriteLine(queue.Dequeue());
            }

            Console.WriteLine();
            Console.WriteLine("Result: Queue is straight.");
            Console.WriteLine();
        }

        private static void DemoStack()
        {
            Stack<string> stack = new Stack<string>();
            stack.Push("first");
            stack.Push("second");
            stack.Push("third");
            stack.Push("fourth");
            stack.Push("fifth");

            Console.WriteLine(string.Join(", ", stack.ToArray<string>()));

            Console.WriteLine();
            Console.WriteLine("Push:");
            foreach (var item in stack)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.WriteLine("Pop:");
            while (stack.Count > 0)
            {
                Console.WriteLine(stack.Pop());
            }

            Console.WriteLine();
            Console.WriteLine("Result: Stack is inverted.");
            Console.WriteLine();
        }
    }
}
