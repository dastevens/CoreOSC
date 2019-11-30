using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NMapAN
    {
        public NMapAN(int nodeId, IEnumerable<(Option<int, string> control, int controlBusIndex, int numberOfControls)> controlValues)
        {
            NodeId = nodeId;
            ControlValues = controlValues;
        }

        public OscMessage Message => new OscMessage(new Address("/n_mapan"),
            new object[] { NodeId }
            .Concat(
                ControlValues
                    .SelectMany(controlValue => new object[] { controlValue.control.Value, controlValue.controlBusIndex, controlValue.numberOfControls })));

        public int NodeId { get; }
        public IEnumerable<(Option<int, string> control, int controlBusIndex, int numberOfControls)> ControlValues { get; }
    }
}
