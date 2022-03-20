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
    public GameObject[] buttonmenuthing;
    private float waitTime;
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
                
            }
            else
            {
                /*
                string[] args = msg.Split('|');
                isSet = true;
                Destroy(Label1);
                Destroy(input);
                Destroy(StartButton);
                Label2.GetComponent<Text>().text = "Current Players: " + args[1] + "/" + args[2];
                currentPlayers = Convert.ToInt32(args[1]);
                numberOfPlayers = Convert.ToInt32(args[2]);
                waitTime = Time.time;*/
                GameManager.instance.isStart = true;
                for (int i = 0; i < buttonmenuthing.Length; i++)
                    buttonmenuthing[i].SetActive(false);
            }

            isStart = false;
        }
        /*
        else if (isHost == true) { }
        else if (isSet == true && Time.time - waitTime >= 0.5F)
        {
            Debug.Log("Entering is set!");
            for(int i = currentPlayers; i < numberOfPlayers+1; i++)
            {
                currentPlayers = Convert.ToInt32(TCPClient.instance.Get_Update());
                Label2.GetComponent<Text>().text = "Current Players: " + currentPlayers + "/" + numberOfPlayers;
            }
            Debug.Log("Exiting is set");
            isSet = false;
            GameManager.instance.isStart = true;
            Destroy(gameObject);
        }
        */

    }
    public void StartPressed()
    {
        
        
        TCPClient.instance.Send_Update(input.text);
        GameManager.instance.isStart = true;
        for (int i = 0; i < buttonmenuthing.Length; i++)
            buttonmenuthing[i].SetActive(false);

        /*
        waitTime = Time.time;
        isHost = false;
        Debug.Log("Button pressed and executed!");
        */


    }
    
}
