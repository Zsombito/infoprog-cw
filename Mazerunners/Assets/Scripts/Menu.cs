using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private List<Lobby_data> lobbies;
    public int currentPage;
    public GameObject lobby1;
    public GameObject lobby2;
    public GameObject lobby3;
    private Button button1;
    private Button button2;
    private Button button3;
    private Text text1;
    private Text text2;
    private Text text3;

    public int addIndex;
    struct Lobby_data
    {
        public string name;
        public int maxPlayer, currentPlayer;
        public bool isJoinable;


        public Lobby_data(string[] data)
        {

            name = data[0];
            
            maxPlayer = System.Convert.ToInt32(data[2]);
            currentPlayer = System.Convert.ToInt32(data[1]);
            isJoinable = System.Convert.ToBoolean(data[3]);
        }
    }

     void Start()
    {
        addIndex = 0;
        currentPage = 0;
        button1 = lobby1.GetComponent<Button>();
        button2 = lobby2.GetComponent<Button>();
        button3 = lobby3.GetComponent<Button>();
        text1 = lobby1.GetComponentInChildren<Text>();
        text2 = lobby1.GetComponentInChildren<Text>();
        text3 = lobby1.GetComponentInChildren<Text>();
        TCPClient.instance.onMessageRecieve += Get_LobbyData;
        TCPClient.instance.Send("LobbyInfo");
        
    }
    private void Update()
    {
        
    }
    public void RequestLobbyData()
    {
        TCPClient.instance.Send("LobbyInfo"); 
    }
    public void Get_LobbyData(string data)
    {
        
        string[] instruction = data.Split(':');
        Debug.Log("LobbyInfo: " + instruction[1]);
        if (instruction[0] == "LobbyInfo")
        {
            string[] lobbiesData = instruction[1].Split(';');
            if (lobbiesData[0] == "0")
                lobbies = null;
            else
            {
                lobbies = null;
                lobbies = new List<Lobby_data>();
                for (int i = 1; i <= System.Convert.ToInt32(lobbiesData[0]); i++)
                {
                    Debug.Log("Adding lobby:" + lobbiesData[i]);
                    lobbies.Add(new Lobby_data(lobbiesData[i].Split('|')));
                }
            }
            Set_LobbyButtonText();
        }
        

    }
    public void Set_LobbyButtonText()
    {
       
        if (lobbies != null)
        {
            Debug.Log("Lobbies: " + lobbies.Count);

            if ((lobbies.Count) == 3)
            {
                addIndex = 0;
                
                text1.text = lobbies[currentPage * 3].name + "             Players: " + lobbies[currentPage * 3].currentPlayer + "/" + lobbies[currentPage * 3].maxPlayer;
                text2.text = lobbies[currentPage * 3 + 1].name + "             Players: " + lobbies[currentPage * 3 + 1].currentPlayer + "/" + lobbies[currentPage * 3 + 1].maxPlayer;
                text3.text = lobbies[currentPage * 3 + 2].name + "             Players: " + lobbies[currentPage * 3 + 2].currentPlayer + "/" + lobbies[currentPage * 3 + 2].maxPlayer;
                button1.interactable = lobbies[currentPage * 3].isJoinable;
                button2.interactable = lobbies[currentPage * 3 + 1].isJoinable;
                button3.interactable = lobbies[currentPage * 3 + 2].isJoinable;
                
            }
            else if ((lobbies.Count) == 2)
            {
                addIndex = 3;
                
                text1.text = lobbies[currentPage * 3].name + "             Players: " + lobbies[currentPage * 3].currentPlayer + "/" + lobbies[currentPage * 3].maxPlayer;
                text2.text = lobbies[currentPage * 3 + 1].name + "             Players: " + lobbies[currentPage * 3 + 1].currentPlayer + "/" + lobbies[currentPage * 3 + 1].maxPlayer;
                text3.text = "+";
                button1.interactable = lobbies[currentPage * 3].isJoinable;
                button2.interactable = lobbies[currentPage * 3 + 1].isJoinable;
                button3.interactable = true;

            }
            else if ((lobbies.Count) == 1)    
            {
                
                addIndex = 2;
                Debug.Log("Set lobbies1");
                
                Debug.Log("Set lobbies2");
                text1.text = lobbies[currentPage * 3].name + "             Players: " + lobbies[currentPage * 3].currentPlayer + "/" + lobbies[currentPage * 3].maxPlayer;
                text2.text = "+";
                text3.text = "EMPTY";
                Debug.Log("Set lobbies3");
                button1.interactable = lobbies[currentPage * 3].isJoinable;
                button2.interactable = true;
                button3.interactable = false;
                Debug.Log("Set lobbies4");




            }
            else
            {

                addIndex = 1;
                
                text1.text = "+";
                text2.text = "EMPTY";
                text3.text = "EMPTY";
                button1.interactable = true;
                button2.interactable = false;
                button3.interactable = false;


            }
        }
        else
        {
            
            addIndex = 1;
            
            text1.text = "+";
            text2.text = "EMPTY";
            text3.text = "EMPTY";
            button1.interactable = true;
            button2.interactable = false;
            button3.interactable = false;



        }

    }
    public void Lobby_pressed(int lobbyNumber)
    {
            
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        if (lobbyNumber == addIndex)
        {
            
            string msg = "Create:Lobby" + (currentPage * 3 + lobbyNumber).ToString() + "|4";
            TCPClient.instance.Send(msg);
            TCPClient.instance.onMessageRecieve += WaitForCreate;
        }
        else
        {
            string msg = "Join:" + (currentPage * 3 + lobbyNumber - 1).ToString();
            TCPClient.instance.Send(msg);
            TCPClient.instance.onMessageRecieve += WaitForJoin;
        }
    }
    public void Exit()
    {
        TCPClient.instance.Disconnect();
        Application.Quit();
    }
    public void WaitForCreate(string msg)
    {

    }
    public void WaitForJoin(string msg)
    {
        string[] instruction = msg.Split(':');
        Debug.Log(instruction[0] + " " + instruction[1]);
        if(instruction[0] == "Join")
        {

            if(instruction[1] == "1")
            {
                
                button1.interactable = false;
                button2.interactable = false;
                button3.interactable = false;
                switch(Convert.ToInt32(instruction[2]))
                {
                    case 0:
                        lobby1.GetComponent<Image>().color = Color.green;
                        break;
                    case 1:
                        lobby2.GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        lobby3.GetComponent<Image>().color = Color.green;
                        break;   
                }
                TCPClient.instance.onMessageRecieve += WaitForStart;
            }
            else
            {
                GameObject.Find("Lobby" + (Convert.ToInt32(instruction[2]) + 1).ToString()).GetComponent<Image>().color = Color.red;
            }



            TCPClient.instance.onMessageRecieve -= WaitForJoin;
        }
    }
    public void WaitForStart(string msg)
    {

    }

}
