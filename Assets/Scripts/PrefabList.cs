using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabList : Singleton<PrefabList>
{
    [SerializeField]
    private GameObject[] prefabList = null;//obj kinds
    // Start is called before the first frame update
    public GameObject[] PfList
    {
        get
        {
            return prefabList;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
