namespace ConsequencesClientExample.InputOutput
{
    public class ConsoleThroughput : IThroughput
    {
        public void OutputToConsole(string message)
        {
            Console.WriteLine(message);
        }

        public string TakeUserInput()
        {
            string input = "";
            input += Console.ReadLine();
            return input;
        }
    }
}
