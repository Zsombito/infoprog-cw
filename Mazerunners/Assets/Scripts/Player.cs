using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Controlled_Mob
{
    //Combat:
    protected float attackCd;
    protected float lastAttacked;
    protected int numberOfAttacks = 0;
    //Render variables
    protected Vector2 facing;
    //protected CamerMotor cam;
    protected override Vector2 Get_Control() //Reads the inputs and generates moveDelta based on that
    {
        Vector2 direction =  new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(direction != Vector2.zero)
            facing = direction.normalized;
        if (Input.GetKey(KeyCode.F))
            return direction * 2F;
        else
            return direction * 1F;
    }
    protected override void Start()
    {
        base.Start();
        immunityTime = 0.2F;
        if (playerId == 0)
            attackCd = 0.1F;
        else if (playerId == 1)
            attackCd = 0.05F;
        else if (playerId == 2)
            attackCd = 0.5F;
        else if (playerId == 3)
            attackCd = 0.5F;
        
        GameObject.Find("MainCamera").GetComponent<CamerMotor>().FocusOnPlayer(mytransform);
    }
    protected override void Update() //Checks for further controls
    {
        base.Update();
        GameManager.instance.Set_LocalPlayerInfo(info);
        if(Time.time - lastAttacked >= attackCd) //If attack is not on couldown and space is presed cause attack
        {
            if(Input.GetKey(KeyCode.Q))
            {
                
                Damage d;
                if (playerId == 0)
                    d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 0.5F), facing, 5, 1F, "BWizardBullet", false, GameManager.instance.LocalPlayerIndex, true); //Defining the damage type
                else if(playerId == 1 && numberOfAttacks % 2 == 0)
                    d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 0.5F), facing, 10, 1F, "BNerdBulletZero", false, GameManager.instance.LocalPlayerIndex, true);
                else if (playerId == 1 && numberOfAttacks % 2 == 1)
                    d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 0.5F), facing, 10, 1F, "BNerdBulletOne", false, GameManager.instance.LocalPlayerIndex, true);
                else if (playerId == 2)
                    d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 0.5F), facing, 10, 2.5F, "BCowboyBullet", false, GameManager.instance.LocalPlayerIndex, true);
                else 
                    d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 0.5F), facing, 20, 5F, "BOrcBullet", false, GameManager.instance.LocalPlayerIndex, true);
                numberOfAttacks++;
                GameManager.instance.Attack(d); //Sending attack to the gamemanager for processing
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                GameManager.instance.ExitMenu();
                
        }
        /*if(Input.GetKeyDown(KeyCode.P)) //If key P is pressed toggle's the camera to be centered or to stay in place
        {
            if (cam.isFocused == true)
                cam.DeleteFocus();
            else
                cam.CreateFocus(gameObject);
        }*/
        
    }
    protected override void Death()
    {
        info.Health = 50;
        info.Position = spawnpoint;
    }



}