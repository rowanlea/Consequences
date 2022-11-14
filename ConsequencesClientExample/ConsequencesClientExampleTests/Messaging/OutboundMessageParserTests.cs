using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExampleTests.Messaging
{
    internal class OutboundMessageParserTests
    {
        [Test]
        public void GetHelloMessage_WhenCalled_GetsString()
        {
            // Arrange and act
            string message = OutboundMessageParser.GetHelloMessage();
            
            // Assert
            Assert.That(message, Is.EqualTo("Hello"));
        }
    }
}
