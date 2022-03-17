using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace TCPServer
{
    class Program
    {
        private static TcpListener myListener;  
        private static IPAddress ipadr;
        private static ASCIIEncoding asen = new ASCIIEncoding();
        private static List<Socket> clients;
        private static int numberOfPlayers;
        private static bool isProcess, isGameActive;
        static void Main(string[] args)
        {
            Server_Start();
            isProcess = false;
            isGameActive = false;
            while (true)
            {
                isGameActive = GameStart();

                while (isGameActive)
                {
                    isGameActive = Update_GameState();
                }
            }
        }
        private static void Server_Start()
        {
            clients = new List<Socket>();
            ipadr = IPAddress.Any;
            Start_Server(ipadr, 24000);
        }
        private static bool GameStart()
        {
            try
            {
                Console.WriteLine("Game starting awaiting connection of the host");
                clients.Add(myListener.AcceptSocket());
                Console.WriteLine("Succesfully connected to host: " + clients[0].RemoteEndPoint);
                Send_data(clients[0], "Host");
                numberOfPlayers = Convert.ToInt32(Recieve_data(clients[0]));
                Console.WriteLine("Number of Players: " + numberOfPlayers);
                for (int i = 1; i < numberOfPlayers; i++)
                {

                    clients.Add(myListener.AcceptSocket());
                    Console.WriteLine("Succesfully connected to: " + clients[i].RemoteEndPoint);
                    Send_data(clients[i], "SOmetghinASGasg");


                }
                Console.WriteLine("Connected to all players, sending initial values to players");
                Thread.Sleep(5000);
                for (int i = 0; i < clients.Count; i++)
                {

                    Send_data(clients[i], (numberOfPlayers.ToString() + "|" + i.ToString()));
                    clients[i].ReceiveTimeout = 100;
                }
            }
            catch
            {
                Console.WriteLine("Game launch failed!, retrying!");
                TerminateConnections();
                return false;
            }
            Console.WriteLine("Game launch successful starting game now");
            return true;
        }
        private static bool Update_GameState()  
        {
            try
            {
                string player_data = "";
                string attack_data = "";
                for (int i = 0; i < clients.Count; i++)
                {
                    string[] data = Recieve_data(clients[i]).Split('$');
                    if(data[0] == "Exit")
                    {
                        for(int j = 0; j < clients.Count; j++)
                        {
                            Send_data(clients[j], "$Exit");
                        }
                        Thread.Sleep(1000);
                        TerminateConnections();
                        return false;
                    }
                    if(data[0] == "Win")
                    {
                        for (int j = 0; j < clients.Count; j++)
                        {
                            Send_data(clients[j],data[0] + "$"+ data[1]);
                        }
                        Thread.Sleep(2000);
                        TerminateConnections();
                        return false;
                    }
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
            catch 
            { 
                Console.WriteLine("Failed to sync, retry");
                for (int i = 0; i < clients.Count; i++)
                {
                    Send_data(clients[i], "Retry");
                }
            }
            return true;


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
            Console.WriteLine("Msg sent to " + s.RemoteEndPoint + ": " + data);
        }
        private static void Start_Server(IPAddress ip, int port)
        {
            myListener = new TcpListener(ip, 24000);
            myListener.Start();
            Console.WriteLine("Server is running!");


        }
        private static void TerminateConnections()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                try
                {
                    clients[i].Disconnect(false);
                }
                catch
                {
                }
            }
            clients = new List<Socket>();
        }
    }
}
