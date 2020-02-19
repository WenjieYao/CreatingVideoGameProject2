using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFun : Singleton<ObjectFun>
{
    [SerializeField]
    private int prefabType = 0;

    [SerializeField]
    private Sprite nsprite = null;

    [SerializeField]
    private int movement = 0;

    public Sprite Nsprite
    {
        get
        {
            return nsprite;
        }
    }

    public int PrefabType
    {
        get
        {
            return prefabType;
        }
    }

    public int Movement
    {
        get
        {
            return movement;
        }
    }

    [SerializeField]
    private bool isDefender = true;
    public bool IsDefender
    {
        get
        {
            return isDefender;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   

    private bool IsMouseOver(){
        Vector3 CameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 ObjectPos = transform.position;
        //Debug.Log(CameraPos);
        //Debug.Log(ObjectPos);
        float offL = 0.5F;
        if (Mathf.Abs(CameraPos.x-ObjectPos.x-offL)<0.5 && Mathf.Abs(CameraPos.y-ObjectPos.y+offL)<0.5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
