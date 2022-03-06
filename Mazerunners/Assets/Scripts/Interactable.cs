using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Collidable
{
    protected float interactTime;
    protected float lastInteract;
    protected override void Start()
    {
        base.Start();
        lastInteract = -10F;
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if(Input.GetKeyDown(KeyCode.E) && coll.name == "LocalPlayer" && Time.time - lastInteract >= interactTime )
        {
            lastInteract = Time.time;
            Interact();
        }
    }
    protected virtual void Interact()
    {

    }
}
