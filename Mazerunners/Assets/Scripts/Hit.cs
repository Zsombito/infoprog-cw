using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : Collidable
{
    public Sprite[] sprites;
    protected SpriteRenderer render;
    public float lasting;
    public int maxStages;
    public int stage;
    public float lastTime;
    public Damage damage;
    public Player player;
    protected override void Start()
    {
        base.Start();
        render = GetComponent<SpriteRenderer>();
        lastTime = Time.time;
        GameObject p = GameObject.Find("LocalPlayer");
        player = p.GetComponent<Player>();
        stage = 0;
        
    }
    protected override void Update()
    {
        base.Update();
        if (stage == maxStages)
            GameObject.Destroy(gameObject);
        //Debug.Log(Time.time - lastTime);
        if(Time.time - lastTime >= (float)(lasting / maxStages))
        {
            stage++;
            lastTime = Time.time;
            render.sprite = sprites[stage];
            
        }
        
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if (coll.name == "LocalPlayer")
            player.Hit(damage);
    }
    
}
