﻿using ConsequencesClientExample.Game;
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
        string uri = "ws://0.0.0.0:1234";

        [SetUp]
        public void Setup()
        {
            throughput = Substitute.For<IThroughput>();
            socketClient = Substitute.For<ISocketClient>();

            throughput.TakeUserInput().Returns("Rowan", "Oak");

            socketClient.Receive().Returns(new InboundResponse { Message = "Give name and room code" }, new InboundResponse { Message = "Wait for players then answer question", Players = new List<string>() { "Rowan", "Finn" }, Question = "Question 1" });
        }

        [Test]
        public void WhenGameRun_SocketClientConnectCalled()
        {
            // Arrange
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
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            socketClient.Received(1).Send(name: "Rowan", room: "Oak");
        }

        [Test]
        public void WhenSocketClientReceivesAnyFurtherMessage_ThroughputOutputsThatMessage()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received(1).OutputToConsole("Wait for players then answer question");
        }
    }
}