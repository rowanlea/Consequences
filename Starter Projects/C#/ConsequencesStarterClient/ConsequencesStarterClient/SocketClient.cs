using System.Collections.Concurrent;
using System.Text.Json;
using Websocket.Client;

namespace ConsequencesStarterClient
{
    public class SocketClient
    {
        private Uri _uri;
        private ManualResetEvent exitEvent;
        private WebsocketClient _client;
        private BlockingCollection<string> _messageList;

        /// <summary>
        /// Sets up private objects to be used by the class.
        /// </summary>
        /// <param name="uri">The Websocket address of the server</param>
        public SocketClient(string uri)
        {
            _uri = new Uri(uri);
            exitEvent = new ManualResetEvent(false);
            _client = new WebsocketClient(_uri);
            _messageList = new BlockingCollection<string>();
        }

        /// <summary>
        /// Unlike the other methods the start method is asynchronous, however you don't need to call it asynchronously.
        /// </summary>
        public async void Connect()
        {
            _client.MessageReceived.Subscribe(msg => _messageList.Add(msg.Text));
            await _client.Start();
            exitEvent.WaitOne();
        }

        /// <summary>
        /// Sends a message to the server. This library doesn't have an asynchronous method for this, but that will just make it easier to TDD.
        /// </summary>
        /// <param name="message">Any object can be passed to send, as long as it is JSON serializable.</param>
        public void Send(object message)
        {
            var serialisedMessage = JsonSerializer.Serialize(message);
            _client.Send(serialisedMessage);
        }

        /// <summary>
        /// Used to retrieve a received message from the message list. This call will block.
        /// </summary>
        /// <returns>
        /// Returns an InboundMessage object parsed from the server's JSON response.
        /// </returns>
        public InboundMessage Receive()
        {
            var message = _messageList.Take();
            InboundMessage? parsedMessage = JsonSerializer.Deserialize<InboundMessage>(message);
            return parsedMessage ?? new InboundMessage();
        }

        /// <summary>
        /// Used to manually disconnect from the server.
        /// If you do not use this the garbage collector will handle it for you.
        /// </summary>
        public void Disconnect()
        {
            _client.Dispose();
        }
    }
}
