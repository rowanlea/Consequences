using ConsequencesClientExample.Messaging;

namespace ConsequencesClientExample.Helpers
{
    public static class ResponseHelpers
    {
        public static bool ResponseGood(InboundResponse serverResponse)
        {
            return !serverResponse.Message.Contains("ERROR");
        }

        public static bool ResponseContainsQuestion(InboundResponse serverResponse)
        {
            return serverResponse.Question != null;
        }
    }
}
