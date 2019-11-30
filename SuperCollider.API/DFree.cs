using CoreOSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperCollider.API
{
    public class DFree
    {
        public DFree(IEnumerable<string> synthDefNames)
        {
            SynthDefNames = synthDefNames;
        }

        public OscMessage Message => new OscMessage(new Address("/d_free"), SynthDefNames.Cast<object>());

        public IEnumerable<string> SynthDefNames { get; }
    }
}
