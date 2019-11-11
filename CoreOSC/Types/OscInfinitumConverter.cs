namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OscInfinitumConverter : IConverter<OscInfinitum>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscInfinitum value)
        {
            value = OscInfinitum.Infinitum;
            return dWords;
        }

        public IEnumerable<DWord> Serialize(OscInfinitum value)
        {
            return new DWord[0];
        }
    }
}