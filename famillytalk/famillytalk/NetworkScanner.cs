using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace famillytalk
{
    public class NetworkScanner
    {
        public string GetLocalSubnet()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    var parts = ip.ToString().Split('.');
                    return $"{parts[0]}.{parts[1]}.{parts[2]}";
                }
            }
            return "192.168.1"; //Default here
        }

        public async Task<bool> PingIpAsync(string ip)
        {
            try
            {
                Ping ping = new Ping();
                var reply = await ping.SendPingAsync(ip, 500);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
