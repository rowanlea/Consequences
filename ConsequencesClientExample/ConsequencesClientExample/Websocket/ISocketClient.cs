using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExample.Websocket
{
    public interface ISocketClient
    {
        public void Connect(string address);
        public void Send(string start = "", string name = "", string room = "", string answer = "");
        public InboundResponse Receive();
    }
}
