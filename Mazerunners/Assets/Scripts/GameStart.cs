using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public bool isHost;
    public static GameStart instance;
    public GameObject hostLabel;
    public GameObject playerNumber;
    public GameObject button;
    public GameObject GameManage;
    public int localPlayerId;
    int currentPlayers;

    public Text text;
    public string players;
    private void Awake()
    {
        instance = this;
        text = playerNumber.GetComponent<Text>();
        isHost = false;
    }
    private void Update()
    {
        text.text = players;
        if(isHost == true)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }
    public void First()
    {
        TCPClient.instance.onMessageRecieve += GetRole;
        TCPClient.instance.Send("Hello");
    }
    public void GetRole(string msg)
    {
        string[] instruction = msg.Split(':');
        if(instruction[0] == "Host")
        {
            isHost = true;
            players = "Current Players: 1";
            TCPClient.instance.onMessageRecieve -= GetRole;
            TCPClient.instance.onMessageRecieve += WaitForStart;

        }
        else if(instruction[0] == "Player")
        {
            isHost = false;
            players = "Current Players: " + Convert.ToInt32(instruction[1]);
            localPlayerId = Convert.ToInt32(instruction[1]) - 1;
            currentPlayers = Convert.ToInt32(instruction[1]);
            TCPClient.instance.onMessageRecieve -= GetRole;
            TCPClient.instance.onMessageRecieve += WaitForStart;
        }
        
    }
    public void WaitForStart(string msg)
    {
        string[] instruction = msg.Split(':');
        if (instruction[0] == "Update")
        {
            players = "Current Players: " + instruction[1];
            currentPlayers = Convert.ToInt32(instruction[1]);
        }
        else if (instruction[0] == "UpdateAll")
        {
            Debug.Log("Starting players");
            string[] playerDatas = instruction[1].Split(';');
            P_data[] p_Datas = new P_data[currentPlayers];
            for(int i = 0; i < currentPlayers; i++)
            {
                string[] data = playerDatas[i].Split('|');
                p_Datas[i] = new P_data(new Vector3((float)(Convert.ToDouble(data[1])), (float)(Convert.ToDouble(data[2])), (float)(Convert.ToDouble(data[3]))), 
                                        new Vector3((float)(Convert.ToDouble(data[4])), (float)(Convert.ToDouble(data[5])), (float)(Convert.ToDouble(data[6]))), 
                                        (float)Convert.ToDouble(data[7]), i);
            }
            Debug.Log("Starting players");
            GameManager.instance.StartGame(false, currentPlayers, localPlayerId, p_Datas);
            Debug.Log("Starting players");
            TCPClient.instance.onMessageRecieve -= WaitForStart;
            GameObject.Destroy(gameObject);
            
        }
              
    }
    public void StartButton()
    {
        
            TCPClient.instance.Send("Start");
        TCPClient.instance.onMessageRecieve -= WaitForStart;
        
        GameManager.instance.StartGame(isHost, currentPlayers, localPlayerId, null);
        
        
    }
}
