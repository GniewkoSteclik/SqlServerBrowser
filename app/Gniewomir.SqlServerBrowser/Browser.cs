using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.NetworkInformation;

namespace Gniewomir.SqlServerBrowser;

public class Browser
{
    public static Task<string[]> QueryServerInstances(string host, int timeoutms)
    {
        return Task.Run(() =>
        {
            using var client = new UdpClient();
            client.EnableBroadcast = true;
            client.Client.ReceiveTimeout = timeoutms;

            byte[] request = { 0x02 };

            var hostAddress = Dns.GetHostAddresses(host)
                .Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .ToArray();

            var endpoint = new IPEndPoint(hostAddress[0], 1434);
            client.Send(request, request.Length, endpoint);

            var instances = new List<string>();
            var start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < timeoutms)
                try
                {
                    var remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    var response = client.Receive(ref remoteEP);
                    var result = Encoding.ASCII.GetString(response, 3, response.Length - 3);

                    var fields = result.Split(';', StringSplitOptions.RemoveEmptyEntries);


                    string? serverName = null;
                    string? instanceName = null;

                    for (var i = 0; i < fields.Length - 1; i++)
                    {
                        if (fields[i] == "ServerName")
                            serverName = fields[i + 1];
                        else if (fields[i] == "InstanceName")
                            instanceName = fields[i + 1];

                        if (serverName != null && instanceName != null)
                        {
                            instances.Add($"{serverName}\\{instanceName}");
                            serverName = null;
                            instanceName = null;
                        }
                    }
                }
                catch (SocketException)
                {
                    //TODO: handle this exception
                }

            return instances.Distinct().ToArray();
        });
    }
    
    
    
    public async Task<List<string>> GetServerList()
    {
        var tasks = GetAdressesToScan().Select(a =>
        {
            return Browser.QueryServerInstances(a, 2000);

        }).ToArray();
        await Task.WhenAll(tasks);
        var lista = new List<string>();
        foreach (var task in tasks)
        {
            lista.AddRange(task.Result);
        }
        return lista.Distinct().ToList();

    }
    
    public IEnumerable<string> GetAdressesToScan()
    {
        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;

            var ipProps = ni.GetIPProperties();
            foreach (var unicast in ipProps.UnicastAddresses)
            {
                if (unicast.Address.AddressFamily != AddressFamily.InterNetwork)
                    continue;

                var ip = unicast.Address;
                var mask = unicast.IPv4Mask;
                if (mask == null) continue;

                var broadcast = AdressUtils.GetBroadcastAddress(ip, mask);
                yield return broadcast.ToString();
            }
        }
    }
    
    
    
}