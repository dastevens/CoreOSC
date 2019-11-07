namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MidiConverter : IConverter<Midi>
    {
        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out Midi value)
        {
            var dWord = dWords.First();
            value = new Midi(dWord.Byte0, dWord.Byte1, dWord.Byte2, dWord.Byte3);
            return dWords.Skip(1);
        }

        public IEnumerable<DWord> Serialize(Midi value)
        {
            yield return new DWord(value.Port, value.Status, value.Data1, value.Data2);
        }
    }
}