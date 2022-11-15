﻿using ConsequencesClientExample.Game;
using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Websocket;
using NSubstitute;

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
    }
}