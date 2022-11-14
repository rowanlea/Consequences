using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExampleTests.Messaging
{
    internal class InboundMessageParserTests
    {
        [Test]
        public void Parse_WhenGivenValidMessageJson_GetsMessageFromJson()
        {
            // Arrange
            var jsonResponse = "{\"Message\": \"Welcome to Consequences, to get started send your name and room code.\"}";

            // Act
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);

            // Assert
            Assert.That(response.Message, Is.EqualTo("Welcome to Consequences, to get started send your name and room code."));
        }

        [Test]
        public void Parse_WhenGivenValidFullJson_GetsMessageQuestionPlayersFromJson()
        {
            // Arrange
            var jsonResponse = "{\"Message\": \"Welcome to room 'Pizza'.Please answer your first question when all players have joined the room.\", \"Players\": [\"Rowan\"], \"Question\": \"Please enter an adjective, followed by a person's name\"}";

            // Act
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);

            // Assert
            Assert.That(response.Message, Is.EqualTo("Welcome to room 'Pizza'.Please answer your first question when all players have joined the room."));
            Assert.That(response.Players.Contains("Rowan"));
            Assert.That(response.Question, Is.EqualTo("Please enter an adjective, followed by a person's name"));
        }
    }
}
