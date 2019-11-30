using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class NFill
    {
        public NFill(int nodeId, IEnumerable<(Option<int, string> control, int m, IEnumerable<Option<float, int>> values)> controlValues)
        {
            NodeId = nodeId;
            ControlValues = controlValues;
        }

        public OscMessage Message => new OscMessage(new Address("/n_fill"),
            new object[] { NodeId }
            .Concat(
                ControlValues
                    .SelectMany(controlValue => new object[] { controlValue.control.Value, controlValue.m }
                    .Concat(controlValue.values.Select(value => value.Value)))));

        public int NodeId { get; }
        public IEnumerable<(Option<int, string> control, int m, IEnumerable<Option<float, int>> values)> ControlValues { get; }
    }
}
