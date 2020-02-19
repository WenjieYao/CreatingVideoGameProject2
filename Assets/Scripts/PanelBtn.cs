using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnPrefab = null;

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
    public GameObject SpawnPrefab
    {
        get
        {
            return spawnPrefab;
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
