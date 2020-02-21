using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************/
/**************   Finish Button Class  ****************/
/******************************************************/

public class FinishBtn : MonoBehaviour
{
    // Private serilize field that can be 
    // controlled using the FinishD/A Button in Unity
    [SerializeField]
    private bool isDefender = true;

    // Public field that corresponds to the private 
    // field above that can be accessed by other scripts
    public bool IsDefender // defender or attacker
    {
        get
        {
            return isDefender;
        }
    }
    
}
