using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlled_Mob : Mob
{
    //Movement:
    protected Vector3 moveDelta;
    protected Vector3 moveActual;
    protected RaycastHit2D hit;
    //Combat:
    protected float immunityTime;
    protected float lastHit;
    protected float knockbackResistance;
    protected bool isHit;
    protected Damage currentHit;

    protected override void Start()
    {
        base.Start(); //All the mob attribute initializastion
        lastHit = Time.time;
    }
    protected override void Update()
    {
        base.Update();
        if(isHit == true) //If it is hit and knocback still holds, it deprives the mob of control and replace is with knockback
        {
            moveDelta = Knockback(currentHit);
        }
        else
        {
            moveDelta = Get_Control();
        }

        //Basic hitbox code to not go into solids
        moveActual = Vector3.zero;
        hit = Physics2D.BoxCast(mytransform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking")); 

        if (hit.collider == null) //Tries to not go near a palyer in order to avoid glitching together due to lag
        {
            hit = Physics2D.BoxCast(mytransform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime * 4.5F), LayerMask.GetMask("Player")); 
            if (hit.collider == null)
                moveActual.y = moveDelta.y * Time.deltaTime;
            else
                moveActual.y = 0;
        }
        else
        {
            moveActual.y = 0;
        }
        hit = Physics2D.BoxCast(mytransform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking"));

        if (hit.collider == null)
        {
            hit = Physics2D.BoxCast(mytransform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime * 2.5F), LayerMask.GetMask("Player"));
            if (hit.collider == null)
                moveActual.x = moveDelta.x * Time.deltaTime;
            else
                moveActual.x = 0;
        }
        else
        {
            moveActual.x = 0;
        }
        //Updates the info of the mob
        info.Position = mytransform.position + moveActual;
        info.Direction = moveActual;
        mytransform.position = info.Position;
    }
    public virtual void Hit(Damage dmg) //Called by a Hit to cause damage to a player
    {
        if (Time.time - lastHit >= immunityTime) //If the mob is not currently immune from last attack
        {
            //Causing the damage
            Debug.Log("Damage caused");
            info.Health -= dmg.dmgAmount;
            if (info.Health <= 0)
            {
                Death(); //Needs to be written
            }
            //Start knockback
            isHit = true;
            lastHit = Time.time;
            Knockback(dmg);
            
        }
        currentHit = dmg;
    }
    public virtual Vector2 Knockback(Damage dmg)
    {
        
        if (Time.time - lastHit >= 0.3F) //If knockback is over
        {
            isHit = false;
            currentHit = null;
            Debug.Log("Knocback over");
            return Vector2.zero;
            
        }
        else //Returns the moveDelta vector if active
        {
            Debug.Log("Knocback");
            Vector2 knockBack;
            if (dmg.direction == Vector2.zero) //If its an explosion type with radial knockback
                knockBack = new Vector2(mytransform.position.x - dmg.origin.x, mytransform.position.y - dmg.origin.y).normalized * dmg.pushForce;
            else //If it's like a slash with set direction
                knockBack = dmg.direction * dmg.pushForce;
            isHit = true;
            
            return knockBack;

        }
    }
    protected virtual Vector2 Get_Control() //Used to set moveDelta with control
    {
        return Vector2.zero;
    }
    protected virtual void Death() //Not yet written death function
    {

    }
}
