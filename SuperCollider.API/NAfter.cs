using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NAfter
    {
        public NAfter(IEnumerable<(int moveNodeId, int afterNodeId)> moves)
        {
            Moves = moves;
        }

        public OscMessage Message => new OscMessage(new Address("/n_after"),
            Moves.SelectMany(move => new object[] { move.moveNodeId, move.afterNodeId }));

        public IEnumerable<(int moveNodeId, int afterNodeId)> Moves { get; }
    }
}
