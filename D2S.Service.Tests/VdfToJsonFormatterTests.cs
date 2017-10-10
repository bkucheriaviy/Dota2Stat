using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace D2S.Service.Tests
{
    [TestFixture]
    public class VdfToJsonFormatterTests
    {
        [Test]
        public void FormatLine_FormatsVdfLineToJson()
        {
            //given  
            var vdfText = File.ReadAllText(@"./Resources/sample.vdf");
            var expectedJson = File.ReadAllText(@"./Resources/sample.json");
            var formatter = new VdfToJsonFormatter();

            //when
            var jsonLine = formatter.FormatLine(vdfText);

            //then
            Assert.That(jsonLine, Is.EqualTo(expectedJson));
        }

        [Test]
        public void Format_GeneratesValidJson()
        {
            //given
            var formatter = new VdfToJsonFormatter();

            //when
            var result = formatter.Format(File.ReadAllText("./Resources/sample.dvf"));
            var json = JsonConvert.DeserializeObject<Dota2Item>(result);

            //then
            Assert.That(json, Is.Not.Null);
        }
    }
}