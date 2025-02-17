using System.Net.Sockets;
using System.Text;

class TCPClient
{
    static void Main(string[] args)
    {
        string serverIp = "127.0.0.1";
        int port = 5000;

        try
        {
            using (TcpClient client = new TcpClient(serverIp, port))
            {
                Console.WriteLine("Connected to server!");
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    Console.Write("Enter command (GET_TEMP, GET_STATUS, EXIT): ");
                    string command = Console.ReadLine();

                    if (command.ToUpper() == "EXIT")
                        break;

                    byte[] data = Encoding.ASCII.GetBytes(command);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine(response);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
    }
}