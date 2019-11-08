namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ULongConverter : IConverter<ulong>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out ulong value)
        {
            value = BitConverter.ToUInt64(
                dWords.Skip(1).First().Reverse().Bytes
                .Concat(dWords.First().Reverse().Bytes).ToArray(), 0);
            return dWords.Skip(2);
        }

        public IEnumerable<DWord> Serialize(ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            yield return new DWord(bytes.Skip(4).Take(4))
                .Reverse();
            yield return new DWord(bytes.Take(4))
                .Reverse();
        }
    }
}