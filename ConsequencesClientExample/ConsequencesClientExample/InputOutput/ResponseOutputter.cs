using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExample.InputOutput
{
    internal class ResponseOutputter
    {
        private IThroughput _throughput;
        public ResponseOutputter(IThroughput throughput)
        {
            _throughput = throughput;
        }

        public void OutputFinalResponses(InboundResponse finalResponse)
        {
            if (finalResponse.Results != null)
            {
                foreach (var result in finalResponse.Results)
                {
                    _throughput.OutputToConsole(result);
                }
            }
        }

        public void OutputQuestion(InboundResponse serverResponse)
        {
            _throughput.OutputToConsole($"Question: {serverResponse.Question}");
        }

        public void OutputPlayerList(InboundResponse response)
        {
            if (response.Players.Count > 0)
            {
                string players = "Players: ";
                foreach (var playerName in response.Players)
                {
                    players += $"{playerName}, ";
                }
                players = players.TrimEnd(' ');
                players = players.TrimEnd(',');
                _throughput.OutputToConsole(players);
            }
        }

        public void OutputMessage(InboundResponse serverResponse)
        {
            if (serverResponse.Message != null)
            {
                _throughput.OutputToConsole(serverResponse.Message);
            }
        }
    }
}
