using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class P_data 
{
    public Vector3 Position { get { return position; } set { position = value; } }
    public Vector3 Direction { get { return direction; } set { direction = value; } }
    public float Health { get { return health; } set { health = value; } }
    private Vector3 position;
    private Vector3 direction;
    private float health;
    public int playerId;
    
    public void Set_Values(Vector3 _pos, Vector3 _dir, int _health)
    {
        position = _pos;
        direction = _dir;
        health = _health;
    }
    public void Set_Values(string p_save)
    {
        Debug.Log("Setting values");
        string[] data = p_save.Split('|');
        position = new Vector3((float)(Convert.ToDouble(data[1])), (float)(Convert.ToDouble(data[2])), (float)(Convert.ToDouble(data[3])));
        direction = new Vector3((float)(Convert.ToDouble(data[4])), (float)(Convert.ToDouble(data[5])), (float)(Convert.ToDouble(data[6])));
        health = (float)Convert.ToDouble(data[7]);
    }
    public string Generate_SaveString()
    {
        string r = playerId + "|";
        r += position.x + "|" + position.y + "|" + position.z + "|";
        r += direction.x + "|" + direction.y + "|" + direction.z + "|";
        r += health;
        return r;
    }
    
}
