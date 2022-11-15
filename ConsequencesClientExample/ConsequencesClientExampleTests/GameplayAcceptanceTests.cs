using ConsequencesClientExample.Game;
using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Messaging;
using ConsequencesClientExample.Websocket;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NSubstitute;
using System.Reactive;

namespace ConsequencesClientExampleTests
{
    internal class GameplayAcceptanceTests
    {
        IThroughput throughput;
        ISocketClient socketClient;
        [SetUp]
        public void Setup()
        {
            throughput = Substitute.For<IThroughput>();
            socketClient = Substitute.For<ISocketClient>();

            throughput.TakeUserInput().Returns("Rowan", "Oak");

            socketClient.Receive().Returns(new InboundResponse { Message = "Give name and room code" });
        }

        [Test]
        public void WhenGameRun_SocketClientConnectCalled()
        {
            // Arrange
            string uri = "ws://0.0.0.0:1234";
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            socketClient.Received(1).Connect(uri);
        }

        [Test]
        public void WhenGameRun_SocketClientSendCalledWithHello()
        {
            // Arrange
            string uri = "ws://0.0.0.0:1234";
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            socketClient.Received(1).Send(start: "Hello");
        }

        [Test]
        public void WhenSocketClientReceivedCalled_OutputReceivesSameMessage()
        {
            // Arrange
            string uri = "ws://0.0.0.0:1234";
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received(1).OutputToConsole("Give name and room code");
        }

        [Test]
        public void WhenThroughputReceivesNameAndRoom_SocketClientSendsOutboundMessage()
        {
            // Arrange
            string uri = "ws://0.0.0.0:1234";
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            socketClient.Received(1).Send(name: "Rowan", room: "Oak");
        }
    }
}