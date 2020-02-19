using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    //[SerializeField]
    public PanelBtn ClickBtn { get; set; }

    private int DMoney;

    private int AMoney;

    [SerializeField]
    private Text DTxt = null;

    [SerializeField]
    private Text ATxt = null;

    public int pDMoney
    {
        get
        {
            return DMoney;
        }
        set
        {
            this.DMoney = value;
            this.DTxt.text = "$" + value.ToString();
        }
    }

    public int pAMoney
    {
        get
        {
            return AMoney;
        }
        set
        {
            this.AMoney = value;
            this.ATxt.text = "$" + value.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pDMoney = 7;
        pAMoney = 7;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void PickSpawn(PanelBtn spawnBtn)
    {
        if (spawnBtn.IsDefender)
        {
            if (pDMoney >= spawnBtn.Price){
                this.ClickBtn = spawnBtn;
                Hover.Instance.Activate(spawnBtn.Nsprite);
            }
        }
        else
        {
            if (pAMoney >= spawnBtn.Price){
                this.ClickBtn = spawnBtn;
                Hover.Instance.Activate(spawnBtn.Nsprite);
            }
        }
    }

    public void MoneyCost()
    {
        if (ClickBtn.IsDefender)
        {
            if (pDMoney >= ClickBtn.Price)
            {
                pDMoney -= ClickBtn.Price;
                Hover.Instance.Deactivate();
            }
        }
        else
        {
            if (pAMoney >= ClickBtn.Price)
            {
                pAMoney -= ClickBtn.Price;
                Hover.Instance.Deactivate();
            }
        }
        
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }
}
