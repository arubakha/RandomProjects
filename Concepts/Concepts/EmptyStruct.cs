using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Concepts
{
    struct Test
    {
    }

    public static class EmptyStruct
    {
        public static void Demo()
        {
            Test temp;
            Debugger.Break();

            //to avoid warning
            var s = temp.ToString();
            s = s.ToString();
        }
    }
}
