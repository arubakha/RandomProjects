using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concepts
{
    public static class DelegatesAndEvents
    {
        public delegate void HaveFun();
        public static event HaveFun haveFunEvent;

        private static AsyncCallback acallback = new AsyncCallback(asyncCallBack);
        private static void asyncCallBack(IAsyncResult ar)
        {
            HaveFun del = ar.AsyncState as HaveFun;
            del.EndInvoke(ar);
        }


        public static void Demo()
        {
            //Async calls of Events from an Invocation list.

            foreach (HaveFun del in haveFunEvent.GetInvocationList())
            {
                //(inline)
                del.BeginInvoke(new AsyncCallback(asyncCallBack), del);
                //or (variable)
                AsyncCallback cb = new AsyncCallback(asyncCallBack);
                del.BeginInvoke(cb, del);
                //or (member variable)
                del.BeginInvoke(acallback, del);
                //or (function name)
                del.BeginInvoke(asyncCallBack, del);
                //or (anonymous)
                del.BeginInvoke(delegate(IAsyncResult ar)
                {
                    HaveFun d = ar.AsyncState as HaveFun;
                    d.EndInvoke(ar);
                }, 
                del);
                //or (lambda)
                del.BeginInvoke(ar => 
                {
                    HaveFun d = ar.AsyncState as HaveFun;
                    d.EndInvoke(ar);
                },
                del);
            }
            
            (haveFunEvent.GetInvocationList()[0] as HaveFun).BeginInvoke(null, haveFunEvent.GetInvocationList()[0]);

            haveFunEvent.GetInvocationList().ToList().ForEach(d => (d as HaveFun).BeginInvoke(null, d));

            haveFunEvent.GetInvocationList().ToList().ForEach(delegate(Delegate d)
            {
                (d as HaveFun).BeginInvoke(null, null);
            });
        }

        delegate T SelfApplicable<T>(SelfApplicable<T> self);
        public static void DemoRecursiveLambda()
        {
            // The Y combinator
            SelfApplicable<
              Func<Func<Func<int, int>, Func<int, int>>, Func<int, int>>
            > Y = y => f => x => f(y(y)(f))(x);

            // The fixed point generator
            Func<Func<Func<int, int>, Func<int, int>>, Func<int, int>> Fix =
              Y(Y);

            // The higher order function describing factorial
            Func<Func<int, int>, Func<int, int>> F =
              fac => x => x == 0 ? 1 : x * fac(x - 1);

            // The factorial function itself
            Func<int, int> factorial = Fix(F);

            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine(factorial(i));
            }
        }

    }
}
