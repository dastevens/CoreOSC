namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BytesConverter : IConverter<IEnumerable<byte>>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out IEnumerable<byte> value)
        {
            if (!dWords.Any())
            {
                value = new byte[0];
                return dWords;
            }
            var next = dWords.First().Bytes;
            var nextDWords = Deserialize(dWords.Skip(1), out var nextValue);
            value = next.Concat(nextValue);
            return nextDWords;
        }

        public IEnumerable<DWord> Serialize(IEnumerable<byte> value)
        {
            if (!value.Any())
            {
                return new DWord[0];
            }

            var next = value.Take(4);
            var dWord = new DWord(next.ToArray());
            return new[] { dWord }.Concat(Serialize(value.Skip(4)));
        }
    }
}