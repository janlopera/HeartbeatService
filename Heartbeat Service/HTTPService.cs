using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;

namespace Heartbeat_Service;

public class HTTPService
{
    private HttpClient _client;

    private string baseUrl = "http://10.0.1.5";

    private ConnectionModel conn;

    public HTTPService(ILogger<HTTPService> log)
    {
        _client = new HttpClient();
        string? privateIp;
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("1.1.1.1", 53);
            var endPoint = socket.LocalEndPoint as IPEndPoint;
            privateIp = endPoint?.Address.ToString();
        }

        log.Log(LogLevel.Information, $"IP Address: {privateIp}");

        conn = new ConnectionModel()
        {
            IpAddress = privateIp ?? "",
            Port = "3389",
            ExtraParams = ""
        };
    }

    public async void HeartBeat()
    {
        await _client.PostAsync($"{baseUrl}/Heartbeat", JsonContent.Create<ConnectionModel>(conn));
    }
    public async void Release()
    {
        await _client.PostAsync($"{baseUrl}/Release", JsonContent.Create<ConnectionModel>(conn));
    }
}