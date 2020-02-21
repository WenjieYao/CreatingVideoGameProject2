using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************/
/*********  Stores the spawn prefabs in a list ********/
/******************************************************/

public class SpawnList : Singleton<SpawnList>
{
    // Private serilize field that can be 
    // controlled using the SpawnList Object in Unity
    [SerializeField]
    private GameObject[] spawnList = null;
    
    // Public spawn list that can be accessed by other scripts
    public GameObject[] SPList
    {
        get
        {
            return spawnList;
        }
    }
    
}
