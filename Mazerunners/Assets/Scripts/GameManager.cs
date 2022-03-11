using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* ================================================Class system:==============================================================
     * -Game Handling Classes:
     *    -> 1.TCP Client: Contains the TCP Client for server communication, this is the first to be activated when the game start
     *    -> 2.Game Manager: Handling everything from starting the game, to send and recieve multiplayer data
     *    
     * -Data Classes:
     *    -> 1.P_data: containing info needed for mob, to be sent and recieved from and to a server
     *    -> 2.Damage: containing info needed for Hit/Attack, to be sent and recieve from and to a server
     *    
     * -Players/Mobs inharitence chain: 
     *   -> 1.Collidable: to cause later on touch hits
     *   -> 2.Mob: This contains movement updates based on server updates using the P_data data class
     *   -> 3.Controlled Mobs: Containing local control based movement, hit detection and everything connected to these
     *   -> 4.Player: Defines the Control and handles the inputs
     *   
     * -Hit inharitance change:
     *   -> 1.Collidable to detect if the player touches it
     *   -> 2.Hits, having anymation and causing player to take hits when touching them
     *   -> 3.Travelling Hits: Adding movement to hits, if it's a projectile
     *   
     * -Others:
     *   -> 1.Camera Class for focusing on and detaching from local player
     *   -> 2.Interactable: for later on objects which the player can innteract with
     *   =========================================================================================================================
     */

    public static GameManager instance; //Instance of the gamemanager currently active
    private  P_data[] p_Datas; //Having all the player locaiton
    private  List<Damage> attacks; //All the attacks generated in one tick, this will be sent to the server and also rendered
    private  int localPlayerIndex; //Having the local playewr index is imporant
    private  int p;  //Required for giving the right index to the right P_data
    bool isSet, isStart; //Needed for smooth game start
    public List<GameObject> players; //All the local and remote players in one List
    public GameObject localPlayer; //Prefab for client
    public GameObject remotePlayer; //Prefab for remote player
    private  int numberOfPlayers; 
    //Gets and Sets for private values:
    public  int NumberOfPlayers { get { return numberOfPlayers; } }
    public  int LocalPlayerIndex { get { return localPlayerIndex; } }
    private void Awake() //Make sure GameManager will not be destroy while travelling to the next Scene
    {
        DontDestroyOnLoad(this);
        instance = this;
    }
    void Start() //Start values
    {
        isSet = false;
        isStart = false;
    }
     void Update()
     {
        if(isStart == true) //isStart will be true when the TCP client connects to the server
        {
            //1. Recieves the ammount of players from the server, and localPlayerIndex
            string[] initalData = TCPClient.instance.Get_Update().Split('|'); 
            numberOfPlayers = Convert.ToInt32(initalData[0]);
            Debug.Log("The amount of players: " + numberOfPlayers);

            //2. Inisializates the Lists, Arrays and
            p_Datas = new P_data[numberOfPlayers];
            attacks = new List<Damage>();
            players = new List<GameObject>();
            localPlayerIndex = Convert.ToInt32(initalData[1]);
            Debug.Log("Got local player id: " + localPlayerIndex);

            //3. Generates the players from Prefabs 
            for (int i = 0; i < p_Datas.Length; i++)
            {
                p_Datas[i] = new P_data();
                Debug.Log("Generating players");
                Debug.Log("Local player id: " + localPlayerIndex);

                if (i == localPlayerIndex)
                {
                    var p = Instantiate(localPlayer, new Vector3(i * 5, 0, 0), Quaternion.identity);
                    Player geci = p.GetComponent<Player>();
                    geci.playerId = i;
                    geci.name = "LocalPlayer";
                    Debug.Log("Local player created!");
                }
                else
                {
                    var p = Instantiate(remotePlayer, new Vector3(i * 5, 0, 0), Quaternion.identity);
                    Mob geci = p.GetComponent<Mob>();
                    geci.playerId = i;
                    geci.name = "Player" + i.ToString();
                }
            }
            //Further setup can be added here, and possible parts of this could be moved into a different function for later reuse upon entering different Scenes
            isSet = true; //sets isSet = true, so normal gameupdates can start
        }
        if (isSet == true)
        {
            TCPClient.instance.Send_Update(p_Datas[localPlayerIndex].Generate_SaveString() + "$" + Generate_Attack_Packet()); //Sending Local updates to server (local playerdata + attacks created by this client)
            string[] data = TCPClient.instance.Get_Update().Split('$');
            Load_Player_Data(data[0]); //Setting all the players to the recieved states
            Render_Attacks(data[1]); //Calling all the recieved attacks to be rendered
        }
        
        
     }
    public  P_data Get_PlayerInfo(int playerIndex){ return p_Datas[playerIndex]; } //Called from mob classes to update their info
    public  void Set_LocalPlayerInfo(P_data data) { p_Datas[localPlayerIndex] = data; } //Called by the player class to set the Local player value to be sent to the server
    public  void Attack(Damage dmg) { attacks.Add(dmg); } //Function called whenever a player attacks: adding the attack into the List needed to be sent and attacked
    public  int Get_PlayerIndex() //Giving each mob and children classes their indexes
    {
        p++;
        return p - 1;
    }

    private  void Load_Player_Data(string msg) //Loading the server message into the P_datas array
    {
        string[] data = msg.Split(';');
        for (int i = 0; i < p_Datas.Length; i++)
        {
            p_Datas[i].Set_Values(data[i]);
        }
    }
    private  string Generate_Attack_Packet() //Generates the attack part of the server string to be sent
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
    private  void Render_Attacks(string data) //Gets the attacks string from the server and renders them
    {
        if (data != "nothing")
        {
            Debug.Log("Attacks being rendered: " + data);
            string[] dataAttacks = data.Split(';');
            for (int i = 0; i < dataAttacks.Length; i++)
            {
                Damage d = new Damage(dataAttacks[i]);
                GameObject obj = Resources.Load<GameObject>("Prefabs/" + d.type); //Searching the created Hit from the prefabs
                var p = Instantiate(obj, d.origin, Quaternion.identity);
                if (d.isTravelling == false)
                    p.GetComponent<Hit>().damage = d; //Determines weather stationary
                else
                    p.GetComponent<TravelingHit>().damage = d; //Or travelling hit
            }
        }
    }
    


}
