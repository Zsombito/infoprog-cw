using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collidable : MonoBehaviour
{
    //All the necessary objects needed for collision detection
    public ContactFilter2D filter;
    protected BoxCollider2D boxCollider;
    protected Collider2D[] hits ;
    protected virtual void Start() //Inisiating the components
    {
        boxCollider = GetComponent<BoxCollider2D>();
        hits = new Collider2D[10];
    }

    protected virtual void Update() //Check for collisions, if there is a collision it calls onCollide
    {
        boxCollider.OverlapCollider(filter, hits); 
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            OnCollide(hits[i]);
            hits[i] = null;
        }
    }
    protected virtual void OnCollide(Collider2D coll) //Only this function needs to be changed in children classes, for different collision effects
    {
        Debug.Log(coll.name);
    }
}
