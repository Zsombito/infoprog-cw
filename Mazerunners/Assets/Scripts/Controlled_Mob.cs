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
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lastHit = Time.time;
        
    }
    protected override void Update()
    {
        base.Update();
        if(isHit == true)
        {
            moveDelta = Knockback(currentHit);
            GameManager.instance.Set_LocalPlayerInfo(info);
        }
        else
        {
            moveDelta = Get_Control();
            if(moveDelta != Vector3.zero)
            {
                GameManager.instance.Set_LocalPlayerInfo(info);
            }
        }
        moveActual = Vector3.zero;
        hit = Physics2D.BoxCast(mytransform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking"));

        if (hit.collider == null)
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
        info.Position = mytransform.position + moveActual;
        info.Direction = moveActual;
        mytransform.position = info.Position;
    }
    // Update is called once per frame
    public virtual void Hit(Damage dmg)
    {
        if (Time.time - lastHit >= immunityTime)
        {
            //Cause dmg
            Debug.Log("Damage caused");
            info.Health -= dmg.dmgAmount;
            if (info.Health <= 0)
            {
                Death();
            }
            isHit = true;
            lastHit = Time.time;
            Knockback(dmg);
            
        }
        currentHit = dmg;
    }
    public virtual Vector2 Knockback(Damage dmg)
    {
        
        if (Time.time - lastHit >= 0.3F)
        {
            isHit = false;
            currentHit = null;
            Debug.Log("Knocback over");
            return Vector2.zero;
            
        }
        else
        {
            Debug.Log("Knocback");
            Vector2 knockBack;
            if (dmg.direction == Vector2.zero)
                knockBack = new Vector2(mytransform.position.x - dmg.origin.x, mytransform.position.y - dmg.origin.y).normalized * dmg.pushForce;
            else
                knockBack = dmg.direction * dmg.pushForce;
            isHit = true;
            
            return knockBack;

        }
    }
    protected virtual Vector2 Get_Control()
    {
        return Vector2.zero;
    }
    protected virtual void Death()
    {

    }
}
