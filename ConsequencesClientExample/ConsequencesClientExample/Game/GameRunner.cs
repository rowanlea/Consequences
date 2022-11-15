using ConsequencesClientExample.Helpers;
using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Messaging;
using ConsequencesClientExample.Websocket;

namespace ConsequencesClientExample.Game
{
    public class GameRunner
    {
        private IThroughput _throughput;
        private ISocketClient _socketClient;
        private ResponseOutputter _responseOutputter;

        public GameRunner(IThroughput throughput, ISocketClient socketClient)
        {
            _throughput = throughput;
            _socketClient = socketClient;
            _responseOutputter = new ResponseOutputter(_throughput);
        }

        public void Start(string uri)
        {
            InitialConnection(uri);
            InboundResponse setupResponse = RoomSetup();
            InboundResponse finalResponse = QuestionsLoop(setupResponse);
            _responseOutputter.OutputFinalResponses(finalResponse);
        }
        private void InitialConnection(string uri)
        {
            _socketClient.Connect(uri);
            _socketClient.Send(start: "Hello");

            var initialResponse = _socketClient.Receive();
            _throughput.OutputToConsole(initialResponse.Message);
        }

        private InboundResponse RoomSetup()
        {
            InboundResponse serverResponse;

            while (true)
            {
                SendNameAndRoom();
                serverResponse = _socketClient.Receive();

                // If there was no error message
                if (ResponseHelpers.ResponseGood(serverResponse))
                    break;

                // Output the error message
                _throughput.OutputToConsole(serverResponse.Message);
            }

            return serverResponse;
        }

        private void SendNameAndRoom()
        {
            _throughput.OutputToConsole("Name:");
            var nameInput = _throughput.TakeUserInput();

            _throughput.OutputToConsole("Room:");
            var roomInput = _throughput.TakeUserInput();

            _socketClient.Send(name: nameInput, room: roomInput);
        }

        private InboundResponse QuestionsLoop(InboundResponse serverResponse)
        {
            var totalQuestions = 0;

            while (totalQuestions < 7)
            {
                if (ResponseHelpers.ResponseContainsQuestion(serverResponse))
                {
                    QuestionReceived(serverResponse);
                    totalQuestions++;
                }
                else
                {
                    // Either an error or a 'waiting' message
                    QuestionNotReceived(serverResponse);
                }
                serverResponse = _socketClient.Receive();
            }

            return serverResponse;
        }

        private void QuestionReceived(InboundResponse serverResponse)
        {
            _responseOutputter.OutputPlayerList(serverResponse);
            _responseOutputter.OutputMessage(serverResponse);
            _responseOutputter.OutputQuestion(serverResponse);
            var playerResponse = _throughput.TakeUserInput();
            _socketClient.Send(answer: playerResponse);
        }

        private void QuestionNotReceived(InboundResponse serverResponse)
        {
            _responseOutputter.OutputMessage(serverResponse);

            if (!ResponseHelpers.ResponseGood(serverResponse))
            {
                // Player needs to answer again
                var playerResponse = _throughput.TakeUserInput();
                _socketClient.Send(answer: playerResponse);
            }

        }
    }
}
