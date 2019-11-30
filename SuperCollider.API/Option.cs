using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCollider.API
{
    public class Option<A, B>
    {
        public Option(A a)
        {
            this.Value = a;
        }

        public Option(B b)
        {
            this.Value = b;
        }

        public object Value { get; }
    }
}
