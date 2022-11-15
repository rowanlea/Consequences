namespace ConsequencesClientExample.InputOutput
{
    public interface IThroughput
    {
        string TakeUserInput(string messageToUser);
        void OutputToConsole(string message);
    }
}
