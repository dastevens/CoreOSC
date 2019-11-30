using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NBefore
    {
        public NBefore(IEnumerable<(int moveNodeId, int beforeNodeId)> moves)
        {
            Moves = moves;
        }

        public OscMessage Message => new OscMessage(new Address("/n_before"),
            Moves.SelectMany(move => new object[] { move.moveNodeId, move.beforeNodeId }));

        public IEnumerable<(int moveNodeId, int beforeNodeId)> Moves { get; }
    }
}
