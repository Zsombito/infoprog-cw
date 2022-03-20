using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingHit : Hit
{
    public float speed;
    public RaycastHit2D hit;
    public bool isBouncy;
    public bool isPenetrating;
    protected override void Update() //Making the hit travel and detecting if it collides with wall/or something
    {
        base.Update();
        Vector2 moveDelta = Vector2.zero;
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0,damage.direction.y), Mathf.Abs(damage.direction.y * Time.deltaTime), LayerMask.GetMask("Blocking", "Player"));
        if (hit.collider == null)
            moveDelta.y = damage.direction.y * Time.deltaTime * speed;
        else if (isBouncy == true)
            damage.direction.y = -damage.direction.y;
        else
            GameObject.Destroy(gameObject);
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, damage.direction.x), Mathf.Abs(damage.direction.x * Time.deltaTime), LayerMask.GetMask("Blocking", "Player"));
        if (hit.collider == null)
            moveDelta.x = damage.direction.x * Time.deltaTime * speed;
        else if (isBouncy == true)
            damage.direction.x = -damage.direction.x;
        else
            GameObject.Destroy(gameObject);

        transform.position += new Vector3(moveDelta.x, moveDelta.y, 0);

    }
    protected override void OnCollide(Collider2D coll) //Further to hitting player, if it's not penetrating it passes through them
    {
        base.OnCollide(coll);
        Debug.LogError(coll.name);
        if (!(coll.name.StartsWith("B") || coll.name.EndsWith(damage.owner.ToString()) || (damage.owner == GameManager.instance.LocalPlayerIndex && coll.name == "LocalPlayer")))   
        {
            Debug.Log("Execute bullet");
            GameObject.Destroy(gameObject);
        }
    }
}
