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
        info.playerId = GameManager.Get_PlayerIndex();
        playerId = info.playerId;
        info.Health = 10;
        info.Position = Vector3.zero;
        info.Direction = Vector3.zero;
        Debug.Log("Got index of: " + info.playerId);

    }

    protected override void Update()
    {
        base.Update();

    }
    protected virtual void LateUpdate()
    {
        Debug.Log("Getting player update for player" + playerId);
        info = GameManager.Get_PlayerInfo(playerId);
        mytransform.position = info.Position;
        health = info.Health;
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        //Apply on touch hit
        
    }
}
