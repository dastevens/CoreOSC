using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NOrder
    {
        public NOrder(int addAction, int addTargetId, IEnumerable<int> nodeIds)
        {
            AddAction = addAction;
            AddTargetId = addTargetId;
            NodeIds = nodeIds;
        }

        public OscMessage Message => new OscMessage(new Address("/n_order"),
            new object[] { AddAction, AddTargetId }.Concat(NodeIds.Cast<object>()));

        public int AddAction { get; }
        public int AddTargetId { get; }
        public IEnumerable<int> NodeIds { get; }
    }
}
