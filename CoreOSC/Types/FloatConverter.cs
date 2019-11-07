namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FloatConverter : IConverter<float>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out float value)
        {
            value = BitConverter.ToSingle(dWords.First().Reverse().Bytes, 0);
            return dWords.Skip(1);
        }

        public IEnumerable<DWord> Serialize(float value)
        {
            yield return new DWord(BitConverter.GetBytes(value))
                .Reverse();
        }
    }
}