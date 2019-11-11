namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OscNilConverter : IConverter<OscNil>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscNil value)
        {
            value = OscNil.Nil;
            return dWords;
        }

        public IEnumerable<DWord> Serialize(OscNil value)
        {
            return new DWord[0];
        }
    }
}