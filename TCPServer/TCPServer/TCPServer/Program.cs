using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace TCPServer
{
    class Program
    {
        private static TcpListener myListener;
        private static IPAddress ipadr;
        private static ASCIIEncoding asen = new ASCIIEncoding();
        static void Main(string[] args)
        {
            ipadr = IPAddress.Parse("146.169.160.78");

            
            Socket client1 = Start_Server(ipadr, 24000);
            int x = 0;
            while (true)
            {
                Console.WriteLine("Waiting on data");
                Recieve_data(client1);
                Send_data(client1, x.ToString());
                x++;
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
