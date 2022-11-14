using System.Text.Json;

namespace ConsequencesClientExample.Messaging
{
    public class InboundResponseParser
    {
        public static InboundResponse Parse(string jsonResponse)
        {
            InboundResponse? parsedResponse = JsonSerializer.Deserialize<InboundResponse>(jsonResponse);
            return parsedResponse ?? new InboundResponse();
        }
    }
}
