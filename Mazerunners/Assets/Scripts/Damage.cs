using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage 
{
    public Vector2 origin;
    public Vector2 direction;
    public bool friendly;
    public string type;
    public float dmgAmount;
    public float pushForce;
    public Damage(Vector2 _origin, Vector2 _direction, float _amount, float _pushForce, string _type, bool _friendly)
    {
        this.origin = _origin;
        this.direction = _direction;
        this.dmgAmount = _amount;
        this.pushForce = _pushForce;
        this.type = _type;
        this.friendly = _friendly;
    }
    public Damage(string data)
    {
        string[] traits = data.Split('|');
        this.origin = new Vector2((float)Convert.ToDouble(traits[0]), (float)Convert.ToDouble(traits[1]));
        this.direction = new Vector2((float)Convert.ToDouble(traits[2]), (float)Convert.ToDouble(traits[3]));
        this.dmgAmount = (float)Convert.ToDouble(traits[4]);
        this.pushForce = (float)Convert.ToDouble(traits[5]);
        this.type = traits[6];
        this.friendly = Convert.ToBoolean(traits[7]);

    }
    public string Get_String()
    {
        string s = "";
        s += ((double)origin.x).ToString() + "|" + ((double)origin.y).ToString() + "|";
        s += ((double)direction.x).ToString() + "|" + ((double)direction.y).ToString() + "|";
        s += dmgAmount + "|" + pushForce + "|" + type + "|" + friendly.ToString();
        return s;
        
        
    }
    
}
