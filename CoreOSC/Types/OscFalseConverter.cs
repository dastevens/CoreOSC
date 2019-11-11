namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class OscFalseConverter : IConverter<OscFalse>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscFalse value)
        {
            value = OscFalse.False;
            return dWords;
        }

        public IEnumerable<DWord> Serialize(OscFalse value)
        {
            return new DWord[0];
        }
    }
}