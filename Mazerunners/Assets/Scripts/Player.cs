using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Controlled_Mob
{
    protected override Vector2 Get_Control()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    protected override void Update()
    {
        base.Update();
        GameManager.Set_LocalPlayerInfo(info);
    }



}