namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TimetagConverter : IConverter<Timetag>
    {
        private readonly ULongConverter converter = new ULongConverter();

        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out Timetag value)
        {
            var result = converter.Deserialize(dWords, out ulong ulongValue);
            value = new Timetag(ulongValue);
            return result;
        }

        public IEnumerable<DWord> Serialize(Timetag value)
        {
            return converter.Serialize(value.Tag);
        }
    }
}