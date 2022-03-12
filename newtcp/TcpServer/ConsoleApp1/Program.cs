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
        private static int numberOfPlayers;
        static void Main(string[] args)
        {
            numberOfPlayers = 1;
            clients = new List<Socket>();
            ipadr = IPAddress.Parse("146.169.172.5");
            Start_Server(ipadr, 24000);

            for (int i = 0; i < numberOfPlayers; i++)
            {
                clients.Add(myListener.AcceptSocket());
                Console.WriteLine("Succesfully connected to: " + clients[i].RemoteEndPoint);
            }
            Console.WriteLine("Connected to all players, initial values to players");
            for (int i = 0; i < clients.Count; i++)
            {
                Send_data(clients[i], (numberOfPlayers.ToString() + "|" + i.ToString()));

            }
            while (true)
            {
                Update_GameState();
            }
        }
        private static void Update_GameState()
        {
            string player_data = "";
            string attack_data = "";
            for (int i = 0; i < clients.Count; i++)
            {
                string[] data = Recieve_data(clients[i]).Split('$');
                player_data += data[0] + ";";
                if (data[1] != "nothing")
                    attack_data += data[1] + ";";
            }
            if (attack_data == "")
                attack_data = "nothing";
            string send_data = player_data + "$" + attack_data;
            for (int i = 0; i < clients.Count; i++)
            {
                Send_data(clients[i], send_data);
            }


        }
        private static string Recieve_data(Socket s)
        {
            string msg = "";
            byte[] recieved_msg = new byte[250];
            int k = s.Receive(recieved_msg);
            for (int i = 0; i < k; i++)
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
        private static void Start_Server(IPAddress ip, int port)
        {
            myListener = new TcpListener(ip, 24000);
            myListener.Start();
            Console.WriteLine("Server is running!");


        }
    }
}
