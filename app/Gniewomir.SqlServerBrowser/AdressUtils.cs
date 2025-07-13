using System.Net;

namespace Gniewomir.SqlServerBrowser;

public static class AdressUtils
{
    private static IPAddress 
        GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
    {
        var ipBytes = address.GetAddressBytes();
        var maskBytes = subnetMask.GetAddressBytes();

        if (ipBytes.Length != maskBytes.Length)
            throw new ArgumentException("IP i maska muszą mieć ten sam rozmiar");

        var broadcastBytes = new byte[ipBytes.Length];
        for (int i = 0; i < ipBytes.Length; i++)
        {
            broadcastBytes[i] = (byte)(ipBytes[i] | (maskBytes[i] ^ 255));
        }

        return new IPAddress(broadcastBytes);
    }
}