using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NFree
    {
        public NFree(IEnumerable<int> nodeIds)
        {
            NodeIds = nodeIds;
        }

        public OscMessage Message => new OscMessage(new Address("/n_free"), NodeIds.Cast<object>());

        public IEnumerable<int> NodeIds { get; }
    }
}
