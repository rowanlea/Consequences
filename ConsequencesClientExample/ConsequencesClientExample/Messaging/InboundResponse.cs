namespace ConsequencesClientExample.Messaging
{
    public class InboundResponse
    {
        public string Message { get; set; }
        public string Question { get; set; }
        public List<string> Players { get; set; }
        public List<string> Results { get; set; }
    }
}
