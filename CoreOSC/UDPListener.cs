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
        private readonly ConcurrentQueue<byte[]> queue = new ConcurrentQueue<byte[]>();
        private readonly object callbackLock = new object();

        private IPEndPoint remoteIpEndPoint;
        private bool closing = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPListener"/> class.
        /// </summary>
        /// <param name="port"></param>
        public UDPListener(int port)
        {
            // try to open the port 10 times, else fail
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    this.ReceivingUdpClient = new UdpClient(port);
                    break;
                }
                catch (Exception)
                {
                    // Failed in ten tries, throw the exception and give up
                    if (i >= 9)
                    {
                        throw;
                    }

                    Thread.Sleep(5);
                }
            }

            this.remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // setup first async event
            var callBack = new AsyncCallback(this.ReceiveCallback);
            this.ReceivingUdpClient.BeginReceive(callBack, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPListener"/> class.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public UDPListener(int port, HandleOscPacket callback)
            : this(port)
        {
            this.OscPacketCallback = callback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPListener"/> class.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public UDPListener(int port, HandleBytePacket callback)
            : this(port)
        {
            this.BytePacketCallback = callback;
        }

        protected UdpClient ReceivingUdpClient { get; }

        protected HandleBytePacket BytePacketCallback { get; set; }

        protected HandleOscPacket OscPacketCallback { get; set; }

        public void Dispose()
        {
            lock (this.callbackLock)
            {
                this.closing = true;
                this.ReceivingUdpClient.Close();
                this.ReceivingUdpClient.Dispose();
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

        private void ReceiveCallback(IAsyncResult result)
        {
            Monitor.Enter(this.callbackLock);
            byte[] bytes = null;

            try
            {
                bytes = this.ReceivingUdpClient.EndReceive(result, ref this.remoteIpEndPoint);
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
                var callBack = new AsyncCallback(this.ReceiveCallback);
                this.ReceivingUdpClient.BeginReceive(callBack, null);
            }

            Monitor.Exit(this.callbackLock);
        }
    }
}