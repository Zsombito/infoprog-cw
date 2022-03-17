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
    private  TcpClient client;
    private  Stream networkStream;
    
    public string host;
    private  int port;
    private  ASCIIEncoding asen = new ASCIIEncoding();
    // Start is called before the first frame update
    private void Awake() //Connects the TCP Client to the server
    {
        DontDestroyOnLoad(this);
        instance = this;
        try
        {

            
            port = 24000;
            client = new TcpClient();
            client.Connect(host, port);
            client.ReceiveTimeout = 1000;
            networkStream = client.GetStream();
            Debug.Log("Connecting");

        }
        catch (Exception e) { Debug.Log("Connection failed: " + e.StackTrace); }

        
    }
    void Start()
    {
        GameStart.instace.isStart = true;   
    }
    public string Get_Update() //Function used to recieve server message
    {
        string s = "";
        byte[] data_recieved = new byte[250];
        int k = networkStream.Read(data_recieved, 0, 250);
        for(int i = 0; i < k; i++)
        {
            s += Convert.ToChar(data_recieved[i]);
        }
        Debug.Log("Msg recieved:" + s);
        return s;
    }
    
    public void Send_Update(string data) //Function used to send string to the server
    {
        byte[] encodedMsg = asen.GetBytes(data);
        networkStream.Write(encodedMsg, 0, encodedMsg.Length);
       
    }
}
