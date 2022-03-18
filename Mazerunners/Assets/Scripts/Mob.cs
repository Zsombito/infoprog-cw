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
    protected Vector3 previousLocation;
    public Animator animator;
    protected bool isAttack = false;
    protected float lastAttack;
    protected Vector3 spawnpoint;
   protected override void Start() //Inisiating the data
    {
        base.Start();
        info = new P_data();
        mytransform = GetComponent<Transform>();
        info.playerId = GameManager.instance.Get_PlayerIndex();
        playerId = info.playerId;
        info.Health = 10;
        info.Position = mytransform.position;
        spawnpoint = mytransform.position;
        info.Direction = Vector3.zero;
        Debug.Log("Got index of: " + info.playerId);
        previousLocation = info.Position;
        animator = gameObject.GetComponent<Animator>();
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
        if ((info.Position - previousLocation).x < 0)
            mytransform.localScale = new Vector3(1, 1, 1);
        else
            mytransform.localScale = new Vector3(-1, 1, 1);
        Debug.Log((info.Position - previousLocation).magnitude);
        animator.SetFloat("Speed", (info.Position - previousLocation).magnitude);
        if (GameManager.instance.isAttacking[playerId])
        {
            animator.SetBool("Attack", true);
            lastAttack = Time.time;
            GameManager.instance.isAttacking[playerId] = false;
        }
        if (Time.time - lastAttack >= 0.3F)
            animator.SetBool("Attack", false);

        previousLocation = info.Position;
        
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        //Apply on touch hit
        
    }
}
