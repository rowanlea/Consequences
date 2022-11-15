// See https://aka.ms/new-console-template for more information
using ConsequencesClientExample.Game;
using ConsequencesClientExample.InputOutput;
using ConsequencesClientExample.Websocket;

IThroughput throughput = new ConsoleThroughput();
ISocketClient socketClient = new SocketClient();
GameRunner gameRunner = new GameRunner(throughput, socketClient);
gameRunner.Start("ws://51.141.52.52:1234");

Console.WriteLine("Press any key to end.");
Console.ReadKey();