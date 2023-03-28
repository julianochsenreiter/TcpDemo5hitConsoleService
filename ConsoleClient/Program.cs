using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpLibrary;

Console.WriteLine("Press a key to start ...");
Console.ReadKey();

Person p1 = new("Clemens", "Kerer");
Person p2 = new("Hans", "Müller");
List<Person> persons = new() { p1, p2 };

using TcpClient client = new TcpClient();
await client.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 3558));

using (NetworkStream stream = client.GetStream())
{
    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
    {
        writer.Write(persons.Count);
        foreach (var p in persons)
        {
            writer.Write(p.Firstname);
            writer.Write(p.Lastname);
        }
    }

    using (StreamReader reader = new StreamReader(stream))
    {
        Console.WriteLine(await reader.ReadLineAsync());
    }
}


// TODO: send messages using different serialization techniques: StreamReader/StreamWriter, BinaryReader/BinaryWriter, BinaryFormatter, JsonSerializer
// TODO: use async calls where possible
