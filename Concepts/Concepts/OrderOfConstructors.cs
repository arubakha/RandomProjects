using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    class X
    {
        Y y = new Y();
        public X()
        {
            Console.Write("X");
        }
    }

    class Y
    {
        public Y()
        {
            Console.Write("Y");
        }
    }

    class Z : X
    {
        Y y = new Y();
        public Z()
        {
            Console.Write("Z");
        }
    }

    public static class OrderOfConstructors
    {
        public static void Demo()
        {
            new Z();
        }
    }
}
