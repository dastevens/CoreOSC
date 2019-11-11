namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OscBundleConverter : IConverter<OscBundle>
    {
        private readonly StringConverter stringConverter = new StringConverter();
        private readonly TimetagConverter timetagConverter = new TimetagConverter();
        private readonly OscMessageConverter messageConverter = new OscMessageConverter();

        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscBundle value)
        {
            var afterBundleHeader = DeserializeBundleHeader(dWords);
            var afterTimetag = DeserializeTimetag(afterBundleHeader, out var timetag);
            var afterMessages = DeserializeMessages(afterTimetag, out var messages);
            value = new OscBundle(timetag, messages);
            return afterMessages;
        }

        public IEnumerable<DWord> Serialize(OscBundle value)
        {
            return
                SerializeBundleHeader()
                .Concat(SerializeTimetag(value.Timetag))
                .Concat(SerializeMessages(value.Messages));
        }

        private IEnumerable<DWord> DeserializeBundleHeader(IEnumerable<DWord> dWords)
        {
            return stringConverter.Deserialize(dWords, out string _);
        }

        private IEnumerable<DWord> SerializeBundleHeader()
        {
            return stringConverter.Serialize("#bundle");
        }

        private IEnumerable<DWord> DeserializeTimetag(IEnumerable<DWord> dWords, out Timetag timetag)
        {
            return timetagConverter.Deserialize(dWords, out timetag);
        }

        private IEnumerable<DWord> SerializeTimetag(Timetag timetag)
        {
            return timetagConverter.Serialize(timetag);
        }

        private IEnumerable<DWord> DeserializeMessages(IEnumerable<DWord> dWords, out IEnumerable<OscMessage> messages)
        {
            var result = new List<OscMessage>();
            while (dWords.Any())
            {
                dWords = messageConverter.Deserialize(dWords, out OscMessage message);
                result.Add(message);
            }

            messages = result;
            return dWords;
        }

        private IEnumerable<DWord> SerializeMessages(IEnumerable<OscMessage> messages)
        {
            return messages.Select(message => messageConverter.Serialize(message)).SelectMany(dWord => dWord);
        }
    }
}
