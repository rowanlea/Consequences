using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExampleTests.Messaging
{
    internal class InboundMessageParserTests
    {
        [Test]
        public void Parse_WhenGivenValidJson_GetsMessageFromJson()
        {
            // Arrange
            var jsonResponse = "{\"Message\": \"Welcome to Consequences, to get started send your name and room code.\"}";

            // Act
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);

            // Assert
            Assert.That(response.Message, Is.EqualTo("Welcome to Consequences, to get started send your name and room code."));
        }
    }
}
