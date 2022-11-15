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
        string uri = "ws://0.0.0.0:1234";

        [SetUp]
        public void Setup()
        {
            throughput = Substitute.For<IThroughput>();
            socketClient = Substitute.For<ISocketClient>();

            throughput.TakeUserInput().Returns("Rowan", "Oak",
                "Happy Henry",
                "Smiling Sam",
                "the supermarket",
                "eat cake",
                "lost their shoe",
                "solved a riddle",
                "zombies rose from the dead");

            socketClient.Receive().Returns(
                new InboundResponse { Message = "Give name and room code" },
                new InboundResponse { Message = "Wait for players then answer question", Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter an adjective, followed by a person's name" },
                new InboundResponse { Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter another adjective, followed by a different person's name" },
                new InboundResponse { Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter a place where people could meet" },
                new InboundResponse { Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter why they were there" },
                new InboundResponse { Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter something the first person did" },
                new InboundResponse { Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter something the second person did" },
                new InboundResponse { Players = new List<string>() { "Rowan", "Finn" }, Question = "Please enter the consequence of their actions" },
                new InboundResponse { Results = new List<string>() { "Happy Henry met Smiling Sam at the supermarket to eat cake. Henry lost their shoe, whilst Sam solved a riddle. The consequence of their actions was zombies rose from the dead." } }
                );
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
        public void WhenNameAndRoomInput_SocketClientSendsOutboundMessage()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            socketClient.Received(1).Send(name: "Rowan", room: "Oak");
        }

        [Test]
        public void WhenSocketClientReceivesAnyFurtherMessage_OutputsThatMessage()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received(1).OutputToConsole("Wait for players then answer question");
        }

        [Test]
        public void WhenSocketClientReceivesAnyFurtherMessage_OutputsPlayerList()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received().OutputToConsole("Players:");
            throughput.Received().OutputToConsole("Rowan");
            throughput.Received().OutputToConsole("Finn");
        }

        [Test]
        public void WhenSocketClientReceivesAnyFurtherMessage_OutputsQuestion_SendsAnswer()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received(1).OutputToConsole("Question: Please enter an adjective, followed by a person's name");
            socketClient.Received(1).Send(answer: "Happy Henry");
        }

        [Test]
        public void AllQuestionsAskedAndAnswered()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received(1).OutputToConsole("Question: Please enter an adjective, followed by a person's name");
            socketClient.Received(1).Send(answer: "Happy Henry");

            throughput.Received(1).OutputToConsole("Question: Please enter another adjective, followed by a different person's name");
            socketClient.Received(1).Send(answer: "Smiling Sam");

            throughput.Received(1).OutputToConsole("Question: Please enter a place where people could meet");
            socketClient.Received(1).Send(answer: "the supermarket");

            throughput.Received(1).OutputToConsole("Question: Please enter why they were there");
            socketClient.Received(1).Send(answer: "eat cake");

            throughput.Received(1).OutputToConsole("Question: Please enter something the first person did");
            socketClient.Received(1).Send(answer: "lost their shoe");

            throughput.Received(1).OutputToConsole("Question: Please enter something the second person did");
            socketClient.Received(1).Send(answer: "solved a riddle");

            throughput.Received(1).OutputToConsole("Question: Please enter the consequence of their actions");
            socketClient.Received(1).Send(answer: "zombies rose from the dead");
        }

        [Test]
        public void WhenAllQuestionsAnswered_OutputsFullAnswer()
        {
            // Arrange
            GameRunner gameRunner = new GameRunner(throughput, socketClient);

            // Act
            gameRunner.Start(uri);

            // Assert
            throughput.Received().OutputToConsole("Happy Henry met Smiling Sam at the supermarket to eat cake. Henry lost their shoe, whilst Sam solved a riddle. The consequence of their actions was zombies rose from the dead.");
        }
    }
}