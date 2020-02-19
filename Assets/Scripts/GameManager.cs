using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    int DM0 = 8;
    int AM0 = 8;
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
        pDMoney = DM0;
        pAMoney = AM0;
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
            if (pDMoney >= spawnBtn.Price && spawnBtn.NumLeft>0){
                this.ClickBtn = spawnBtn;
                Hover.Instance.Activate(spawnBtn.Nsprite);
            }
        }
        else
        {
            if (pAMoney >= spawnBtn.Price && spawnBtn.NumLeft>0){
                this.ClickBtn = spawnBtn;
                Hover.Instance.Activate(spawnBtn.Nsprite);
            }
        }
    }

    public void FinishRound(FinishBtn finishBtn)
    {
        if (finishBtn.IsDefender)
        {
            pDMoney = DM0;
        }
        else
        {
            pAMoney = AM0;
        }
    }
    public void MoneyCost()
    {
        if (ClickBtn.IsDefender)
        {
            if (pDMoney >= ClickBtn.Price && ClickBtn.NumLeft>0)
            {
                pDMoney -= ClickBtn.Price;
                ClickBtn.NumLeft -= 1;
                Hover.Instance.Deactivate();
            }
        }
        else
        {
            if (pAMoney >= ClickBtn.Price && ClickBtn.NumLeft>0)
            {
                pAMoney -= ClickBtn.Price;
                ClickBtn.NumLeft -= 1;
                Hover.Instance.Deactivate();
            }
        }
        
    }

    public void MoveCost(bool isd)
    {
        if(isd)
        {
            pDMoney -= 1;
            Hover.Instance.Deactivate();
        }
        else
        {
            pAMoney -= 1;
            Hover.Instance.Deactivate();
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
