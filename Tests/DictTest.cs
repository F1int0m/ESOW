using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ESOW.Tests
{

    [TestFixture]
    class DictTest
    {
        [Test]
        public static void Test()
        {
            var c =new DictWithTranslate();
            c.Add("dsadasт","ru-en", new Translator());
        }
    }
}
