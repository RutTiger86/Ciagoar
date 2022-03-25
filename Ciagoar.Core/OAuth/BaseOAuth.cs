using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Ciagoar.Core.OAuth
{
    public class BaseOAuth
    {
        // ref http://stackoverflow.com/a/3978040
        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
