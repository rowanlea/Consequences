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
            var response = socketClient.Receive();

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
            var response = socketClient.Receive();

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
            var firstQuestion = socketClient.Receive().Question;
            socketClient.Send(answer: "Happy Henry");
            var secondQuestion = socketClient.Receive().Question;

            // Assert
            Assert.That(firstQuestion.Length, Is.GreaterThan(0));
            Assert.That(secondQuestion.Length, Is.GreaterThan(0));
            Assert.That(firstQuestion, Is.Not.EqualTo(secondQuestion));
        }

        [Test]
        public void WhenISendAllAnswers_ServerRespondsWithCompletedAnswer()
        {
            socketClient.Connect("ws://51.141.52.52:1234");
            socketClient.Send(start: "Hello");
            socketClient.Receive();
            socketClient.Send(name: "Rowan", room: "Burger");
            socketClient.Receive();
            socketClient.Send(answer: "Happy Henry");
            socketClient.Receive();
            socketClient.Send(answer: "Smiling Sam");
            socketClient.Receive();
            socketClient.Send(answer: "the supermarket");
            socketClient.Receive();
            socketClient.Send(answer: "eat cake");
            socketClient.Receive();
            socketClient.Send(answer: "lost their shoe");
            socketClient.Receive();
            socketClient.Send(answer: "solved a riddle");
            socketClient.Receive();
            socketClient.Send(answer: "zombies rose from the dead");

            var finalAnswer = socketClient.Receive();

            Assert.That(finalAnswer.Results.Contains("Happy Henry met Smiling Sam at the supermarket to eat cake. Henry lost their shoe, whilst Sam solved a riddle. The consequence of their actions was zombies rose from the dead."));
        }
    }
}