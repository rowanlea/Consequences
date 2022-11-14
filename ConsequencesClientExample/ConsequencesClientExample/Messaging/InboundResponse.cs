namespace ConsequencesClientExample.Messaging
{
    public class InboundResponse
    {
        public string Message { get; set; }
        public string Question { get; internal set; }
        public List<string> Players { get; internal set; }
    }
}
