﻿namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringConverter : IConverter<string>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out string value)
        {
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
                var nextDWords = Deserialize(dWords.Skip(1), out var nextValue);
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