using CoreOSC.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreOSC.Test.Types
{
    class OscBundleTest
    {
        [Test]
        public void Deserialize()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Serialize()
        {
            var value = new OscBundle();

            var sut = new OscBundleConverter();
            var result = sut.Serialize(value);
        }
    }
}
