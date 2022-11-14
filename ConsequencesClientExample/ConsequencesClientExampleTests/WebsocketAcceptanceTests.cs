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
            socketClient.Connect("ws://51.141.52.52:1234");
        }

        [Test]
        public void WhenISendAnInitialMessageToServer_ServerRespondsWithWelcomeMessage()
        {
            // Arrange
            string helloMessage = OutboundMessageParser.GetHelloMessage();

            // Act
            socketClient.Send(helloMessage);
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
            string helloMessage = OutboundMessageParser.GetHelloMessage();
            OutboundMessage setupMessage = OutboundMessageParser.GetSetupMessage("Rowan", "Pizza");

            // Act
            socketClient.Send(helloMessage);
            socketClient.Receive();
            socketClient.Send(setupMessage);

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
            string helloMessage = OutboundMessageParser.GetHelloMessage();
            OutboundMessage setupMessage = OutboundMessageParser.GetSetupMessage("Rowan", "Hotdog");
            OutboundMessage answerMessage = OutboundMessageParser.GetAnswerMessage("Happy Henry");

            // Act
            socketClient.Send(helloMessage);
            socketClient.Receive();
            socketClient.Send(setupMessage);

            string firstResponse = socketClient.Receive();
            string firstQuestion = InboundResponseParser.Parse(firstResponse).Question;

            socketClient.Send(answerMessage);
            string secondResponse = socketClient.Receive();
            string secondQuestion = InboundResponseParser.Parse(secondResponse).Question;

            // Assert
            Assert.That(firstQuestion.Length, Is.GreaterThan(0));
            Assert.That(secondQuestion.Length, Is.GreaterThan(0));
            Assert.That(firstQuestion, Is.Not.EqualTo(secondQuestion));
        }
    }
}