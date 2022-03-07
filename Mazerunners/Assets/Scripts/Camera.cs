using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public bool isFocused;
    public int Focus;
    public bool isReached;
    public Transform inFocus;
    // Start is called before the first frame update
    void Start()
    {
        isFocused = false;
        isReached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFocused == true && isReached == false)
        {
            Vector2 moveDelta = inFocus.position - transform.position;
            if(moveDelta.magnitude > 1F)
            {
                transform.position += new Vector3(moveDelta.x / 30F, moveDelta.y / 30F, 0);
                
            }
            else
            {
                isReached = true;
                transform.parent = inFocus;
            }
        }
        
        


    }
    public void CreateFocus(GameObject obj)
    {
        inFocus = obj.GetComponent<Transform>();
        isFocused = true;
    }
    public void DeleteFocus()
    {
        inFocus = null;
        transform.parent = null;
        isFocused = false;
        isReached = false;
    }
}
