using System.Net;
using System.Net.Sockets;
using System.Text;

try
{
    string ipPowerMetter = "192.168.100.254";
    int port = 11;
    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    s.ReceiveTimeout = 2000;
    s.SendTimeout = 2000;

    float wifiPwrDB = 0, max = 0;

    IPAddress broadcast = IPAddress.Parse(ipPowerMetter);

    IPEndPoint ep = new IPEndPoint(broadcast, 80);
    bool Test = true;
    Console.WriteLine("Press any key for retry test.\n\rpress ESC key for exit test...\n\rPress p for change TEst port and enter port:[PORT NUMBER] for change port Test");
    Console.WriteLine("---------------------------");
    Console.WriteLine("PM Device ip:" + ipPowerMetter);
    Console.WriteLine("PM Device Port:" + port);
    while (Test)
    {
        byte[] sendbuf = Encoding.ASCII.GetBytes("SWADVR " + port);
        s.SendTo(sendbuf, ep);
        byte[] bytes = new byte[20];

        s.Receive(bytes, 0, 20, SocketFlags.None);
        string power_db = Encoding.UTF8.GetString(bytes);
        power_db = power_db.Substring(("SWADVR " + port).Length).Replace("\0", "");
        wifiPwrDB = float.Parse(power_db) / 100;
        if (wifiPwrDB > max)
            max = wifiPwrDB;
        Console.WriteLine("Test Port:" + port + " - Test value:" + wifiPwrDB);

        var keys = Console.ReadKey();
        if (keys.Key == ConsoleKey.Escape)
        {
            Test = false;
        }
        else if (keys.Key == ConsoleKey.P)
        {
            Console.Write("\n\rport:");
            port = int.Parse(Console.ReadLine());
        }


    }
    Console.WriteLine("Best value:" + max);
}
catch (Exception e)
{

    Console.WriteLine("Power metter isn't connected!"+ e.Message);
}