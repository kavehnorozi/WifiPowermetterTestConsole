using System.Net;
using System.Net.Sockets;
using System.Text;

string ipPowerMetter = "192.168.100.254";
int port = 11;
Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
s.ReceiveTimeout = 2000;
s.SendTimeout = 2000;

float wifiPwrDB = 0, max = 0;

IPAddress broadcast = IPAddress.Parse(ipPowerMetter);
byte[] sendbuf = Encoding.ASCII.GetBytes("SWADVR " + port);
IPEndPoint ep = new IPEndPoint(broadcast, 80);
for (int i = 0; i < 20; i++)
{
    s.SendTo(sendbuf, ep);
    byte[] bytes = new byte[20];

    s.Receive(bytes, 0, 20, SocketFlags.None);
    string power_db = Encoding.UTF8.GetString(bytes);
    power_db = power_db.Substring(("SWADVR " + port).Length).Replace("\0", "");
    wifiPwrDB = float.Parse(power_db) / 100;
    if (wifiPwrDB > max)
        max = wifiPwrDB;
}
Console.WriteLine("Best value:" + max);