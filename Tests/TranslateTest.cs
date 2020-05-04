using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ESOW.Tests
{
    [TestFixture]
    class TranslateTest
    {
        [Test]
        public void Test()
        {
            var test = new Translator();
            var s = test.Lookup("cat", "en-ru");
        }


    }
}
