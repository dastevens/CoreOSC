namespace CoreOSC
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public delegate void HandleOscPacket(OscPacket packet);

    public delegate void HandleBytePacket(byte[] packet);

    public class UDPListener : IDisposable
    {
        public int Port { get; private set; }

        readonly private object callbackLock = new object();

        protected UdpClient receivingUdpClient;
        private IPEndPoint RemoteIpEndPoint;

        protected HandleBytePacket BytePacketCallback = null;
        protected HandleOscPacket OscPacketCallback = null;

        readonly private ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();

        public UDPListener(int port)
        {
            Port = port;

            // try to open the port 10 times, else fail
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    receivingUdpClient = new UdpClient(port);
                    break;
                }
                catch (Exception)
                {
                    // Failed in ten tries, throw the exception and give up
                    if (i >= 9)
                        throw;

                    Thread.Sleep(5);
                }
            }
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // setup first async event
            AsyncCallback callBack = new AsyncCallback(ReceiveCallback);
            receivingUdpClient.BeginReceive(callBack, null);
        }

        public UDPListener(int port, HandleOscPacket callback) : this(port)
        {
            this.OscPacketCallback = callback;
        }

        public UDPListener(int port, HandleBytePacket callback) : this(port)
        {
            this.BytePacketCallback = callback;
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Monitor.Enter(callbackLock);
            Byte[] bytes = null;

            try
            {
                bytes = receivingUdpClient.EndReceive(result, ref RemoteIpEndPoint);
            }
            catch (ObjectDisposedException)
            {
                // Ignore if disposed. This happens when closing the listener
            }

            // Process bytes
            if (bytes != null && bytes.Length > 0)
            {
                if (BytePacketCallback != null)
                {
                    BytePacketCallback(bytes);
                }
                else if (OscPacketCallback != null)
                {
                    OscPacket packet = null;
                    try
                    {
                        packet = OscPacket.GetPacket(bytes);
                    }
                    catch (Exception)
                    {
                        // If there is an error reading the packet, null is sent to the callback
                    }

                    OscPacketCallback(packet);
                }
                else
                {
                    queue.Enqueue(bytes);
                }
            }

            if (!closing)
            {
                // Setup next async event
                AsyncCallback callBack = new AsyncCallback(ReceiveCallback);
                receivingUdpClient.BeginReceive(callBack, null);
            }
            Monitor.Exit(callbackLock);
        }

        private bool closing = false;

        public void Dispose()
        {
            lock (callbackLock)
            {
                closing = true;
                receivingUdpClient.Close();
                receivingUdpClient.Dispose();
            }
        }

        public OscPacket Receive()
        {
            if (!closing)
            {
                if (queue.TryDequeue(out var bytes))
                {
                    var packet = OscPacket.GetPacket(bytes);
                    return packet;
                }
            }
            return null;
        }
    }
}