namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BlobConverter : IConverter<IEnumerable<byte>>
    {
        private readonly BytesConverter bytesConverter = new BytesConverter();
        private readonly IntConverter intConverter = new IntConverter();

        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out IEnumerable<byte> value)
        {
            intConverter.Deserialize(dWords.Take(1), out int length);
            bytesConverter.Deserialize(dWords.Skip(1).Take((length + 3) / 4), out IEnumerable<byte> paddedValue);
            value = paddedValue.Take(length);
            return dWords.Skip(1 + (length + 3) / 4);
        }

        public IEnumerable<DWord> Serialize(IEnumerable<byte> value)
        {
            return intConverter.Serialize(value.Count())
                .Concat(bytesConverter.Serialize(value));
        }
    }
}