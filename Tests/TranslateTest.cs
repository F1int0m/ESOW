using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;
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
            var s = test.Lookup("car", "en-ru");
            //var s1 = test.Lookup("кот", "ru-en");
        }


    }
}
