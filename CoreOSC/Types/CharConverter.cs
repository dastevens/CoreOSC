namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CharConverter : IConverter<char>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out char value)
        {
            value = (char)dWords.First().Byte3;
            return dWords.Skip(1);
        }

        public IEnumerable<DWord> Serialize(char value)
        {
            yield return new DWord(0, 0, 0, (byte)value);
        }
    }
}