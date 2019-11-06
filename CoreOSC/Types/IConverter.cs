namespace CoreOSC.Types
{
    using System.Collections.Generic;

    public interface IConverter<T>
    {
        (T value, IEnumerable<DWord> dWords) Deserialize(IEnumerable<DWord> dWords);
        IEnumerable<DWord> Serialize(T value);
    }
}