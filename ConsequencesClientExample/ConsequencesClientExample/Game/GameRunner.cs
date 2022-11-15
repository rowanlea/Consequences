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

            InboundResponse serverResponse = null;
            while (true)
            {
                RoomSetup();
                serverResponse = _socketClient.Receive();
                if (!serverResponse.Message.Contains("ERROR"))
                {
                    break;
                }
                _throughput.OutputToConsole(serverResponse.Message);
            }

            var totalQuestions = 0;

            while (totalQuestions < 7)
            {
                if (serverResponse.Question != null)
                {
                    OutputPlayerList(serverResponse);
                    OutputMessage(serverResponse);
                    OutputQuestion(serverResponse);
                    var playerResponse = _throughput.TakeUserInput();
                    _socketClient.Send(answer: playerResponse);
                    totalQuestions++;
                }
                else
                {
                    OutputMessage(serverResponse);
                    var playerResponse = _throughput.TakeUserInput();
                    _socketClient.Send(answer: playerResponse);
                }
                serverResponse = _socketClient.Receive();
            }

            OutputFinalResponses(serverResponse);
        }

        private void OutputFinalResponses(InboundResponse finalResponse)
        {
            if (finalResponse.Results != null)
            {
                foreach (var result in finalResponse.Results)
                {
                    _throughput.OutputToConsole(result);
                }
            }
        }

        private void OutputQuestion(InboundResponse serverResponse)
        {
            _throughput.OutputToConsole($"Question: {serverResponse.Question}");
        }

        private void OutputMessage(InboundResponse serverResponse)
        {
            if (serverResponse.Message != null)
            {
                _throughput.OutputToConsole(serverResponse.Message);
            }
        }

        private void RoomSetup()
        {
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

            var initialResponse = _socketClient.Receive();
            _throughput.OutputToConsole(initialResponse.Message);
        }

        private void OutputPlayerList(InboundResponse response)
        {
            if (response.Players.Count > 0)
            {
                _throughput.OutputToConsole("Players:");
                foreach (var playerName in response.Players)
                {
                    _throughput.OutputToConsole(playerName);
                }
            }
        }
    }
}
