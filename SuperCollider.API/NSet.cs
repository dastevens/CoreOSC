using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NSet
    {
        public NSet(int nodeId, IEnumerable<(Option<int, string> control, Option<float, int> value)> controlValues)
        {
            NodeId = nodeId;
            ControlValues = controlValues;
        }

        public OscMessage Message => new OscMessage(new Address("/n_set"),
            new object[] { NodeId }
            .Concat(
                ControlValues
                    .SelectMany(controlValue => new object[] { controlValue.control.Value, controlValue.value.Value })));

        public int NodeId { get; }
        public IEnumerable<(Option<int, string> control, Option<float, int> value)> ControlValues { get; }
    }
}
