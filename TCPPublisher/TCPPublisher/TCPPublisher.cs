using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

namespace TCPPublisher
{
    public class TCPPublisher
    {
        private object commandLock = new object();
        private IPEndPoint endPoint;

        public TCPPublisher(IPAddress ipAddress, int port)
        {
            endPoint = new IPEndPoint(ipAddress, port);
        }

        public void Start()
        {
            TcpListener tcpListener = new TcpListener(endPoint);
            tcpListener.Start();

            while (true)
            {
                Socket socket = tcpListener.AcceptSocket();
                Task.Factory.StartNew(() => HandleListener(socket));
            }
        }

        private void HandleListener(Socket socket)
        {
            using (socket)
            {
                using (NetworkStream networkStream = new NetworkStream(socket))
                {
                    StartCommunicationLoop(networkStream);
                }
            }
        }

        private void StartCommunicationLoop(NetworkStream networkStream)
        {
            CommandState commandState = new CommandState(5);
            commandState.NetworkStream = networkStream;

            //byte[] prompt = Encoding.ASCII.GetBytes("Please say a command:\n\r");
            //networkStream.Write(prompt, 0, prompt.Length);

            string command;
            bool backspaced;
            while (true)
            {
                //Get command
                lock (commandLock)
                {
                    command = commandState.Command.ToString();
                    backspaced = commandState.IsBackSpaced;
                    commandState.IsBackSpaced = false;
                }

                //Process command
                if (command == "end")
                {
                    break;
                }
                else if (backspaced)
                {
                    SendNewLine(commandState);
                }
                else if (command == "start")
                {
                    SendData(commandState);
                }

                //Keep listening for new commands
                if (commandState.NetworkStream.DataAvailable)
                {
                    ReadData(commandState);
                }
            }
        }

        private void ReadData(CommandState commandState)
        {
            try
            {
                IAsyncResult ar = commandState.NetworkStream.BeginRead(commandState.Data, 0, commandState.Data.Length, ReadCallback, commandState);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception thrown: {0}", e.Message);
            }
        }

        private void ReadCallback(IAsyncResult ar)
        {
            CommandState commandState = ar.AsyncState as CommandState;
            if (commandState != null)
            {
                int bytesRead = 0;
                try
                {
                    bytesRead = commandState.NetworkStream.EndRead(ar);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception thrown: {0}", e.Message);
                    return;
                }

                string data = Encoding.ASCII.GetString(commandState.Data, 0, bytesRead);
                ProcessReadData(commandState, bytesRead, data);

                
            }
        }

        private void ProcessReadData(CommandState commandState, int bytesRead, string data)
        {
            lock (commandLock)
            {
                if (IsEndOfManualCommand(data))
                {
                    //end of manual command: chars -> command
                    commandState.Command.Clear();
                    commandState.Command.Append(commandState.ManualCommandCharacters.ToArray());
                    commandState.ManualCommandCharacters.Clear();
                }
                else
                {
                    if (IsSingleCharacter(bytesRead))
                    {
                        char enteredCharacter = System.Char.Parse(data);
                        if (IsLetter(enteredCharacter))
                        {
                            commandState.ManualCommandCharacters.Add(enteredCharacter);
                        }
                        else if (IsBackspace(enteredCharacter))
                        {
                            commandState.IsBackSpaced = true;
                        }
                    }
                    else
                    {
                        //full command
                        commandState.Command.Clear();
                        commandState.Command.Append(data);
                    }
                }
            }
        }

        private static bool IsBackspace(char enteredCharacter)
        {
            return enteredCharacter == (char)KeyInterop.VirtualKeyFromKey(Key.Back);
        }

        private bool IsLetter(char enteredCharacter)
        {
            return Char.IsLetter(enteredCharacter);
        }

        private bool IsSingleCharacter(int bytesRead)
        {
            return bytesRead == 1;
        }

        private bool IsEndOfManualCommand(string data)
        {
            return data == "\r\n";
        }

        private void SendData(CommandState commandState)
        {
            lock (commandLock)
            {
                commandState.Clear();
            }

            for (int i = 0; i < 10; i++)
            {
                WriteLine(commandState, string.Format("Data string {0}.", i));
            }
        }

        private void SendNewLine(CommandState commandState)
        {
            lock (commandLock)
            {
                commandState.Clear();
            }

            WriteLine(commandState, "");
        }

        private void WriteLine(CommandState commandState, string line)
        {
            try
            {
                if (line != "|")
                    line = line + "|";

                byte[] lineBytes = Encoding.ASCII.GetBytes(line);
                commandState.NetworkStream.BeginWrite(lineBytes, 0, lineBytes.Length, WriteCallback, commandState);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception thrown: {0}", e.Message);
            }
        }

        private void WriteCallback(IAsyncResult ar)
        {
            CommandState commandState = ar.AsyncState as CommandState;
            if (commandState != null)
            {
                try
                {
                    commandState.NetworkStream.EndWrite(ar);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception thrown: {0}", e.Message);
                }
            }
        }

        
    }
}
