using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System;

public class TCPClient : MonoBehaviour
{
    private static TcpClient client;
    private static Stream networkStream;
    private static string host;
    private static int port;
    private static ASCIIEncoding asen = new ASCIIEncoding();
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
        try
        {

            host = "192.168.0.14";
            port = 24000;
            client = new TcpClient();
            client.Connect(host, port);
            networkStream = client.GetStream();
            Debug.Log("Connecting");

        }
        catch (Exception e) { Debug.Log("Connection failed: " + e.StackTrace); }
    }

    public static string Get_Update()
    {
        string s = "";
        byte[] data_recieved = new byte[250];
        int k = networkStream.Read(data_recieved, 0, 250);
        for(int i = 0; i < k; i++)
        {
            s += Convert.ToChar(data_recieved[i]);
        }
        //Debug.Log("Messege recieved: " + s);
        return s;
    }
    
    public static  void Send_Update(string data)
    {
        byte[] encodedMsg = asen.GetBytes(data);
        networkStream.Write(encodedMsg, 0, encodedMsg.Length);
       // Debug.Log("Client transmitting!");
    }
}
