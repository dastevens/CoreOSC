namespace CoreOSC
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class UDPSender
    {
        private readonly IPEndPoint remoteIpEndPoint;
        private readonly Socket sock;

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPSender"/> class.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public UDPSender(string address, int port)
        {
            this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var addresses = System.Net.Dns.GetHostAddresses(address);
            if (addresses.Length == 0)
            {
                throw new Exception("Unable to find IP address for " + address);
            }

            this.remoteIpEndPoint = new IPEndPoint(addresses[0], port);
        }

        public void Send(byte[] message)
        {
            this.sock.SendTo(message, this.remoteIpEndPoint);
        }

        public void Send(OscPacket packet)
        {
            var data = packet.GetBytes();
            this.Send(data);
        }

        public void Close()
        {
            this.sock.Close();
        }
    }
}