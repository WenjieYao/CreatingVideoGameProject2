using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************/
/****** Script describing spawn unit properties *******/
/******************************************************/

public class UnitProperty : Singleton<UnitProperty>
{
    /******************************************************/
    /******** UnitProperty Class Component ****************/
    /******************************************************/

    /**************** Basic Properties ********************/
    // Private serilize field that can be controlled
    // in the Prefabs Object in Unity
    /******************************************************/
    [SerializeField]
    private int health = 0;

    [SerializeField]
    private int attackPower = 0;

    [SerializeField]
    private int attackRange = 0;

    // Movement distance
    [SerializeField]
    private int movement = 0;

    // Defender or attacker
    [SerializeField]
    private bool isDefender = true;
    
    /********************Other Properties*****************/

    // Check if the unit is already moved in one round
    private bool isMoved = false;
    public bool IsMoved
    {
        get
        {
            return isMoved;
        }
        set
        {
            this.isMoved = value;
        }
    }
    
    /******************************************************/

    // Spawned prefab type {0,1,2,3} = {Tower, Soldier1,2,3}
    [SerializeField]
    private int prefabType = 0;

    // Sprite icon associated with the prefab
    [SerializeField]
    private Sprite nsprite = null;

    /******************************************************/
    // Public field that corresponds to the private field 
    // above that can be accessed by other scripts
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            this.health = value;
        }
    }

    public int AttackPower
    {
        get
        {
            return attackPower;
        }
        set
        {
            this.attackPower = value;
        }
    }

    public int AttackRange
    {
        get
        {
            return attackRange;
        }
        set
        {
            this.attackRange = value;
        }
    }

    public int Movement
    {
        get
        {
            return movement;
        }
    }

    public bool IsDefender
    {
        get
        {
            return isDefender;
        }
    }

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

    /******************************************************/
   
    /******************************************************/
    /**** GameManager Manual Function Controls ************/
    /******************************************************/

    // Check if the mouse is over the current unit
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
