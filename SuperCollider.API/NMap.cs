using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NMap
    {
        public NMap(int nodeId, IEnumerable<(Option<int, string> control, int controlBusIndex)> controlValues)
        {
            NodeId = nodeId;
            ControlValues = controlValues;
        }

        public OscMessage Message => new OscMessage(new Address("/n_map"),
            new object[] { NodeId }
            .Concat(
                ControlValues
                    .SelectMany(controlValue => new object[] { controlValue.control.Value, controlValue.controlBusIndex })));

        public int NodeId { get; }
        public IEnumerable<(Option<int, string> control, int controlBusIndex)> ControlValues { get; }
    }
}
