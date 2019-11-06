namespace CoreOSC
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class UDPSender
    {
        public int Port
        {
            get { return this.port; }
        }

        private int port;

        public string Address
        {
            get { return this.address; }
        }

        private string address;

        private IPEndPoint RemoteIpEndPoint;
        private Socket sock;

        public UDPSender(string address, int port)
        {
            this.port = port;
            this.address = address;

            this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var addresses = System.Net.Dns.GetHostAddresses(address);
            if (addresses.Length == 0) throw new Exception("Unable to find IP address for " + address);

            this.RemoteIpEndPoint = new IPEndPoint(addresses[0], port);
        }

        public void Send(byte[] message)
        {
            this.sock.SendTo(message, this.RemoteIpEndPoint);
        }

        public void Send(OscPacket packet)
        {
            byte[] data = packet.GetBytes();
            this.Send(data);
        }

        public void Close()
        {
            this.sock.Close();
        }
    }
}