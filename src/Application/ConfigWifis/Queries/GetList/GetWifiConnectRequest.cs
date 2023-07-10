using System.Net.Sockets;
using System.Net;
using System.Text;
using MediatR;
using NativeWifi;

namespace mentor_v1.Application.ConfigWifis.Queries.GetList;
public class GetWifiConnectRequest : IRequest<WifiConnect>
{
}

public class GetWifiConnectRequestHandler : IRequestHandler<GetWifiConnectRequest, WifiConnect>
{
    public async Task<WifiConnect> Handle(GetWifiConnectRequest request, CancellationToken cancellationToken)
    {
        //lấy tên mạng đang kết nối
        WlanClient client = new WlanClient();
        string ipName = "";
        foreach (WlanClient.WlanInterface wlanInterface in client.Interfaces)
        {
            if (wlanInterface.InterfaceState == Wlan.WlanInterfaceState.Connected)
            {
                Wlan.Dot11Ssid ssid = wlanInterface.CurrentConnection.wlanAssociationAttributes.dot11Ssid;
                string ssidName = Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);

                Wlan.WlanBssEntry[] bssEntries = wlanInterface.GetNetworkBssList();
                foreach (Wlan.WlanBssEntry bssEntry in bssEntries)
                {
                    byte[] macBytes = bssEntry.dot11Bssid;
                    string macAddress = BitConverter.ToString(macBytes, 0, 6).Replace("-", ":");
                    ipName = ssidName;
                }
                break;
            }
        }

        //lấy IPv4 Address mạng đang kết nối
        string ipAddress = "";
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                break;
            }
        }

        WifiConnect wifiConnect = new WifiConnect() { 
            NameWifi = ipName,
            IPv6Adderss = ipAddress
        };

        return wifiConnect;
    }
}

public class WifiConnect
{ 
    public string NameWifi { get; set; }
    public string IPv6Adderss { get; set; }
}