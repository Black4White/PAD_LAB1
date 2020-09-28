using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace client_1
{
    class client_1
    {

        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int number = rnd.Next(1, 20);

            string numberRand = number.ToString();

            Console.WriteLine(number);
            string myip = GetLocalIPAddress();
            Console.WriteLine(myip);
            Console.Title = "Client_1";
            socket.Connect("192.168.1.2", 10002);
            string ID = "1001";
            //string ModuleDescriprion = "Sending random number";
            //string NameOfModule = "Genarator";
            byte[] buffermessage1 = Encoding.ASCII.GetBytes(ID);

            byte[] buffermessage2 = Encoding.ASCII.GetBytes(myip);
            socket.Send(buffermessage1);
            //Console.ReadLine();
            socket.Send(buffermessage2);
            Console.ReadLine();

        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }




    }
}
