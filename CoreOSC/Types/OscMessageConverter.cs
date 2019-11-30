namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OscMessageConverter : IConverter<OscMessage>
    {
        struct TypeConverter
        {
            public TypeConverter(char ch, IConverter converter)
            {
                this.Char = ch;
                this.Converter = converter;
            }

            public char Char { get; }

            public IConverter Converter { get; }
        }

        private static readonly TypeConverter[] typeConverters = new[]
        {
            new TypeConverter('i', new Converter<int>(new IntConverter())),
            new TypeConverter('f', new Converter<float>(new FloatConverter())),
            new TypeConverter('s', new Converter<string>(new StringConverter())),
            new TypeConverter('b', new Converter<IEnumerable<byte>>(new BlobConverter())),
            new TypeConverter('h', new Converter<long>(new LongConverter())),
            new TypeConverter('t', new Converter<Timetag>(new TimetagConverter())),
            new TypeConverter('d', new Converter<double>(new DoubleConverter())),
            new TypeConverter('S', new Converter<Symbol>(new SymbolConverter())),
            new TypeConverter('c', new Converter<char>(new CharConverter())),
            new TypeConverter('r', new Converter<RGBA>(new RGBAConverter())),
            new TypeConverter('m', new Converter<Midi>(new MidiConverter())),
            new TypeConverter('T', new Converter<OscTrue>(new OscTrueConverter())),
            new TypeConverter('F', new Converter<OscFalse>(new OscFalseConverter())),
            new TypeConverter('N', new Converter<OscNil>(new OscNilConverter())),
            new TypeConverter('I', new Converter<OscInfinitum>(new OscInfinitumConverter())),
            //new TypeConverter('[', new ArrayConverter())
        };

        private static readonly StringConverter stringConverter = new StringConverter();

        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscMessage value)
        {
            var afterAddress = DeserializeAddress(dWords, out var address);
            var afterType = DeserializeTypes(afterAddress, out var typeConverters);
            var afterArguments = DeserializeArguments(afterType, typeConverters, out var arguments);
            value = new OscMessage(address, arguments);
            return afterArguments;
        }

        public IEnumerable<DWord> Serialize(OscMessage value)
        {
            return
                SerializeAddress(value.Address)
                .Concat(SerializeTypes(value.Arguments))
                .Concat(SerializeArguments(value.Arguments));
        }

        private IEnumerable<DWord> SerializeAddress(Address value)
        {
            return stringConverter.Serialize(value.Value);
        }

        private IEnumerable<DWord> DeserializeAddress(IEnumerable<DWord> dWords, out Address value)
        {
            var result = stringConverter.Deserialize(dWords, out string stringValue);
            value = new Address(stringValue);
            return result;
        }

        private IEnumerable<DWord> SerializeTypes(IEnumerable<object> arguments)
        {
            return stringConverter.Serialize(
                "," + string.Join(
                    string.Empty,
                    arguments.Select(
                        argument => typeConverters.Single(
                            typeConverter => typeConverter.Converter.ForType == argument.GetType())
                        .Char.ToString())));
        }

        private IEnumerable<DWord> DeserializeTypes(IEnumerable<DWord> dWords, out IEnumerable<IConverter> value)
        {
            var result = stringConverter.Deserialize(dWords, out string typeString);
            value = typeString
                .SkipWhile(ch => ch == ',')
                .Select(ch => typeConverters.Single(
                            typeConverter => typeConverter.Char == ch)
                        .Converter);
            return result;
        }

        private IEnumerable<DWord> SerializeArguments(IEnumerable<object> arguments)
        {
            var result = new List<DWord>();
            foreach (var argument in arguments)
            {
                var typeConverter = typeConverters.Single(
                            tc => tc.Converter.ForType == argument.GetType());
                var dWords = typeConverter.Converter.Serialize(argument);
                result.AddRange(dWords);
            }

            return result;
        }

        private IEnumerable<DWord> DeserializeArguments(IEnumerable<DWord> dWords, IEnumerable<IConverter> typeConverters, out IEnumerable<object> values)
        {
            var valuesList = new List<object>();
            foreach (var typeConverter in typeConverters)
            {
                dWords = typeConverter.Deserialize(dWords, out object value);
                valuesList.Add(value);
            }

            values = valuesList;
            return dWords;
        }
    }
}
