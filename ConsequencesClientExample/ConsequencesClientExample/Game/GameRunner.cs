using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Websocket;

namespace ConsequencesClientExample.Game
{
    public class GameRunner
    {
        private IThroughput _throughput;
        private ISocketClient _socketClient;

        public GameRunner(IThroughput throughput, ISocketClient socketClient)
        {
            _throughput = throughput;
            _socketClient = socketClient;
        }

        public void Start(string uri)
        {
            _socketClient.Connect(uri);

            var input = _throughput.TakeUserInput();
            _socketClient.Send(start: input);

            var response = _socketClient.Receive();
            _throughput.OutputToConsole(response.Message);
        }
    }
}
