using ConsequencesClientExample.Game;
using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Messaging;
using ConsequencesClientExample.Websocket;
using NSubstitute;
using System.Reactive;

namespace ConsequencesClientExampleTests
{
    internal class GameplayAcceptanceTests
    {
        [Test]
        public void WhenGameRun_SocketClientConnectCalled()
        {
            // Arrange
            string uri = "ws://0.0.0.0:1234";
            IThroughput throughput = Substitute.For<IThroughput>();
            ISocketClient socketClient = Substitute.For <ISocketClient>();
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
            IThroughput throughput = Substitute.For<IThroughput>();
            ISocketClient socketClient = Substitute.For<ISocketClient>();
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act, plus pre-arranged acting through mocks
            gameRunner.Start(uri);

            // Assert
            socketClient.Received(1).Send(start: "Hello");
        }

        [Test]
        public void WhenGameRun_AndSocketClientReceivedCalled_OutputReceivesSameMessage()
        {
            // Arrange
            string uri = "ws://0.0.0.0:1234";
            IThroughput throughput = Substitute.For<IThroughput>();
            ISocketClient socketClient = Substitute.For<ISocketClient>();
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act, plus pre-arranged acting through mocks
            socketClient.Receive().Returns(new InboundResponse { Message =  "Give name and room code"});
            gameRunner.Start(uri);

            // Assert
            throughput.Received(1).OutputToConsole("Give name and room code");
        }
    }
}