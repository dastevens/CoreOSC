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

        public async Task DFree(IEnumerable<string> synthDefNames)
        {
            var dFree = new DFree(synthDefNames);
            await udpClient.SendMessageAsync(dFree.Message);
        }

        public async Task<Done> DLoad(string pathname)
        {
            var dLoad = new DLoad(pathname);
            await udpClient.SendMessageAsync(dLoad.Message);
            var response = await udpClient.ReceiveMessageAsync();
            return Done.FromMessage(response);
        }

        public async Task<Done> DLoadDir(string pathname)
        {
            var dLoadDir = new DLoadDir(pathname);
            await udpClient.SendMessageAsync(dLoadDir.Message);
            var response = await udpClient.ReceiveMessageAsync();
            return Done.FromMessage(response);
        }

        public async Task<Done> DRecv(SynthDef synthDef)
        {
            var dRecv = new DRecv(synthDef.ToBytes());
            await udpClient.SendMessageAsync(dRecv.Message);
            var response = await udpClient.ReceiveMessageAsync();
            return Done.FromMessage(response);
        }

        public async Task NAfter(IEnumerable<(int moveNodeId, int afterNodeId)> moves)
        {
            var nAfter = new NAfter(moves);
            await udpClient.SendMessageAsync(nAfter.Message);
        }

        public async Task NBefore(IEnumerable<(int moveNodeId, int beforeNodeId)> moves)
        {
            var nBefore = new NBefore(moves);
            await udpClient.SendMessageAsync(nBefore.Message);
        }

        public async Task NFill(int nodeId, IEnumerable<(Option<int, string> control, int m, IEnumerable<Option<float, int>> values)> controlValues)
        {
            var nFill = new NFill(nodeId, controlValues);
            await udpClient.SendMessageAsync(nFill.Message);
        }

        public async Task NFree(IEnumerable<int> nodeIds)
        {
            var nFree = new NFree(nodeIds);
            await udpClient.SendMessageAsync(nFree.Message);
        }

        public async Task NMap(int nodeId, IEnumerable<(Option<int, string> control, int controlBusIndex)> controlValues)
        {
            var nMap = new NMap(nodeId, controlValues);
            await udpClient.SendMessageAsync(nMap.Message);
        }

        public async Task NMapA(int nodeId, IEnumerable<(Option<int, string> control, int controlBusIndex)> controlValues)
        {
            var nMapA = new NMapA(nodeId, controlValues);
            await udpClient.SendMessageAsync(nMapA.Message);
        }

        public async Task NMapAN(int nodeId, IEnumerable<(Option<int, string> control, int controlBusIndex, int numberOfControls)> controlValues)
        {
            var nMapAN = new NMapAN(nodeId, controlValues);
            await udpClient.SendMessageAsync(nMapAN.Message);
        }

        public async Task NMapN(int nodeId, IEnumerable<(Option<int, string> control, int controlBusIndex, int numberOfControls)> controlValues)
        {
            var nMapN = new NMapN(nodeId, controlValues);
            await udpClient.SendMessageAsync(nMapN.Message);
        }

        public async Task NOrder(int addAction, int addTargetId, IEnumerable<int> nodeIds)
        {
            var nOrder = new NOrder(addAction, addTargetId, nodeIds);
            await udpClient.SendMessageAsync(nOrder.Message);
        }

        public async Task NQuery(IEnumerable<int> nodeIds)
        {
            var nQuery = new NQuery(nodeIds);
            await udpClient.SendMessageAsync(nQuery.Message);
        }

        public async Task NRun(IEnumerable<(int nodeId, int runFlag)> nodeIdRunFlags)
        {
            var nRun = new NRun(nodeIdRunFlags);
            await udpClient.SendMessageAsync(nRun.Message);
        }

        public async Task NSet(int nodeId, IEnumerable<(Option<int, string> control, Option<float, int> value)> controlValues)
        {
            var nSet = new NSet(nodeId, controlValues);
            await udpClient.SendMessageAsync(nSet.Message);
        }

        public async Task NSetN(int nodeId, IEnumerable<(Option<int, string> control, int m, IEnumerable<Option<float, int>> values)> controlValues)
        {
            var nSetN = new NSetN(nodeId, controlValues);
            await udpClient.SendMessageAsync(nSetN.Message);
        }

        public async Task NTrace(IEnumerable<int> nodeIds)
        {
            var nTrace = new NTrace(nodeIds);
            await udpClient.SendMessageAsync(nTrace.Message);
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
