using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    protected int playerId;
    protected P_data info;
    protected int health;
    protected Vector3 moveDelta;
    protected Vector3 moveActual;
    protected bool isControlled;
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;
    // Start is called before the first frame update
    protected void Start()
    {
        info = new P_data();
        
        boxCollider = GetComponent<BoxCollider2D>();
        info.playerId = GameManager.Get_PlayerIndex();
        playerId = info.playerId;
        info.Health = 10;
        info.Position =  Vector3.zero;
        info.Direction = Vector3.zero;
        Debug.Log("Got index of: " + info.playerId);
        if(info.playerId == 0)
        {
            isControlled = true;
        }
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        if(isControlled == true)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            moveDelta = new Vector3(x * 2.5F, y * 2.5F, 0);
            moveActual = Vector3.zero;
            hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking"));

            if (hit.collider == null)
            {
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime * 4.5F), LayerMask.GetMask("Player"));
                if (hit.collider == null)
                    moveActual.y = moveDelta.y * Time.deltaTime;
                else
                    moveActual.y = 0;
            }
            else
            {
                moveActual.y = 0;
            }
            hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask( "Blocking"));

            if (hit.collider == null)
            {
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime * 2.5F), LayerMask.GetMask("Player"));
                if (hit.collider == null)
                    moveActual.x = moveDelta.x * Time.deltaTime;
                else
                    moveActual.x = 0;
            }       
            else
            {
                moveActual.x = 0;
            }
            info.Position = transform.position + moveActual;
            info.Direction = moveActual;
            GameManager.Set_LocalPlayerInfo(info);
            

        }
        

    }
    protected void LateUpdate()
    {
        Debug.Log("Getting player update for player" + playerId);
        info = GameManager.Get_PlayerInfo(playerId);
        transform.position = info.Position;
        health = info.Health;
    }
}
