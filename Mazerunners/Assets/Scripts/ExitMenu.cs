using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    
    public void ExitButtonPressed()
    {
        Debug.Log("Exit pressed");
        GameManager.instance.isExit = true;
    }
}
