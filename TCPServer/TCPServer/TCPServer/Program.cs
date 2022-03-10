using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace TCPServer
{
    class TCP_Server
    {

        static Socket serverSocket;
        static List<Socket> clients;
        static List<GameInstance> gameInstances;
        static byte[] buffer = new byte[1024];
        public static bool lobby;
        static int currentGameInstance;
       
        static void Main(string[] args)
        {
            lobby = false;
            clients = new List<Socket>();
            gameInstances = new List<GameInstance>();
            currentGameInstance = -1;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 24000));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(new AsyncCallback(AcceptPotentialClient), null);
            Console.ReadLine();
        }
        public static void Send(string msg, Socket client)
        {
            Console.WriteLine("Sending messege to: " + client.RemoteEndPoint);
            Console.WriteLine("                 ->" + msg);
            client.Send(Encoding.ASCII.GetBytes(msg));
        }
        static void AcceptPotentialClient(IAsyncResult AR)
        {
            Socket c = serverSocket.EndAccept(AR);
            clients.Add(c);
            Console.WriteLine("Connected to " + c.RemoteEndPoint);
            c.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(MenuMessages), c);
            serverSocket.BeginAccept(new AsyncCallback(AcceptPotentialClient), null);
        }
        public static void ConsoleWrite(string msg)
        {
            Console.WriteLine(msg);
        }
        static void MenuMessages(IAsyncResult AR)
        {
            Socket client = (Socket)AR.AsyncState;
            int recieved = client.EndReceive(AR);
            byte[] data_recieved = new byte[recieved];
            Buffer.BlockCopy(buffer, 0, data_recieved, 0, recieved);
            string[] instruction = Encoding.ASCII.GetString(data_recieved).Split(':');
            Console.WriteLine("Instruction recieved from " + client.RemoteEndPoint + "->" + instruction[0]);
            if(instruction[0] == "Hello")
            {
                if(lobby == false)
                {
                    Send("Host:", client);
                    currentGameInstance++;
                    gameInstances.Add(new GameInstance(client, currentGameInstance));
                    lobby = true;
                    client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(MenuMessages), client);
                }
                else
                {
                    string msg = "Player:" + gameInstances[currentGameInstance].Join(client);
                    Send(msg, client);
                    
                    // Only need if there are Player commands : client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(MenuMessages), client);
                }
            }
            if(instruction[0] == "Start")
            {
                lobby = false;
                gameInstances[currentGameInstance].Start();

            }


        }
    }
    class GameInstance
    {
        public Socket host;
        public List<Socket> players;
        public int currentPlayers;
        public int id;
        byte[] buffer = new byte[1024];
        public GameInstance(Socket host, int id)
        {

            this.host = host;
            players = new List<Socket>();
            players.Add(host);
            currentPlayers = 1;
            this.id = id;
            
        }
        public string Join(Socket player)
        {
            currentPlayers++;
            for (int i = 0; i < players.Count; i++)
            {
                TCP_Server.Send("Update:" + currentPlayers.ToString(), players[i]);
            }
                
            players.Add(player);
            
            return MenuInfo();
        }
        public string MenuInfo()
        {
            return currentPlayers.ToString();
        }
        public void Start()
        {
            
            int recieved =  host.Receive(buffer);
            byte[] data_recieved = new byte[recieved];
            Buffer.BlockCopy(buffer, 0, data_recieved, 0, recieved);
            string[] instruction = Encoding.ASCII.GetString(data_recieved).Split(':');
            host.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InGameCommands),host);
            if (instruction[0] == "FullPlayerData")
                for(int i = 1; i < currentPlayers; i++)
                {
                    players[i].BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InGameCommands), players[i]);
                    TCP_Server.Send("UpdateAll:" + instruction[1], players[i]);
                    
                }
            
        }
        public void InGameCommands(IAsyncResult AR)
        {
            Socket client = (Socket)AR.AsyncState;
            int recieved = client.EndReceive(AR);
            byte[] data_recieved = new byte[recieved];
            Buffer.BlockCopy(buffer, 0, data_recieved, 0, recieved);
            string[] instruction = Encoding.ASCII.GetString(data_recieved).Split(':');
            TCP_Server.ConsoleWrite("Instruction recieved from " + client.RemoteEndPoint + "->" + instruction[0]);
            if(instruction[0] == "UpdatePlayerInfo")
            {
                for(int i = 0; i < currentPlayers; i++)
                {
                    if (i != Convert.ToInt32(instruction[1]))
                        TCP_Server.Send(instruction[0]+":" + instruction[1] +":"+ instruction[2], players[i]);
                }
            }
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InGameCommands), client);

        }
        
    }
}
/* class TCP_Server
 {
     static Socket serverSocket;
     static List<Socket> clients;
     static List<Lobby> lobbies = new List<Lobby>();
     static byte[] buffer = new byte[1024];
     private static ASCIIEncoding encoder = new ASCIIEncoding();

     static void Main(string[] args)
     {
         clients = new List<Socket>();
         serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
         serverSocket.Bind(new IPEndPoint(IPAddress.Any, 24000));
         serverSocket.Listen(1);
         serverSocket.BeginAccept(new AsyncCallback(AcceptPotentialClient), null);
         Console.WriteLine("Server is running");
         //Debug
         while(true)
         {
             string debug = Console.ReadLine();
             if (debug == "Lobbies")
                 for (int i = 0; i < lobbies.Count; i++)
                     Console.WriteLine(lobbies[i].GetMenuInfo());

         }
     }
     static void AcceptPotentialClient(IAsyncResult AR)
     {
         Socket c = serverSocket.EndAccept(AR);
         clients.Add(c);

         Console.WriteLine("Connected to " + c.RemoteEndPoint);
         c.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InitialMessegeRecieved), c);
         serverSocket.BeginAccept(new AsyncCallback(AcceptPotentialClient), null);
     }
     static void InitialMessegeRecieved(IAsyncResult AR)
     {
         Socket client = (Socket)AR.AsyncState;
         int recieved = client.EndReceive(AR);

         byte[] data_recieved = new byte[recieved];
         Buffer.BlockCopy(buffer, 0, data_recieved, 0, recieved);
         string[] message = Encoding.ASCII.GetString(data_recieved).Split(':');
         Console.WriteLine("Recieved msg: " + message[0]);
         if (message[0] == "LobbyInfo" )
         {
             Console.WriteLine("Sending Lobby info");
             if (lobbies.Count != 0)
             {
                 string s = "LobbyInfo:" + lobbies.Count + ";";
                 for (int i = 0; i < lobbies.Count; i++)
                     s += lobbies[i].GetMenuInfo() + "|" + i + ";";
                 client.Send(Encoding.ASCII.GetBytes(s));
                 client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InitialMessegeRecieved), client);
             }
             else
             {
                 client.Send(Encoding.ASCII.GetBytes("LobbyInfo:0"));
                 client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InitialMessegeRecieved), client);
             }
         }
         else if(message[0] == "Join")
         {
             Console.WriteLine("Sending Join Response: ");
             client.Send(encoder.GetBytes("Join:" + (lobbies[Convert.ToInt32(message[1])].Join(client)).ToString() + ":" + message[1]));
             client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InitialMessegeRecieved), client);
         }
         else if(message[0] == "Create")
         {
             string[] param = message[1].Split('|');
             lobbies.Add(new Lobby(client, param[0], Convert.ToInt32(param[1])));
             client.Send(encoder.GetBytes((lobbies.Count - 1).ToString()));
             client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(InitialMessegeRecieved), client);
         }
         else if(message[0] == "Start")
         {
             lobbies[Convert.ToInt32(message[1])].Start();
         }

     }
     static void GamepUdate(IAsyncResult AR)
     {

     }
 }

 class Lobby
 {
     public string name;
     public Socket hostSocket;
     public List<Socket> mates;
     public int maxPlayers;
     public int currentPlayers;
     public bool isJoinable;
     public string GetMenuInfo(){return name + "|" + currentPlayers + "|" + maxPlayers + "|" + isJoinable.ToString();}
     public Lobby(Socket host, string name, int maxPlayers)
     {
         hostSocket = host;
         this.maxPlayers = maxPlayers;
         currentPlayers = 1;
         isJoinable = true;
         mates = new List<Socket>();
         mates.Add(hostSocket);
         this.name = name;
     }
     public int Join(Socket s) 
     {
         if (isJoinable == true)
         {
             mates.Add(s);
             currentPlayers++;
             if (currentPlayers == maxPlayers)
                 isJoinable = false;
             return 1;
         }
         else
             return 0;
     }
     public void Start()
     {
         for(int i = 0; i < currentPlayers; i++)
         {

         }
     }

 }
}
 */
/*class Program
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
        ipadr = IPAddress.Parse("192.168.0.162");
        Start_Server(ipadr, 24000) ;

        for(int i = 0; i < numberOfPlayers; i++)
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
       for(int i = 0; i < clients.Count; i++)
       {
            Send_data(clients[i], send_data);
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
    private static void Start_Server(IPAddress ip, int port)
    {
        myListener = new TcpListener(ip, 24000);
        myListener.Start();
        Console.WriteLine("Server is running!");


    }
} */
