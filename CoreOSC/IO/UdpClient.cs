namespace CoreOSC.IO
{
    using CoreOSC.Types;
    using System.Linq;
    using System.Net.Sockets;

    public static class UdpClientExtensions
    {
        private static readonly BytesConverter BytesConverter = new BytesConverter();
        private static readonly OscMessageConverter MessageConverter = new OscMessageConverter();

        public static void SendMessage(this UdpClient client, OscMessage message)
        {
            var dWords = MessageConverter.Serialize(message);
            _ = BytesConverter.Deserialize(dWords, out var bytes);
            client.Send(bytes.ToArray(), bytes.Count());
        }
    }
}
