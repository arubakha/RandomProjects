using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace TCPSubscriber
{
    class Program
    {
        static List<string> data = new List<string>();

        static void Main(string[] args)
        {
            TCPSubscriber subscriber = new TCPSubscriber();
            subscriber.Start();
        }
        
    }
}
