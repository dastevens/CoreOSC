namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;

    public interface IConverter
    {
        Type ForType { get; }

        IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out object value);

        IEnumerable<DWord> Serialize(object value);
    }

    public interface IConverter<T>
    {
        IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out T value);

        IEnumerable<DWord> Serialize(T value);
    }

    public class Converter<T> : IConverter
    {
        private readonly IConverter<T> typedConverter;

        public Type ForType => typeof(T);

        public Converter(IConverter<T> typedConverter)
        {
            this.typedConverter = typedConverter;
        }

        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out object value)
        {
            var result = typedConverter.Deserialize(dWords, out var typedValue);
            value = typedValue;
            return result;
        }

        public IEnumerable<DWord> Serialize(object value)
        {
            return typedConverter.Serialize((T)value);
        }
    }
}