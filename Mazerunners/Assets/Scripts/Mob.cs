using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Collidable
{
    public Vector2 position;
    public float health;
    protected P_data info;
    public int playerId;
    protected Transform mytransform;
    
   protected override void Start()
    {
        base.Start();
        info = new P_data();
        mytransform = GetComponent<Transform>();
        GameManager.instance.onPlayerDataUpdate += UpdatePlayer;
        

    }

    protected override void Update()
    {
        base.Update();
        mytransform.position = info.Position;

    }
    protected virtual void UpdatePlayer(P_data info, bool isForced)
    {
        Debug.Log("Is: " + info.playerId + " == " + this.playerId);
        if (info.playerId == this.playerId)
        {
            Debug.Log("Getting player update for player" + playerId);
            this.info = info;
            Debug.Log("Getting player update for player" + info.Position.ToString());
            //GetComponent<Transform>().position = info.Position;
            Debug.Log("Getting player update for player" + playerId);
            health = info.Health;
            Debug.Log("Getting player update for player" + playerId);
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        //Apply on touch hit
        
    }
}
