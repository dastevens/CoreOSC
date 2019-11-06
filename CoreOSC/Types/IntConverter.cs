namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IntConverter : IConverter<int>
    {
        public (int value, IEnumerable<DWord> dWords) Deserialize(IEnumerable<DWord> dWords)
        {
            return (
                value: BitConverter.ToInt32(dWords.First().Reverse().Bytes, 0),
                dWords: dWords.Skip(1)
                );
        }

        public IEnumerable<DWord> Serialize(int value)
        {
            yield return new DWord(BitConverter.GetBytes(value))
                .Reverse();
        }
    }
}