namespace ConsequencesClientExample.Messaging
{
    public class OutboundMessageParser
    {
        public static string GetHelloMessage()
        {
            return "Hello";
        }

        internal static OutboundMessage GetSetupMessage(string name, string room)
        {
            OutboundMessage message = new OutboundMessage();
            message.Name = name;
            message.Room = room;
            return message;
        }
    }
}
