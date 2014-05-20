using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TCPSubscriber
{
    public class TCPSubscriber
    {
        private AutoResetEvent waitHandle = new AutoResetEvent(false);
        Queue<string> queue = new Queue<string>();
        private object queueLock = new object();

        public void Start()
        {
            IPEndPoint endPoint = new IPEndPoint(Dns.GetHostAddresses("localhost")[0], 2500);
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(endPoint);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not connect to remote host: {0}.", endPoint.ToString());
                return;
            }
            

            using (NetworkStream networkStream = client.GetStream())
            {
                StartCommunicationLoop(networkStream);
            }
        }

        private void StartCommunicationLoop(NetworkStream networkStream)
        {
            RequestData(networkStream);

            int startIndex = 0;
            while (true)
            {
                ProcessQueue();

                if (networkStream.DataAvailable)
                {
                    Task.Factory.StartNew(() => 
                    { 
                        Thread.CurrentThread.Name = "Thread " + startIndex; 
                        HandleReader(networkStream); 
                    });
                    waitHandle.WaitOne();

                    RequestData(networkStream);
                    startIndex++;
                }

                Console.WriteLine("{0} => {1}", startIndex * 10, queue.Count());
            }
        }

        private void ProcessQueue()
        {
            while (queue.Count > 0)
            {
                var row = queue.Dequeue();
                Console.WriteLine("\t\t\t\tDequeueing {0}... Remaining {1}.", row, queue.Count());
            }
        }

        private void HandleReader(NetworkStream networkStream)
        {
            List<byte> data = new List<byte>();

            byte[] buffer = new byte[256];
            networkStream.ReadTimeout = 1000;

            int bytesRead = 0;
            do
            {
                if (bytesRead > 0)
                    Array.Clear(buffer, 0, buffer.Length);
                try
                {
                    bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    SocketException se = ex.InnerException as SocketException;
                    if (se.SocketErrorCode != SocketError.TimedOut)
                    {
                        throw;
                    }
                    bytesRead = 0;
                }
                
                data.AddRange(buffer.Take(bytesRead));
            }
            while (bytesRead == buffer.Length);

            if (IsDataIntegrityValid(data))
                Enqueue(data);
            else
                LogDataIntegrityProblem();

            waitHandle.Set();
        }

        private bool IsDataIntegrityValid(List<byte> data)
        {
            return data.Count % 15 == 0;
        }

        private void LogDataIntegrityProblem()
        {
            Debugger.Break();
        }

        private void Enqueue(List<byte> data)
        {
            lock (queueLock)
            {
                List<string> receivedList = Encoding.ASCII.GetString(data.ToArray()).Split('|').Where(s => s.Length != 0).ToList();
                receivedList.ForEach(s => { if (s.Length > 0) queue.Enqueue(s); });
                //Encoding.ASCII.GetString(data.ToArray()).Split('|').Where(s => s.Length != 0).ToList().ForEach(s => queue.Enqueue(s));
            }
        }

        private void RequestData(NetworkStream networkStream)
        {
            WriteLine(networkStream, "start");
        }

        private void WriteLine(NetworkStream networkStream, string line)
        {
            try
            {
                byte[] lineBytes = Encoding.ASCII.GetBytes(line);
                networkStream.Write(lineBytes, 0, lineBytes.Length);
                //networkStream.BeginWrite(lineBytes, 0, lineBytes.Length, WriteCallback, networkStream);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception thrown: {0}", e.Message);
            }
        }
    }
}
