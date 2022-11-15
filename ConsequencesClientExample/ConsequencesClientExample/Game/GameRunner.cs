using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Messaging;
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
            InitialConnection(uri);
            RoomSetup();

            var serverResponse = _socketClient.Receive();
            _throughput.OutputToConsole(serverResponse.Message);
            OutputPlayerList(serverResponse);
            _throughput.OutputToConsole($"Question: {serverResponse.Question}");
            var playerResponse = _throughput.TakeUserInput();
            _socketClient.Send(answer: playerResponse);
        }

        private void RoomSetup()
        {
            var setupResponse = _socketClient.Receive();
            _throughput.OutputToConsole(setupResponse.Message);

            _throughput.OutputToConsole("Name:");
            var nameInput = _throughput.TakeUserInput();
            _throughput.OutputToConsole("Room:");
            var roomInput = _throughput.TakeUserInput();

            _socketClient.Send(name: nameInput, room: roomInput);
        }

        private void InitialConnection(string uri)
        {
            _socketClient.Connect(uri);
            _socketClient.Send(start: "Hello");
        }

        private void OutputPlayerList(InboundResponse response)
        {
            if (response.Players.Count > 0)
            {
                _throughput.OutputToConsole("Players:");
                foreach(var playerName in response.Players)
                {
                    _throughput.OutputToConsole(playerName);
                }
            }
        }
    }
}
