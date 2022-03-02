using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace TCPServer
{
    class Program
    {
        private static TcpListener myListener;
        private static IPAddress ipadr;
        private static ASCIIEncoding asen = new ASCIIEncoding();
        private static List<Socket> clients;
        static void Main(string[] args)
        {
            clients = new List<Socket>();
            ipadr = IPAddress.Parse("192.168.0.14");

            Socket client = Start_Server(ipadr, 24000);
            clients.Add(client);
            clients.Add(myListener.AcceptSocket());
           // clients.Add(myListener.AcceptSocket());
            for (int i = 0; i < clients.Count; i++)
                Send_data(clients[i], i.ToString()); 
            while (true)
            {
                Update_GameState();
            }
        }
        private static void Update_GameState()
        {
            string player_data = "";
            for (int i = 0; i < clients.Count; i++)
                player_data += Recieve_data(clients[i]) + ";";
           /* string attack_data = "";
            for (int i = 0; i < clients.Count; i++)
                attack_data += Recieve_data(clients[i]) + ";";
           */
           for(int i = 0; i < clients.Count; i++)
           {
                Send_data(clients[i], player_data);
           }
                
        }
        private static string Recieve_data(Socket s)
        {
            string msg = "";
            byte[] recieved_msg = new byte[250];
            int k = s.Receive(recieved_msg);
            for(int i = 0; i < k; i++)
            {
                msg += Convert.ToChar(recieved_msg[i]);
            }
            Console.WriteLine("Recived msg: " + msg);
            return msg;
        }
        private static void Send_data(Socket s, string data)
        {
            byte[] msg = asen.GetBytes(data);
            s.Send(msg);
            Console.WriteLine("Msg sent");
        }
        private static Socket Start_Server(IPAddress ip, int port)
        {
            myListener = new TcpListener(ip, 24000);
            myListener.Start();
            Console.WriteLine("Server is running!");
            Socket s = myListener.AcceptSocket();
            Console.WriteLine("Connected to: " + s.RemoteEndPoint);
            return s;
        }
    }
}
