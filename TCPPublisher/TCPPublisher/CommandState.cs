using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace TCPPublisher
{
    public class CommandState
    {
        public byte[] Data { get; set; }
        public NetworkStream NetworkStream { get; set; }
        public StringBuilder Command { get; set; }
        public bool IsBackSpaced { get; set; }
        public List<char> ManualCommandCharacters { get; private set; }

        public CommandState(int size)
        {
            Data = new byte[size];
            Command = new StringBuilder();
            IsBackSpaced = false;
            ManualCommandCharacters = new List<char>();
        }

        public void Clear()
        {
            Command.Clear();
            ManualCommandCharacters.Clear();
        }
    }
}
