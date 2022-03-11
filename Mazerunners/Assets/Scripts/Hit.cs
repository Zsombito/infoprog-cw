using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : Collidable
{
    public Sprite[] sprites; //For animation a series of pictures fallowing each other
    protected SpriteRenderer render;
    public float lasting;
    public int maxStages;
    public int stage;
    public float lastTime;
    public Damage damage ; //Damage type
    public Player player; //To check for, this is done to the Local player in each Player's version only registering it to the client player
    protected override void Start() //All basic starting values
    {
        base.Start();//Hitbox initializasiton
        render = GetComponent<SpriteRenderer>();
        lastTime = Time.time;
        GameObject p = GameObject.Find("LocalPlayer");
        player = p.GetComponent<Player>();
        stage = 0;
    }
    protected override void Update()
    {
        base.Update(); //Hitbox detection
        if (stage == maxStages) //Detecting when the hit will be over
            GameObject.Destroy(gameObject);

        if(Time.time - lastTime >= (float)(lasting / maxStages)) //Basic animation
        {
            stage++;
            lastTime = Time.time;
            render.sprite = sprites[stage];
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll); //Debug messages for detecting hits
        Debug.Log("Trying to hurt");
        if (coll.name == "LocalPlayer" && damage.owner != GameManager.instance.LocalPlayerIndex) //If the player hit is the local player, call the hit function in the player (if time allows would be nice with events)
            player.Hit(damage);
    }
    
}
