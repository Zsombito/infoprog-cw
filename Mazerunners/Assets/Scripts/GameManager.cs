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
    public List<GameObject> players;
    public GameObject localPlayer;
    public GameObject remotePlayer;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
        
    }
    void Start()
    {
        p_Datas = new P_data[2];
        attacks = new List<Damage>();
        players = new List<GameObject>();
        localPlayerIndex = Convert.ToInt32(TCPClient.Get_Update());
        p_Datas[0] = new P_data();
        p_Datas[1] = new P_data();
        for (int i = 0; i < 2; i++)
        {
            Debug.Log("Generating players");
            Debug.Log("Local player id: " + localPlayerIndex);
            
            if(i == localPlayerIndex)
            {
                var p = Instantiate(localPlayer, new Vector3(i*5, 0, 0), Quaternion.identity);
                Player geci = p.GetComponent<Player>();
                geci.playerId = i;
                Debug.Log("Local player created!");
            }
            else
            {
                var p = Instantiate(remotePlayer, new Vector3(i*5, 0, 0), Quaternion.identity);
                Mob geci = p.GetComponent<Mob>();
                geci.playerId = i;
            }
        }

        
    }
    
    // Update is called once per frame
     void Update()
     {
        Debug.Log("Sending player data!");
        
        TCPClient.Send_Update(p_Datas[localPlayerIndex].Generate_SaveString());
        //TCPClient.Send_Update(Generate_Attack_Packet());
        float deltaT = Time.time;
        Load_Player_Data(TCPClient.Get_Update());
        Debug.Log("It took " + (Time.time - deltaT) + " seconds to retrive packet");
        //Load_Attack_Data(TCPClient.Get_Update());
        //Render_Attacks();
        
        
     }
    public static P_data Get_PlayerInfo(int playerIndex){ return p_Datas[playerIndex]; }
    public static void Set_LocalPlayerInfo(P_data data) { p_Datas[localPlayerIndex] = data; }
    public static void Attack(Damage dmg) { attacks.Add(dmg); }
    public static int Get_PlayerIndex()
    {
        p++;
        return p - 1;
    }
    private static string Generate_Attack_Packet()
    {
        string msg = "";
        while(attacks.Count != 0)
        {
            msg += attacks[0].Get_String();
            attacks.RemoveAt(0);
        }
        return msg;
            
    }
    private static void Load_Player_Data(string msg)
    {
        string[] data = msg.Split(';');
        for (int i = 0; i < p_Datas.Length; i++)
        {
            Debug.Log("Setting player" + i + " value: " + data[i]);
            p_Datas[i].Set_Values(data[i]);

        }
    }
    private static void Load_Attack_Data(string msg)
    {
        string[] data = msg.Split(';');
        for(int i = 0; i < data.Length; i++)
            attacks.Add(new Damage(data[i]));
    }
    private static void Render_Attacks() { }
    


}
