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
    protected override void Start()
    {
        base.Start();
        render = GetComponent<SpriteRenderer>();
        lastTime = Time.time;
        
        
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

    }
    
}
