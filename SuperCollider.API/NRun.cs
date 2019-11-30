using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NRun
    {
        public NRun(IEnumerable<(int nodeId, int runFlag)> nodeIdRunFlags)
        {
            NodeIdRunFlags = nodeIdRunFlags;
        }

        public OscMessage Message => new OscMessage(new Address("/n_run"), NodeIdRunFlags.SelectMany(nodeIdRunFlags => new object[] { nodeIdRunFlags.nodeId, nodeIdRunFlags.runFlag }));

        public IEnumerable<(int nodeId, int runFlag)> NodeIdRunFlags { get; }
    }
}
