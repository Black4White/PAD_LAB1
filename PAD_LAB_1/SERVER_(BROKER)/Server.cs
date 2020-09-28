using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiServer
{
    class Program
    {
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 1024;
        private const int PORT = 10002;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        //private static List<int> clientlists = new List<int>();
        private static List<List<object>> clientlists = new List<List<object>>(); //инициализация
        

        static void Main()
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine();
            //CloseAllSockets();


            foreach (Socket socket in clientSockets)
            {
                Console.WriteLine(socket);
            }



        }

        private static void SetupServer()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            //Console.WriteLine("Server setup complete");
        }

        private static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            clientSockets.Add(socket);

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Console.WriteLine("Клиент подключен!");
            string login = Encoding.ASCII.GetString(buffer);
            char[] charsToTrim = { (char)0 };
            login = login.Trim(charsToTrim);
            Console.WriteLine("Login: " + login);
            //Console.WriteLine("-------------");

            serverSocket.BeginAccept(AcceptCallback, null);


            clientlists.Add(new List<object>());//добавление новой строки

            // заполнить свою таблицу клиентов index/handle/login
            // buffer -> login


            IntPtr currenthandle = socket.Handle;
            Console.WriteLine(currenthandle);
            int counter = 0;
            foreach (Socket client in clientSockets)
            {
                
                if (currenthandle == socket.Handle)
                {
                    clientlists[counter].Add(currenthandle);//добавление столбца в новую строку
                    clientlists[counter].Add(login);//добавление столбца в новую строку
                    //clientlists[0][0];//обращение к первому столбцу первой строки
                    //Console.WriteLine(socket.Handle);
                    //Console.WriteLine(buffer);
                }
                counter++;

            }

            Console.WriteLine("LIST:");
            counter = 0;
            foreach (Socket client in clientSockets)
            {
                
                //clientlists[counter][0];//обращение к первому столбцу первой строки

                Console.WriteLine(" Index: " +  counter + " Handle: "+ clientlists[counter][0] + " Login: "+ clientlists[counter][1]);

            counter++;


            }






        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                IntPtr currenthandle = current.Handle;
                Console.WriteLine("Клиент отключен!");
                current.Close();
                clientSockets.Remove(current);

                // edit таблицу клиентов index/handle/login
                // поиск по handle
               /* int counter = 0;
                foreach (Socket client in clientSockets)
                {

                    if (currenthandle == socket.Handle)
                    {
                        clientlists[counter].Add(currenthandle);//добавление столбца в новую строку
                        clientlists[counter].Add(login);//добавление столбца в новую строку
                    }
                    counter++;

                }*/

                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string mess = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Message: " + mess);
            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
            
            // поиск по handle в нашей таблице
            //прочесть сообщение, найти получателя и оправить ему


        }
    }
}