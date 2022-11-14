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
            var helloMessage = MessageParser.GetHelloMessage();

            // Act
            socketClient.Connect("ws://51.141.52.52:1234");
            socketClient.Send(helloMessage);
            InboundMessage response = socketClient.Receive();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message.Length > 0);
        }
    }
}