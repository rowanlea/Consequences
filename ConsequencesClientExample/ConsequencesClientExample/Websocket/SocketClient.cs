using System.Collections.Concurrent;
using System.Text.Json;
using Websocket.Client;

namespace ConsequencesClientExample.Websocket
{
    public class SocketClient
    {
        private ManualResetEvent _exitEvent;
        private WebsocketClient _client;
        internal BlockingCollection<string> _responseList;

        public SocketClient()
        {
            _responseList = new BlockingCollection<string>();
        }

        public async void Connect(string address)
        {
            Uri uri = new Uri(address);
            _client = new WebsocketClient(uri);

            _client.MessageReceived.Subscribe(response => _responseList.Add(response.Text));
            await _client.Start();

            _exitEvent = new ManualResetEvent(false);
            _exitEvent.WaitOne();
        }

        public void Send(object message)
        {
            var serialisedMessage = JsonSerializer.Serialize(message);
            _client.Send(serialisedMessage);
        }

        public string Receive()
        {
            var response = _responseList.Take();
            return response;
        }
    }
}
