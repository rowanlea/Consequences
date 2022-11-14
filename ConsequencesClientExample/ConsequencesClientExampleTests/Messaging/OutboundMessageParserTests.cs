using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExampleTests.Messaging
{
    internal class OutboundMessageParserTests
    {
        [Test]
        public void GetHelloMessage_WhenCalled_GetsString()
        {
            string message = OutboundMessageParser.GetHelloMessage();
            Assert.That(message, Is.EqualTo("Hello"));
        }
    }
}
