using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;

public class TCPClient : MonoBehaviour
{
    public static TCPClient instance;
    static Socket client ;
    public string ip = "192.168.0.14";
    public int port = 24000;
    private byte[] buffer = new byte[1024];
    private ASCIIEncoding asc = new ASCIIEncoding();
    public event Action<string> onMessageRecieve;
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("Starting connection");
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        instance = this;
        Connect(0);
        Debug.Log("Succesfully connected");
        //Send("LobbyInfo");
        client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieve), null);
        GameStart.instance.First();
        

    }
    
    private void MessageRecieve(IAsyncResult AR)
    {
        Debug.Log("Messege recieved");
        int recieved = client.EndReceive(AR);
        byte[] rawData = new byte[recieved];
        Buffer.BlockCopy(buffer, 0, rawData, 0, recieved);
        string msg = Encoding.ASCII.GetString(rawData);
        Debug.Log("Server command: " + msg + ", " + recieved);
        onMessageRecieve(msg);
        client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieve), null);
        
        
    }
    private void Connect(int attempt)
    {
       if(attempt < 10)
        {
            try
            {
                
                client.Connect(ip, port);

            }
            catch
            {
                Debug.Log("Failed to connect, attempt:" + attempt);
                Connect(attempt);
            }
            attempt++;
            
        }
    }
    public void Disconnect()
    {
        client.Disconnect(false);
    }
    public void Send(string s)
    {
        Debug.Log("Sending message: " + s);
        //client.BeginSend(asc.GetBytes(s), 0, s.Length, SocketFlags.None, new AsyncCallback(FinishSend), null);
        client.Send(Encoding.ASCII.GetBytes(s));
        
    }
   /* public void FinishSend(IAsyncResult AR)
    {
        client.EndSend(AR);
    }
   */
    
}
