using System;
using System.Net;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: PeerChat.exe <ip-address> <username>");
            Console.WriteLine("Example for testing on single machine:");
            Console.WriteLine("  PeerChat.exe 127.0.0.1 Alice");
            Console.WriteLine("  PeerChat.exe 127.0.0.2 Bob");
            Console.WriteLine("  PeerChat.exe 127.0.0.3 Charlie");
            return;
        }

        if (!IPAddress.TryParse(args[0], out var ipAddress))
        {
            Console.WriteLine($"Invalid IP address: {args[0]}");
            return;
        }

        string username = args[1];
        int tcpPort = 4546;
        int udpPort = 4545;

        for (int i = 2; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--tport":
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int tport))
                    {
                        tcpPort = tport;
                        i++;
                    }
                    else
                    {
                        Console.WriteLine("Invalid TCP port specified");
                        return;
                    }
                    break;

                case "--uport":
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int uport))
                    {
                        udpPort = uport;
                        i++;
                    }
                    else
                    {
                        Console.WriteLine("Invalid UDP port specified");
                        return;
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown argument: {args[i]}");
                    return;
            }
        }

        var chatNode = new ChatNode(ipAddress, username, tcpPort, udpPort);

        Console.WriteLine($"Chat node started as {username} on {ipAddress}. Type messages and press Enter to send.");
        Console.WriteLine($"Using TCP port: {tcpPort}, UDP port: {udpPort}");
        Console.WriteLine("Type 'exit' to quit.");

        while (true)
        {
            string message = Console.ReadLine();
            if (message?.ToLower() == "exit")
            {
                chatNode.Stop();
                break;
            }

            if (!string.IsNullOrEmpty(message))
            {
                chatNode.BroadcastMessage(message);
            }
        }
    }
}