namespace CoreOSC
{
    using System;
    using System.Net;

    public class UDPDuplex : UDPListener
    {
        private readonly IPEndPoint remoteIpEndPoint2;

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPDuplex"/> class.
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <param name="remotePort"></param>
        /// <param name="port"></param>
        public UDPDuplex(string remoteAddress, int remotePort, int port)
            : base(port)
        {
            this.RemotePort = remotePort;
            this.RemoteAddress = remoteAddress;

            var addresses = System.Net.Dns.GetHostAddresses(remoteAddress);
            if (addresses.Length == 0)
            {
                throw new Exception("Unable to find IP address for " + remoteAddress);
            }

            this.remoteIpEndPoint2 = new IPEndPoint(addresses[0], remotePort);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPDuplex"/> class.
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <param name="remotePort"></param>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public UDPDuplex(string remoteAddress, int remotePort, int port, HandleOscPacket callback)
            : this(remoteAddress, remotePort, port)
        {
            this.OscPacketCallback = callback;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPDuplex"/> class.
        /// </summary>
        /// <param name="remoteAddress"></param>
        /// <param name="remotePort"></param>
        /// <param name="port"></param>
        /// <param name="callback"></param>
        public UDPDuplex(string remoteAddress, int remotePort, int port, HandleBytePacket callback)
            : this(remoteAddress, remotePort, port)
        {
            this.BytePacketCallback = callback;
        }

        public int RemotePort { get; private set; }

        public string RemoteAddress { get; private set; }

        public void Send(byte[] message)
        {
            this.ReceivingUdpClient.Send(message, message.Length, this.remoteIpEndPoint2);
        }

        public void Send(OscPacket packet)
        {
            var data = packet.GetBytes();
            this.Send(data);
        }
    }
}