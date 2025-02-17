using System.Net.Sockets;
using System.Net;
using System.Text;

class TCPServer {
    private static TcpListener listener;
    private static bool isRunning = true;

    static void Main()
    {
        int port = 5000;
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Server started on port {port}");

        while (isRunning)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        listener.Stop();
    }

    static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // Client disconnected

                string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received: {request}");

                string response = ProcessCommand(request.Trim());
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }

    static string ProcessCommand(string command)
    {
        switch (command.ToUpper())
        {
            case "GET_TEMP":
                return "TEMP: 25.3C";
            case "GET_STATUS":
                return "STATUS: OK";
            default:
                return "ERROR: Unknown command";
        }
    }
}