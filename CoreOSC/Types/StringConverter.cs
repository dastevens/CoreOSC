namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringConverter : IConverter<string>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out string value)
        {
            if (!dWords.Any())
            {
                value = "";
                return dWords;
            }

            var next = new string(Encoding.ASCII.GetChars(dWords.First().Bytes))
                .Replace("\0", string.Empty);
            if (dWords.First().Bytes.Any(b => b == 0))
            {
                // Terminator found
                value = next;
                return dWords.Skip(1);
            }
            else
            {
                var nextDWords = Deserialize(dWords.Skip(1), out string nextValue);
                value = next + nextValue;
                return nextDWords;
            }
        }

        public IEnumerable<DWord> Serialize(string value)
        {
            return this.Serialize(value.ToCharArray());
        }

        private IEnumerable<DWord> Serialize(IEnumerable<char> chars)
        {
            var firstChars = chars.Take(4);
            var dWord = new DWord(Encoding.ASCII.GetBytes(firstChars.ToArray()));
            if (firstChars.Count() < 4)
            {
                return new[] { dWord };
            }
            else
            {
                var nextChars = chars.Skip(4);
                return new[] { dWord }.Concat(Serialize(nextChars));
            }
        }
    }
}