using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Collidable
{
    protected P_data info;//Contains everything which the server needs to know
    public Vector2 position; 
    public float health;
    public int playerId;
    protected Transform mytransform;
    
   protected override void Start() //Inisiating the data
    {
        base.Start();
        info = new P_data();
        mytransform = GetComponent<Transform>();
        info.playerId = GameManager.instance.Get_PlayerIndex();
        playerId = info.playerId;
        info.Health = 10;
        info.Position = mytransform.position;
        info.Direction = Vector3.zero;
        Debug.Log("Got index of: " + info.playerId);

    }

    protected override void Update()
    {
        base.Update();

    }
    protected virtual void LateUpdate() //Calling the GameManager to send the new P_data to move the mob around etc...
    {
        if (playerId != GameManager.instance.LocalPlayerIndex)
        {
            Debug.Log("Getting player update for player" + playerId);
            info = GameManager.instance.Get_PlayerInfo(playerId);
            Debug.Log(playerId + " got the position of: " + info.Position);
            mytransform.position = info.Position;
            health = info.Health;
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        //Apply on touch hit
        
    }
}
