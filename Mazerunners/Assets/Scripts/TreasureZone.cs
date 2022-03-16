using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureZone : Collidable
{


    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if(coll.name == "LocalPlayer"&&GameManager.instance.isWin == false)
        {
            GameManager.instance.MiddleReached();
        }
    }
}
