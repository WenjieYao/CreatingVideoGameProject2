using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBtn : MonoBehaviour
{
    [SerializeField]
    private int spawnPrefab = 0;

    [SerializeField]
    private Sprite nsprite = null;

    [SerializeField]
    private bool isDefender = true;
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

    [SerializeField]
    private int price = 0;

    public int Price
    {
        get
        {
            return price;
        }
    }
    [SerializeField]
    private Text priceTxt = null;
    public int SpawnPrefab
    {
        get
        {
            return spawnPrefab;
        }
    }

    [SerializeField]
    private int numLeft = 0;
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


    // Start is called before the first frame update
    void Start()
    {
        priceTxt.text = "$" + price.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
