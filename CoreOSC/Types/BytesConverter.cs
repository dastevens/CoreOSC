namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BytesConverter : IConverter<IEnumerable<byte>>
    {
        public (IEnumerable<byte> value, IEnumerable<DWord> dWords) Deserialize(IEnumerable<DWord> dWords)
        {
            if (!dWords.Any())
            {
                return (
                    value: new byte[0],
                    dWords: dWords
                    );
            }
            var next = dWords.First().Bytes;
            (var nextValue, var nextDWords) = Deserialize(dWords.Skip(1));
            return (
                value: next.Concat(nextValue),
                dWords: nextDWords
                );
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