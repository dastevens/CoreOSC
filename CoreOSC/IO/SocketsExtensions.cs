namespace CoreOSC.IO
{
    using System.Linq;
    using System.Net.Sockets;
    using CoreOSC.Types;

    public static class SocketsExtensions
    {
        private static readonly BytesConverter BytesConverter = new BytesConverter();
        private static readonly OscMessageConverter MessageConverter = new OscMessageConverter();

        public static void SendMessage(this UdpClient client, OscMessage message)
        {
            var dWords = MessageConverter.Serialize(message);
            _ = BytesConverter.Deserialize(dWords, out var bytes);
            var byteArray = bytes.ToArray();
            client.Send(byteArray, byteArray.Length);
        }

        public static void SendMessage(this TcpClient client, OscMessage message)
        {
            var dWords = MessageConverter.Serialize(message);
            _ = BytesConverter.Deserialize(dWords, out var bytes);
            var byteArray = bytes.ToArray();
            client.GetStream().Write(byteArray, byteArray.Length, 0);
        }
    }
}
