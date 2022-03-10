using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //needed:
    public static GameManager instance;
    public event Action<P_data, bool> onPlayerDataUpdate;
    private  P_data[] p_Datas;
    private int localPlayerIndex;
    public bool isHost;
    public GameObject localPlayer;
    public GameObject remotePlayer;
    public GameObject Gamestart;
    private int numberOfPlayers;

    private  List<Damage> attacks;
    private  int p;
    
    
    
    public  int NumberOfPlayers { get { return numberOfPlayers; } }
    public  int LocalPlayerIndex { get { return localPlayerIndex; } }
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
       // DontDestroyOnLoad(this);
        
    }
    public void StartGame(bool isHost, int numberOfPlayers, int localPlayerId, P_data[] playerDatas)
    {
        this.isHost = isHost;
        this.numberOfPlayers = numberOfPlayers;
        this.localPlayerIndex = localPlayerId;
        Debug.Log("Starting players: " + isHost + " " + numberOfPlayers + " " + localPlayerId );
        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (i == localPlayerIndex)
            {
                Debug.Log("Trying to generate player");
                var p = Instantiate(localPlayer, new Vector3(i * 5, 0, 0), Quaternion.identity);
                Player geci = p.GetComponent<Player>();
                geci.playerId = i;
                geci.name = "LocalPlayer";
                Debug.Log("Local player created!");
            }
            else
            {
                Debug.Log("Trying to generate remote player");
                var p = Instantiate(remotePlayer, new Vector3(i * 5, 0, 0), Quaternion.identity);
                Mob geci = p.GetComponent<Mob>();
                geci.playerId = i;
                geci.name = "Player" + i.ToString();
            }
        }
        Debug.Log("Players set");
        if (isHost == true)
        {

            p_Datas = new P_data[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                //P_datas initialise
                p_Datas[i] = new P_data(new Vector3(i * 5, 0, 0), new Vector3(0, 0, 0), 50F, i);
            }

            //Transmit full data
            string msg = "FullPlayerData:";
            for (int i = 0; i < numberOfPlayers; i++)
                msg += p_Datas[i].Generate_SaveString() + ";";
            TCPClient.instance.Send(msg);
        }
        else
        {
            p_Datas = playerDatas;
        }
        for(int i = 0; i < numberOfPlayers; i++)
            onPlayerDataUpdate(p_Datas[i], true);
        TCPClient.instance.onMessageRecieve += RecieveServerCommand;
        Destroy(Gamestart);
        
    
    }
    public void Set_LocalPlayerInfo(P_data data) 
    {
        p_Datas[localPlayerIndex] = data;
        TCPClient.instance.Send("UpdatePlayerInfo:" + localPlayerIndex.ToString() + ":" + p_Datas[localPlayerIndex].Generate_SaveString());
    }
    public void RecieveServerCommand(string msg)
    {
        string[] instructions = msg.Split(':');
        if(instructions[0] == "UpdatePlayerInfo")
        {
            p_Datas[Convert.ToInt32(instructions[1])].Set_Values(instructions[2]);
            onPlayerDataUpdate(p_Datas[Convert.ToInt32(instructions[1])], false);
        }
    }
    void Start()
    {
        /*
        string[] initalData = TCPClient.Get_Update().Split('|');
        numberOfPlayers = Convert.ToInt32(initalData[0]);
        Debug.Log("The amount of players: " + numberOfPlayers);
        p_Datas = new P_data[numberOfPlayers];
        attacks = new List<Damage>();
        players = new List<GameObject>();
        localPlayerIndex = Convert.ToInt32(initalData[1]);
        Debug.Log("Got local player id: " + localPlayerIndex);
        
        for (int i = 0; i < p_Datas.Length; i++)
        {
            p_Datas[i] = new P_data();
            Debug.Log("Generating players");
            Debug.Log("Local player id: " + localPlayerIndex);
            
            if(i == localPlayerIndex)
            {
                var p = Instantiate(localPlayer, new Vector3(i*5, 0, 0), Quaternion.identity);
                Player geci = p.GetComponent<Player>();
                geci.playerId = i;
                geci.name = "LocalPlayer";
                Debug.Log("Local player created!");
            }
            else
            {
                var p = Instantiate(remotePlayer, new Vector3(i*5, 0, 0), Quaternion.identity);
                Mob geci = p.GetComponent<Mob>();
                geci.playerId = i;
                geci.name = "Player" + i.ToString();
            }
        }
        */

        
    }
    
    // Update is called once per frame
     void Update()
     {
        /*Debug.Log("Sending player data!");
        
        TCPClient.Send_Update(p_Datas[localPlayerIndex].Generate_SaveString() + "$" + Generate_Attack_Packet());
        
        float deltaT = Time.time;
        string[] data = TCPClient.Get_Update().Split('$');
        Load_Player_Data(data[0]);
        //Debug.Log("It took " + (Time.time - deltaT) + " seconds to retrive packet");
        Render_Attacks(data[1]);
        //Render_Attacks();
        */

    }
    
    
    public  void Attack(Damage dmg) { attacks.Add(dmg); /*Need to do this*/}
    

    
    private  string Generate_Attack_Packet()
    {
        string msg = "";
        if (attacks.Count == 0)
            return "nothing";
        else
        {
            while (attacks.Count != 0)
            {
                msg += attacks[0].Get_String() + ";";
                attacks.RemoveAt(0);
            }
            return msg;
        }

    }
    private  void Render_Attacks(string data)
    {
        if (data != "nothing")
        {
            Debug.Log(data);
            string[] dataAttacks = data.Split(';');
            
            for (int i = 0; i < dataAttacks.Length; i++)
            {
                Damage d = new Damage(dataAttacks[i]);
                GameObject obj = Resources.Load<GameObject>("Prefabs/" + d.type);
                var p = Instantiate(obj, d.origin, Quaternion.identity);
                if (d.isTravelling == false)
                    p.GetComponent<Hit>().damage = d;
                else
                    p.GetComponent<TravelingHit>().damage = d;
            }
        }
    }
    


}
