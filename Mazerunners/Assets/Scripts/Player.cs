using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Controlled_Mob
{
    protected float attackCd;
    protected float lastAttacked;
    protected Vector2 facing;
    protected Camera cam;
    protected override Vector2 Get_Control()
    {
        Vector2 direction =  new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction != Vector2.zero)
            facing = direction.normalized;
        return direction * 5F;
    }
    protected override void Start()
    {
        base.Start();
        immunityTime = 0.5F;
        cam = GameObject.Find("Camera").GetComponent<Camera>();
    }
    protected override void Update()
    {
        base.Update();
        
        if(Time.time - lastAttacked >= attackCd)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Damage d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 2F), facing, 1, 5F, "BulletTest", false, GameManager.instance.LocalPlayerIndex, true);
                GameManager.instance.Attack(d);
            }
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            if (cam.isFocused == true)
                cam.DeleteFocus();
            else
                cam.CreateFocus(gameObject);
        }
        
        
    }
    protected override void UpdatePlayer(P_data info, bool isForced)
    {
        if (isForced == true)
        {
            base.UpdatePlayer(info, isForced);
        }
    }




}