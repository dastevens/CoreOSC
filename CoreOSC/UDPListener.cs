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

        private readonly object callbackLock = new object();

        protected UdpClient receivingUdpClient;
        private IPEndPoint RemoteIpEndPoint;

        protected HandleBytePacket BytePacketCallback = null;
        protected HandleOscPacket OscPacketCallback = null;

        private readonly ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();

        public UDPListener(int port)
        {
            this.Port = port;

            // try to open the port 10 times, else fail
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    this.receivingUdpClient = new UdpClient(port);
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
            this.RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // setup first async event
            AsyncCallback callBack = new AsyncCallback(this.ReceiveCallback);
            this.receivingUdpClient.BeginReceive(callBack, null);
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
            Monitor.Enter(this.callbackLock);
            byte[] bytes = null;

            try
            {
                bytes = this.receivingUdpClient.EndReceive(result, ref this.RemoteIpEndPoint);
            }
            catch (ObjectDisposedException)
            {
                // Ignore if disposed. This happens when closing the listener
            }

            // Process bytes
            if (bytes != null && bytes.Length > 0)
            {
                if (this.BytePacketCallback != null)
                {
                    this.BytePacketCallback(bytes);
                }
                else if (this.OscPacketCallback != null)
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

                    this.OscPacketCallback(packet);
                }
                else
                {
                    this.queue.Enqueue(bytes);
                }
            }

            if (!this.closing)
            {
                // Setup next async event
                AsyncCallback callBack = new AsyncCallback(this.ReceiveCallback);
                this.receivingUdpClient.BeginReceive(callBack, null);
            }
            Monitor.Exit(this.callbackLock);
        }

        private bool closing = false;

        public void Dispose()
        {
            lock (this.callbackLock)
            {
                this.closing = true;
                this.receivingUdpClient.Close();
                this.receivingUdpClient.Dispose();
            }
        }

        public OscPacket Receive()
        {
            if (!this.closing)
            {
                if (this.queue.TryDequeue(out var bytes))
                {
                    var packet = OscPacket.GetPacket(bytes);
                    return packet;
                }
            }
            return null;
        }
    }
}