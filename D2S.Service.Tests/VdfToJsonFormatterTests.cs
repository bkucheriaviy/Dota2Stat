using System.IO;
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
            var jsonLine = formatter.Format(vdfText);
            File.WriteAllText("./Resources/test.txt", jsonLine);
            //then
            Assert.That(jsonLine, Is.EqualTo(expectedJson));
        }

        [Test]
        [TestCase("\"DOTAUnits\" { \"Version\" \"1\" \"npc_dota_units_base\" \"value 2\" }", "{\"DOTAUnits\": { \"Version\": \"1\", \"npc_dota_units_base\": \"value 2\" }")]
        [TestCase("\"DOTAUnits\" { \"Version\" \"1\" \"npc_dota_units_base\" { \"key 1\" \"value 1\" \"key 2\" { \"key 3\" \"value 3\" }}", "{\"DOTAUnits\": { \"Version\": \"1\", \"npc_dota_units_base\": { \"key 1\": \"value 1\", \"key 2\": { \"key 3\": \"value 3\"}}")]
        public void Format_FormatsDvfToJson(string input, string expected)
        {
            //given
            var formatter = new VdfToJsonFormatter();

            //when
            var jsonLine = formatter.Format(input);

            //then
            Assert.That(jsonLine, Is.EqualTo(expected));
        }
    }
}