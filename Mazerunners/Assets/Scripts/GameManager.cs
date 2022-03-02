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
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        p_Datas = new P_data[2];
        p_Datas[1] = new P_data();
        localPlayerIndex = 0;
        attacks = new List<Damage>();
    }
    
    // Update is called once per frame
     void Update()
     {
        Debug.Log("Sending player data!");
        
        TCPClient.Send_Update(p_Datas[localPlayerIndex].Generate_SaveString());
        TCPClient.Send_Update(Generate_Attack_Packet());
        Load_Player_Data(TCPClient.Get_Update());
        Load_Attack_Data(TCPClient.Get_Update());
        Render_Attacks();
        
        
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
