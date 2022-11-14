using ConsequencesClientExample.Messaging;
using ConsequencesClientExample.Websocket;

namespace ConsequencesClientExampleTests
{
    internal class WebsocketAcceptanceTests
    {
        SocketClient socketClient;

        [SetUp]
        public void Setup()
        {
            socketClient = new SocketClient();
        }

        [Test]
        public void WhenISendAnInitialMessageToServer_ServerRespondsWithWelcomeMessage()
        {
            // Arrange and Act
            socketClient.Connect("ws://51.141.52.52:1234");
            socketClient.Send(start: "Hello");
            string jsonResponse = socketClient.Receive();
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.Not.Null);
            Assert.That(response.Message.Length, Is.GreaterThan(0));
            Assert.That(response.Message, Is.EqualTo("Welcome to Consequences, to get started send your name and room code."));
        }

        [Test]
        public void WhenISendANameAndRoomCode_ServerRespondsWithMessageAndQuestion()
        {
            // Arrange
            socketClient.Connect("ws://51.141.52.52:1234");
            socketClient.Send(start: "Hello");
            socketClient.Receive();
            socketClient.Send(name: "Rowan", room: "Pizza");

            // Act
            string jsonResponse = socketClient.Receive();
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.Not.Null);
            Assert.That(response.Message.Length, Is.GreaterThan(0));
            Assert.That(response.Question, Is.Not.Null);
            Assert.That(response.Question.Length, Is.GreaterThan(0));
            Assert.That(response.Players.Count, Is.GreaterThan(0));
            Assert.That(response.Players.Contains("Rowan"));
        }

        [Test]
        public void WhenISendAnswer_ServerRespondsWithNextQuestion()
        {
            // Arrange
            socketClient.Connect("ws://51.141.52.52:1234");
            socketClient.Send(start: "Hello");
            socketClient.Receive();
            socketClient.Send(name: "Rowan", room: "Hotdog");

            // Act
            string firstResponse = socketClient.Receive();
            string firstQuestion = InboundResponseParser.Parse(firstResponse).Question;

            socketClient.Send(answer: "Happy Henry");
            string secondResponse = socketClient.Receive();
            string secondQuestion = InboundResponseParser.Parse(secondResponse).Question;

            // Assert
            Assert.That(firstQuestion.Length, Is.GreaterThan(0));
            Assert.That(secondQuestion.Length, Is.GreaterThan(0));
            Assert.That(firstQuestion, Is.Not.EqualTo(secondQuestion));
        }
    }
}