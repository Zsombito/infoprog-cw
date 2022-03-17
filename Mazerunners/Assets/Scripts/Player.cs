using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Controlled_Mob
{
    //Combat:
    protected float attackCd;
    protected float lastAttacked;
    //Render variables
    protected Vector2 facing;
    //protected CamerMotor cam;
    protected override Vector2 Get_Control() //Reads the inputs and generates moveDelta based on that
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            direction = new Vector2(-1, 0);
        else if (Input.GetKey(KeyCode.D))
            direction = new Vector2(1, 0);
        else if (Input.GetKey(KeyCode.S))
            direction = new Vector2(0, -1);
        else if (Input.GetKey(KeyCode.W))
            direction = new Vector2(0, 1);
        if (Input.GetKey(KeyCode.H))
            direction = new Vector2(-2, 0);
        else if (Input.GetKey(KeyCode.K))
            direction = new Vector2(2, 0);
        else if (Input.GetKey(KeyCode.J))
            direction = new Vector2(0, -2);
        else if (Input.GetKey(KeyCode.U))
            direction = new Vector2(0, 2);


        if (direction != Vector2.zero)
            facing = direction.normalized;
        return direction * 1F;
    }
    protected override void Start()
    {
        base.Start();
        immunityTime = 0.5F;
        GameObject.Find("MainCamera").GetComponent<CamerMotor>().FocusOnPlayer(mytransform);
    }
    protected override void Update() //Checks for further controls
    {
        base.Update();
        GameManager.instance.Set_LocalPlayerInfo(info);
        if(Time.time - lastAttacked >= attackCd) //If attack is not on couldown and space is presed cause attack
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Damage d = new Damage(new Vector2(transform.position.x, transform.position.y) + (facing * 0.5F), facing, 1, 5F, "BulletTest", false, GameManager.instance.LocalPlayerIndex, true); //Defining the damage type
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
    
    


}