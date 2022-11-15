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
            _socketClient.Send(start: "Hello");

            var response = _socketClient.Receive();
            _throughput.OutputToConsole(response.Message);

            _throughput.OutputToConsole("Name:");
            var nameInput = _throughput.TakeUserInput();
            _throughput.OutputToConsole("Room:");
            var roomInput = _throughput.TakeUserInput();

            _socketClient.Send(name: nameInput, room: roomInput);

        }
    }
}
