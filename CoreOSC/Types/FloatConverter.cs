namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FloatConverter : IConverter<float>
    {
        public (float value, IEnumerable<DWord> dWords) Deserialize(IEnumerable<DWord> dWords)
        {
            return (
                value: BitConverter.ToSingle(dWords.First().Reverse().Bytes, 0),
                dWords: dWords.Skip(1)
                );
        }

        public IEnumerable<DWord> Serialize(float value)
        {
            yield return new DWord(BitConverter.GetBytes(value))
                .Reverse();
        }
    }
}