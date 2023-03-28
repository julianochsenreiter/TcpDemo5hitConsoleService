using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TcpLibrary;

namespace WpfService
{
    public partial class MainWindow : Window
    {
        ObservableCollection<string> messages = new();
        public MainWindow()
        {
            InitializeComponent();
            lbMessages.ItemsSource = messages;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _ = Task.Run(async () =>
            {

                TcpListener listener = new TcpListener(
                    new IPEndPoint(IPAddress.Loopback, 3558));
                listener.Start();

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    _ = Task.Factory.StartNew(async (object? param) =>
                    {
                        using (TcpClient innerClient = (TcpClient)param!)
                        {
                            using (NetworkStream stream = innerClient.GetStream())
                            {
                                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                                {
                                    int count = reader.ReadInt32();
                                    for (int i = 0; i < count; i++)
                                    {
                                        string first = reader.ReadString();
                                        string last = reader.ReadString();
                                        Person p = new(first, last);
                                        string? message = p.ToString();
                                        Dispatcher.Invoke(() => messages.Add(message ?? "nothing"));

                                    }
                                }

                                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                                {
                                    await writer.WriteLineAsync("Message received.");
                                }
                            }

                        }


                    }, client);


                }
            });



            // TODO: start the TCP service here
            // TODO: process each request on a separate thread
            // TODO: display the received message in the list box
            // TODO: use different serialization techniques: StreamReader/StreamWriter, BinaryReader/BinaryWriter, BinaryFormatter, JsonSerializer
        }

        // TODO: make sure the service is stopped when the window is closed
    }
}
