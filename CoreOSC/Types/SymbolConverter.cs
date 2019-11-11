namespace CoreOSC.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SymbolConverter : IConverter<Symbol>
    {
        private readonly StringConverter converter = new StringConverter();

        public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out Symbol value)
        {
            var result = converter.Deserialize(dWords, out string internalValue);
            value = new Symbol(internalValue);
            return result;
        }

        public IEnumerable<DWord> Serialize(Symbol value)
        {
            return converter.Serialize(value.Value);
        }
    }
}