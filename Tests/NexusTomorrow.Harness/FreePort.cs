using System.Net;
using System.Net.Sockets;

namespace NexusTomorrow.Harness;

public static class FreePort
{
    private static readonly Lock s_gate = new();
    private static readonly HashSet<int> s_taken = [];

    // A port free a moment ago; the listener that found it is closed before the host binds. No port is handed out twice in a
    // process, so one test's three listeners (and parallel tests) never share one.
    public static int Take()
    {
        lock (s_gate)
        {
            while (true)
            {
                var listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                int port = ((IPEndPoint)listener.LocalEndpoint).Port;
                listener.Stop();
                if (s_taken.Add(port))
                    return port;
            }
        }
    }
}
