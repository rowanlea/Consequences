using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExampleTests.Messaging
{
    internal class InboundMessageParserTests
    {
        [Test]
        public void Parse_WhenGivenValidJson_GetsMessageFromJson()
        {
            var jsonResponse = "{\"Message\": \"Welcome to Consequences, to get started send your name and room code.\"}";
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);
            Assert.That(response.Message, Is.EqualTo("Welcome to Consequences, to get started send your name and room code."));
        }
    }
}
