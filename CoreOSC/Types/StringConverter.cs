namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringConverter : IConverter<string>
    {
        public (string value, IEnumerable<DWord> dWords) Deserialize(IEnumerable<DWord> dWords)
        {
            var next = new string(Encoding.ASCII.GetChars(dWords.First().Bytes))
                .Replace("\0", string.Empty);
            if (dWords.First().Bytes.Any(b => b == 0))
            {
                // Terminator found
                return (
                    value: next,
                    dWords: dWords.Skip(1));
            }
            else
            {
                (var nextValue, var nextDWords) = Deserialize(dWords.Skip(1));
                return (
                    value: next + nextValue,
                    dWords: nextDWords
                    );
            }
        }

        public IEnumerable<DWord> Serialize(string value)
        {
            return this.Serialize(value.ToCharArray());
        }

        private IEnumerable<DWord> Serialize(IEnumerable<char> chars)
        {
            if (!chars.Any())
            {
                return new DWord[0];
            }

            var next = chars.Take(4);
            var dWord = new DWord(Encoding.ASCII.GetBytes(next.ToArray()));
            return new[] { dWord }.Concat(Serialize(chars.Skip(4)));
        }
    }
}