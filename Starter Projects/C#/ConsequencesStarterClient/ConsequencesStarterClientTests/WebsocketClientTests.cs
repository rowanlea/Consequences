using ConsequencesStarterClient;

namespace ConsequencesStarterClientTests
{
    public class WebsocketClientTests
    {
        SocketClient socketClient;

        [SetUp]
        public void Setup()
        {
            socketClient = new SocketClient("ws://51.141.52.52:1234");
        }

        [Test]
        public void WhenISendAnInitialMessageToServer_ServerRespondsWithWelcomeMessage()
        {
            // Arrange
            socketClient.Connect();

            // Act
            socketClient.Send(new OutboundMessage { Hello = "Hello" });
            var response = socketClient.Receive();

            // Assert
            Assert.That(response.Message, Is.EqualTo("Welcome to Consequences, to get started send your name and room code."));
        }
    }
}