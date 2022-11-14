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
            // Arrange
            string helloMessage = OutboundMessageParser.GetHelloMessage();

            // Act
            socketClient.Connect("ws://51.141.52.52:1234");
            socketClient.Send(helloMessage);
            string jsonResponse = socketClient.Receive();
            InboundResponse response = InboundResponseParser.Parse(jsonResponse);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.Not.Null);
            Assert.That(response.Message.Length > 0);
            Assert.That(response.Message, Is.EqualTo("Welcome to Consequences, to get started send your name and room code."));
        }
    }
}