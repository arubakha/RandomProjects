using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TCPPublisher
{
    class Program
    {
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);
        //private static AutoResetEvent waitHandle = new AutoResetEvent(false);
        private static Random random = new Random();

        static void Main(string[] args)
        {
            //StartPublisher();

            //StartPublisherTcpClient();

            //StartMultithreadedPublisher();

            //StartTPLPublisher();

            //StartSocketPublisher();

            TCPPublisher tcpPublisher = new TCPPublisher(Dns.GetHostAddresses("localhost")[0], 2500);
            tcpPublisher.Start();

            //Console.ReadLine();
        }

        private static void StartTPLPublisher()
        {
            TcpListener tcpListener = new TcpListener(Dns.GetHostAddresses("localhost")[0], 2500);
            tcpListener.Start();

            while (true)
            {
                Socket socket = tcpListener.AcceptSocket();
                Task.Factory.StartNew(() => HandleListener(socket));
            }
        }

        private class CommandState
        { 
            public byte[] Data = new byte[500];
            public NetworkStream NetworkStream;
            public StringBuilder Command = new StringBuilder();
        }

        private static void HandleListener(Socket socket)
        {
            using (socket)
            {
                using (NetworkStream networkStream = new NetworkStream(socket))
                {
                    //using (StreamReader reader = new StreamReader(networkStream))
                    {
                        using (StreamWriter writer = new StreamWriter(networkStream))
                        {
                            writer.AutoFlush = true;
                            writer.WriteLine("Hello, World !");

                            try
                            {
                                var state = new CommandState();
                                state.NetworkStream = networkStream;

                                while (true)
                                {
                                    if (state.Command.ToString() == "end")
                                    {
                                        break;
                                    }
                                    else if (state.Command.ToString() == "start")
                                    {
                                        state.Command.Clear();
                                        writer.WriteLine();
                                        for (int i = 0; i < 10; i++)
                                        {
                                            writer.WriteLine("Data string {0}.", i);
                                        }
                                    }

                                    if (networkStream.DataAvailable)
                                    {
                                        IAsyncResult ar = networkStream.BeginRead(state.Data, 0, state.Data.Length, ReadCallback, state);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Exception thrown: {0}", e.Message);
                            }
                        }

                        
                    }
                }
            }
        }

        private static void ReadCallback(IAsyncResult ar)
        { 
            CommandState readState = ar.AsyncState as CommandState;
            if (readState != null)
            {
                int bytesRead = readState.NetworkStream.EndRead(ar);
                string data = Encoding.ASCII.GetString(readState.Data, 0, bytesRead);
                if (bytesRead == 1 && Char.IsLetter(data, 0))
                    readState.Command.Append(data);
            }
        }

        #region StartMultithreadedPublisher
        private static void StartMultithreadedPublisher()
        {
            TcpListener tcpListener = new TcpListener(Dns.GetHostAddresses("localhost")[0], 2500);
            tcpListener.Start();

            int threadCounter = 0;
            while (true)
            {
                waitHandle.Reset();
                Thread th = new Thread(ThreadCallBack);
                th.Name = string.Format("Thread {0}", threadCounter);
                th.Start(tcpListener);
                waitHandle.WaitOne();
                threadCounter++;
            }
        }

        private static void ThreadCallBack(object data)
        {
            TcpListener tcpListener = data as TcpListener;
            if (tcpListener != null)
            {
                using (Socket socket = tcpListener.AcceptSocket())
                {
                    waitHandle.Set();
                    using (NetworkStream networkStream = new NetworkStream(socket))
                    {
                        using (StreamReader reader = new StreamReader(networkStream))
                        {
                            using (StreamWriter writer = new StreamWriter(networkStream))
                            {
                                writer.AutoFlush = true;
                                writer.WriteLine("Hello, World !");

                                //while (true)
                                //{
                                //    string request = reader.ReadLine();
                                //    if (request == "start")
                                //    {
                                //        int index = 0;
                                //        while (request != "end")
                                //        {
                                //            writer.WriteLine("[{0}] Data string {1}.", Thread.CurrentThread.Name, index);
                                //            index++;
                                //            request = reader.ReadLine();
                                //        }
                                //    }
                                //}

                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region StartPublisherTcpClient
        private static void StartPublisherTcpClient()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 2502);
            tcpListener.Start();

            while (true)
            {
                tcpListener.BeginAcceptTcpClient(AcceptTcpClientCallback, tcpListener);
                waitHandle.WaitOne();
            }
        }

        static private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpListener tcpListener = ar.AsyncState as TcpListener;
            if (tcpListener != null)
            {
                using (TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar))
                {
                    using (NetworkStream networkStream = tcpClient.GetStream())
                    {
                        using (StreamWriter writer = new StreamWriter(networkStream))
                        {
                            writer.AutoFlush = true;
                            writer.WriteLine("Hello, World!");
                        }
                    }
                }
            }
        }
        #endregion

        #region StartPublisher
        private static void StartPublisher()
        {
            TcpListener tcpListener = new TcpListener(Dns.GetHostAddresses("localhost")[0], 2500);
            tcpListener.Start();

            while (true)
            {
                tcpListener.BeginAcceptSocket(AcceptSocketCallback, tcpListener);
                waitHandle.WaitOne();
            }
        }

        static private void AcceptSocketCallback(IAsyncResult ar)
        {
            waitHandle.Set();

            TcpListener tcpListener = ar.AsyncState as TcpListener;
            if (tcpListener != null)
            {
                using (Socket socket = tcpListener.EndAcceptSocket(ar))
                {
                    using (NetworkStream networkStream = new NetworkStream(socket))
                    {
                        using (StreamWriter writer = new StreamWriter(networkStream))
                        {
                            writer.AutoFlush = true;
                            writer.WriteLine("Hello, World !");

                            //while (true)
                            //{
                            //    writer.Write("{0}, ", random.Next(1, 101));
                            //}
                        }
                    }
                }
            }
        }
        #endregion

        #region StartSocketPublisher
        private static void StartSocketPublisher()
        {
            IPEndPoint localEndPoint = new IPEndPoint(Dns.GetHostAddresses("localhost")[0], 2500);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                listener.BeginAccept(AcceptCallback, listener);
                waitHandle.WaitOne();
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            waitHandle.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            using (Socket handler = listener.EndAccept(ar))
            {
                using (NetworkStream networkStream = new NetworkStream(handler))
                {
                    using (StreamWriter writer = new StreamWriter(networkStream))
                    {
                        writer.AutoFlush = true;
                        writer.WriteLine("Hello, World !");

                        //while (true)
                        //{
                        //    writer.Write("{0}, ", random.Next(1, 101));
                        //}
                    }
                }
            }

        }
        #endregion

    }
}
