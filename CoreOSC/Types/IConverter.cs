namespace CoreOSC.Types
{
    using System.Collections.Generic;

    public interface IConverter<T>
    {
        IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out T value);
        IEnumerable<DWord> Serialize(T value);
    }
}