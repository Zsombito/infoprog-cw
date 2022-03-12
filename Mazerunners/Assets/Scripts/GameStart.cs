using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameStart : MonoBehaviour
{
    public static GameStart instace;
    public InputField input;
    public bool isStart,isSet,isHost;
    public GameObject StartButton;
    public GameObject Label1;
    public GameObject Label2;
    private int numberOfPlayers, currentPlayers;
    private void Awake()
    {
        instace = this;
        isStart = false;
        isSet = false;
        isHost = false;
    }
    private void Start()
    {
        
        
    }
    private void Update()
    {
        if (isStart == true)
        {
            string msg = TCPClient.instance.Get_Update();
            if (msg == "Host")
            {
                Debug.Log("I am host");
                isHost = true;
                isSet = true;
            }
            else
            {
                string[] args = msg.Split('|');
                isSet = true;
                Destroy(Label1);
                Destroy(input);
                Destroy(StartButton);
                Label2.GetComponent<Text>().text = "Current Players: " + args[1] + "/" + args[2];
                currentPlayers = Convert.ToInt32(args[1]);
                numberOfPlayers = Convert.ToInt32(args[2]);
            }
            isStart = false;
        }
        else if (isHost == true) { }
        else if (isSet == true)
        {
            Debug.Log("Entering is set!");
            for(int i = currentPlayers; i < numberOfPlayers; i++)
            {
                currentPlayers = Convert.ToInt32(TCPClient.instance.Get_Update());
                Label2.GetComponent<Text>().text = "Current Players: " + currentPlayers + "/" + numberOfPlayers;
            }
            Debug.Log("Exiting is set");
            isSet = false;
            GameManager.instance.isStart = true;
            Destroy(gameObject);
        }
    }
    public void StartPressed()
    {
        TCPClient.instance.Send_Update(input.text);
        numberOfPlayers = Convert.ToInt32(input.text);
        currentPlayers = 1;
        Label2.GetComponent<Text>().text = "Current Players: " + currentPlayers + "/" + numberOfPlayers;
        Destroy(Label1);
        Destroy(input);
        Destroy(StartButton);
        isHost = false;
    }
    
}
