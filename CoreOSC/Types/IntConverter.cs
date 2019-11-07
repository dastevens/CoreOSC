namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IntConverter : IConverter<int>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out int value)
        {
            value = BitConverter.ToInt32(dWords.First().Reverse().Bytes, 0);
            return dWords.Skip(1);
        }

        public IEnumerable<DWord> Serialize(int value)
        {
            yield return new DWord(BitConverter.GetBytes(value))
                .Reverse();
        }
    }
}