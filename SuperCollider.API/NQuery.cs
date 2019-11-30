using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NQuery
    {
        public NQuery(IEnumerable<int> nodeIds)
        {
            NodeIds = nodeIds;
        }

        public OscMessage Message => new OscMessage(new Address("/n_query"), NodeIds.Cast<object>());

        public IEnumerable<int> NodeIds { get; }
    }
}
