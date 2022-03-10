using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static P_data[] p_Datas;
    private static List<Damage> attacks;
    private static int localPlayerIndex;
    private static int p;
    bool isSet;
    public List<GameObject> players;
    public GameObject localPlayer;
    public GameObject remotePlayer;
    private static int numberOfPlayers;
    public static int NumberOfPlayers { get { return numberOfPlayers; } }
    public static int LocalPlayerIndex { get { return localPlayerIndex; } }
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
    }
    void Start()
    {
        isSet = false;
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
        isSet = true;

        
    }
    
    // Update is called once per frame
     void Update()
     {
        if (isSet == true)
        {
            //Debug.Log("Sending player data!");

            TCPClient.Send_Update(p_Datas[localPlayerIndex].Generate_SaveString() + "$" + Generate_Attack_Packet());

            float deltaT = Time.time;
            string[] data = TCPClient.Get_Update().Split('$');
            Load_Player_Data(data[0]);
            //Debug.Log("It took " + (Time.time - deltaT) + " seconds to retrive packet");
            Render_Attacks(data[1]);
            //Render_Attacks();
        }
        
        
     }
    public static P_data Get_PlayerInfo(int playerIndex){ return p_Datas[playerIndex]; }
    public static void Set_LocalPlayerInfo(P_data data) { p_Datas[localPlayerIndex] = data; }
    public static void Attack(Damage dmg) { attacks.Add(dmg); }
    public static int Get_PlayerIndex()
    {
        p++;
        return p - 1;
    }

    private static void Load_Player_Data(string msg)
    {
        string[] data = msg.Split(';');
        for (int i = 0; i < p_Datas.Length; i++)
        {
            //Debug.Log("Setting player" + i + " value: " + data[i]);
            p_Datas[i].Set_Values(data[i]);

        }
    }
    private static string Generate_Attack_Packet()
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
    private static void Render_Attacks(string data)
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
