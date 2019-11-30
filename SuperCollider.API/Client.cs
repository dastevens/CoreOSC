namespace SuperCollider.API
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using CoreOSC.IO;

    public class Client
    {
        private readonly UdpClient udpClient;

        public Client(UdpClient udpClient)
        {
            this.udpClient = udpClient;
        }

        public async Task<Done> Quit()
        {
            var quit = new Quit();
            await udpClient.SendMessageAsync(quit.Message);
            var response = await udpClient.ReceiveMessageAsync();
            return Done.FromMessage(response);
        }

        public async Task<StatusReply> Status()
        {
            var status = new Status();
            await udpClient.SendMessageAsync(status.Message);
            var response = await udpClient.ReceiveMessageAsync();
            return StatusReply.FromMessage(response);
        }

        public async Task<VersionReply> Version()
        {
            var version = new Version();
            await udpClient.SendMessageAsync(version.Message);
            var response = await udpClient.ReceiveMessageAsync();
            return VersionReply.FromMessage(response);
        }
    }
}
