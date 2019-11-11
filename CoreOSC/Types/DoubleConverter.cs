namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DoubleConverter : IConverter<double>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out double value)
        {
            value = BitConverter.ToDouble(
                dWords.Skip(1).First().Reverse().Bytes
                .Concat(dWords.First().Reverse().Bytes).ToArray(), 0);
            return dWords.Skip(2);
        }

        public IEnumerable<DWord> Serialize(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            yield return new DWord(bytes.Skip(4).Take(4))
                .Reverse();
            yield return new DWord(bytes.Take(4))
                .Reverse();
        }
    }
}