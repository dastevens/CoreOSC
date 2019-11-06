namespace CoreOSC
{
    using System;
    using System.Net;

    public class UDPDuplex : UDPListener
    {
        public int RemotePort { get; private set; }
        public string RemoteAddress { get; private set; }

        private IPEndPoint RemoteIpEndPoint2;

        public UDPDuplex(string remoteAddress, int remotePort, int port) : base(port)
        {
            this.RemotePort = remotePort;
            this.RemoteAddress = remoteAddress;

            var addresses = System.Net.Dns.GetHostAddresses(remoteAddress);
            if (addresses.Length == 0) throw new Exception("Unable to find IP address for " + remoteAddress);

            this.RemoteIpEndPoint2 = new IPEndPoint(addresses[0], remotePort);
        }

        public UDPDuplex(string remoteAddress, int remotePort, int port, HandleOscPacket callback) : this(remoteAddress, remotePort, port)
        {
            this.OscPacketCallback = callback;
        }

        public UDPDuplex(string remoteAddress, int remotePort, int port, HandleBytePacket callback) : this(remoteAddress, remotePort, port)
        {
            this.BytePacketCallback = callback;
        }

        public void Send(byte[] message)
        {
            this.receivingUdpClient.Send(message, message.Length, this.RemoteIpEndPoint2);
        }

        public void Send(OscPacket packet)
        {
            byte[] data = packet.GetBytes();
            this.Send(data);
        }
    }
}