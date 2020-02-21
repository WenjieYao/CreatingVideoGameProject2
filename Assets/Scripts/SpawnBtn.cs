using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/******************************************************/
/********** Button used for spawning units ************/
/******************************************************/

public class SpawnBtn : MonoBehaviour
{
    /******************************************************/
    /******** SpawnBtn Class Component ********************/
    /******************************************************/

    // Private serilize field that can be controlled
    // in the Tower/Soldier/CaptainButton in Unity
    /******************************************************/

    // Spawned prefab type {0,1,2,3} = {Tower, Soldier1,2,3}
    [SerializeField]
    private int prefabType = 0;

    // Sprite icon associated with the prefab
    [SerializeField]
    private Sprite nsprite = null;

    // Defender or attacker
    [SerializeField]
    private bool isDefender = true;

    // Price to spawn the unit
    [SerializeField]
    private int price = 0;

    // Associated price display text
    [SerializeField]
    private Text priceTxt = null;

    // Number of units left that can be spawned
    [SerializeField]
    private int numLeft = 0;

    /******************************************************/
    // Public field that corresponds to the private field 
    // above that can be accessed by other scripts
    public int PrefabType
    {
        get
        {
            return prefabType;
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

    public int Price
    {
        get
        {
            return price;
        }
    }
    
    public int NumLeft 
    {
        get
        {
            return numLeft;
        }
        set
        {
            this.numLeft = value;
        }
    }


    /******************************************************/

    /******************************************************/
    /******** SpawnBtn Basic Functions ********************/
    /******************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialize price display
        priceTxt.text = "$" + price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
