using Heartbeat_Service;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<Worker>();
        services.AddSingleton<HTTPService>();
    })
    .Build();

await host.RunAsync();


var client = new HttpClient();
string? privateIp;
using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
{
    socket.Connect("1.1.1.1", 53);
    var endPoint = socket.LocalEndPoint as IPEndPoint;
    privateIp = endPoint?.Address.ToString();
}



var conn = new ConnectionModel()
{
    IpAddress = privateIp ?? "",
    Port = "3389",
    ExtraParams = ""
};

Console.WriteLine("Exiting...");
await client.PostAsync($"http://10.0.1.5/Shutdown", JsonContent.Create(conn));
Thread.Sleep(400);


