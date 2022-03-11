using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public bool isFocused, isReached;
    public Transform inFocus;


    public void CreateFocus(GameObject obj) //Creates a focus target for the camera
    {
        inFocus = obj.GetComponent<Transform>();
        isFocused = true;
    }
    public void DeleteFocus() //Removes the focus target for the camera
    {
        inFocus = null;
        transform.parent = null;
        isFocused = false;
        isReached = false;
    }
    void Start()  //Setting focus and reached values
    {
        isFocused = false;
        isReached = false;
    }
    void Update()
    {
        if(isFocused == true && isReached == false) //If the there is a focus but it's not reached yet
        {
            Vector2 moveDelta = inFocus.position - transform.position;

            if(moveDelta.magnitude > 1F)//If we are not in capture distance move towards it
            {
                transform.position += new Vector3(moveDelta.x / 30F, moveDelta.y / 30F, 0);
            }
            else //If we capture it attach itself to the object 
            {
                isReached = true;
                transform.parent = inFocus;
            }
        }
    }   
}
