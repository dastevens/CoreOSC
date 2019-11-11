namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RGBAConverter : IConverter<RGBA>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out RGBA value)
        {
            var dWord = dWords.First();
            value = new RGBA(dWord.Byte0, dWord.Byte1, dWord.Byte2, dWord.Byte3);
            return dWords.Skip(1);
        }

        public IEnumerable<DWord> Serialize(RGBA value)
        {
            yield return new DWord(value.R, value.G, value.B, value.A);
        }
    }
}