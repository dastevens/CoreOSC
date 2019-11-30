namespace CoreOSC.IO
{
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using CoreOSC.Types;

    public static class SocketsExtensions
    {
        private static readonly BytesConverter BytesConverter = new BytesConverter();
        private static readonly OscMessageConverter MessageConverter = new OscMessageConverter();

        public static async Task SendMessageAsync(this UdpClient client, OscMessage message)
        {
            var dWords = MessageConverter.Serialize(message);
            _ = BytesConverter.Deserialize(dWords, out var bytes);
            var byteArray = bytes.ToArray();
            await client.SendAsync(byteArray, byteArray.Length);
        }

        public static async Task<OscMessage> ReceiveMessageAsync(this UdpClient client)
        {
            var receiveResult = await client.ReceiveAsync();
            var dWords = BytesConverter.Serialize(receiveResult.Buffer);
            MessageConverter.Deserialize(dWords, out var value);
            return value;
        }
    }
}
