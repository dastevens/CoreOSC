namespace CoreOSC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public struct OscMessage
    {
        public OscMessage(Address address, IEnumerable<object> arguments)
        {
            this.Address = address;
            this.Arguments = arguments;
        }

        public Address Address { get; }

        public IEnumerable<object> Arguments { get; }
    }
}