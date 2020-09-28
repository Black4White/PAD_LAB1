using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client_2
{
    class client_2
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Console.Title = "Client_2";
            socket.Connect("192.168.1.2", 10002);
            string message = Console.ReadLine();         
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            socket.Send(buffer);
            Console.ReadLine();
        }
    }
}
