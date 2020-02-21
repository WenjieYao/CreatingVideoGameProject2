using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/******************************************************/
/*** Script for controlling the game processes ********/
/******************************************************/

public class GameManager : Singleton<GameManager>
{
    /******************************************************/
    /*********      Game Control Variables    *************/
    /******************************************************/

    // Initial amount of money for defender
    int DM0 = 6;  
    // Initial amount of money for attacker
    int AM0 = 6;  
    // Round count for defender
    int roundD = 0;
    // Round count for attacker
    int roundA = 0;
    // Round control parameter
    int rLimit = 4;

    // Check if the current round is the defender round
    public bool isdRound = true; // Defender starts first

    /******************************************************/
    /******** GameManager Class Component *****************/
    /******************************************************/

    // Currently Clicked Button
    public SpawnBtn ClickedBtn { get; set; } 

    // Amount of money left for defender
    private int DMoney; 

    // Amount of money left for attacker
    private int AMoney; 


    // Private serilize field that can be 
    // controlled using the GamManager Object in Unity
    /******************************************************/
    // Defender money left display
    [SerializeField]
    private Text DTxt = null; 

    // Attacker money left display
    [SerializeField]
    private Text ATxt = null; 

    // Current round
    [SerializeField]
    private Text RoundTxt = null; 

    private int curRound = 1;
    public int CurRound
    {
        get
        {
            return curRound;
        }
        set
        {
            this.curRound = value;
            RoundTxt.text = "Round " + curRound.ToString();
        }
    }
    
    /******************************************************/
    // Public field that corresponds to the private field 
    // above that can be accessed by other scripts
    public int pDMoney
    {
        get
        {
            return DMoney;
        }
        set
        {
            // update value 
            this.DMoney = value;
            // update display
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
            // update value
            this.AMoney = value;
            // update display
            this.ATxt.text = "$" + value.ToString();
        }
    }

    /******************************************************/

    /******************************************************/
    /******** GameManager Basic Functions *****************/
    /******************************************************/

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the money display
        pDMoney = DM0;
        pAMoney = AM0;
    }

    // Update is called once per frame
    void Update()
    {
        // ESC key action
        HandleEscape();
    }

    /******************************************************/
    /**** GameManager Manual Function Controls ************/
    /******************************************************/

    // Function linked to the Spawn Button in Unity
    // Set value in the Tower/Solider/CaptainButton Object 
    // in unity under the Button Component at On Click()
    public void PickSpawn(SpawnBtn spawnBtn)
    {
        // Defender's round
        if (spawnBtn.IsDefender && isdRound) 
        {
            // Enough money and number of spawns left
            if (pDMoney >= spawnBtn.Price && spawnBtn.NumLeft>0){
                // Pass the clicked button to the component
                this.ClickedBtn = spawnBtn;
                // Activate the hover using the sprite
                // associated with the current button
                Hover.Instance.Activate(spawnBtn.Nsprite);
            }
        }
        // Attacker's round
        else if (!spawnBtn.IsDefender && !isdRound) 
        {
            if (pAMoney >= spawnBtn.Price && spawnBtn.NumLeft>0){
                // Pass the clicked button to the component
                this.ClickedBtn = spawnBtn;
                // Activate the hover using the sprite
                // associated with the current button
                Hover.Instance.Activate(spawnBtn.Nsprite);
            }
        }
    }

    // Function linked to the Finish Button in Unity
    // Set value in the FinishD/A Object 
    // in unity under the Button Component at On Click()
    public void FinishRound(FinishBtn finishBtn)
    {
        // Defender's round
        if (finishBtn.IsDefender && isdRound) 
        {
            // Count round
            roundD += 1;
            // Defender's round finished
            isdRound = false;
            // Execute attacks
            DefenderAttack();
            // Update all unit status on map
            UnitUpdate();
            if (roundD>rLimit)
            {
                // Fixed amount of money 2 after certain rounds
                pDMoney = 2;
            }
            else{
                // Reduce the amount of money in later rounds
                pDMoney = DM0 - roundD;
            }
        }
        // Attacker's round
        else if (!finishBtn.IsDefender && !isdRound) 
        {
            // Count round
            roundA += 1;
            CurRound = roundA + 1;
            // Attacker's round finished
            isdRound = true;
            // Execute attack
            AttackerAttack();
            // Update all unit status on map
            UnitUpdate();
            if (roundA>rLimit)
            {
                // Fixed amount of money 2 after certain rounds
                pAMoney = 2;
            }
            else{
                // Reduce the amount of money in later rounds
                pAMoney = AM0 - roundA;
            }
        }
    }
    
    // Calculate the spawn cost 
    public void SpawnCost()
    {
        if (ClickedBtn.IsDefender) // Defender or attacker
        {
            if (pDMoney >= ClickedBtn.Price && ClickedBtn.NumLeft>0)
            {
                // Update the money display
                pDMoney -= ClickedBtn.Price;
                // Update teh number of spawns left
                ClickedBtn.NumLeft -= 1;
                // Relase the hover upon prefab spawned
                Hover.Instance.Deactivate();
            }
        }
        else
        {
            if (pAMoney >= ClickedBtn.Price && ClickedBtn.NumLeft>0)
            {
                // Update the money display
                pAMoney -= ClickedBtn.Price;
                // Update teh number of spawns left
                ClickedBtn.NumLeft -= 1;
                // Relase the hover upon prefab spawned
                Hover.Instance.Deactivate();
            }
        }
        
    }

    // Calculate the cost of moving a character
    public void MoveCost(bool isd, bool PosChanged)
    {
        if(isd) // Defender or attacker
        {
            // Update money display
            if (PosChanged)
                pDMoney -= 1;
            // Relase hover upon move finished
            Hover.Instance.Deactivate();
        }
        else
        {
            // Update money display
            if (PosChanged)
                pAMoney -= 1;
            // Relase hover upon move finished
            Hover.Instance.Deactivate();
        }
    }

    // Cancel an Spawn Button Click and the hover with ESC
    private void HandleEscape()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Relase hover
            Hover.Instance.Deactivate();
        }
    }

    // Update unit status on the map
    private void UnitUpdate()
    {
        // Loop through tiles
        for (int y =0 ; y < LevelManager.mapYSize; y++)
        {
            for (int x = 0; x < LevelManager.mapXSize; x++)
            {
                Point curPos = new Point(x,y);
                // If the current tile has an unit on it
                if(!LevelManager.Instance.Tiles[curPos].IsEmpty && LevelManager.Instance.Tiles[curPos].GridPosition != LevelManager.CenterPos)
                {
                    // Check for death unit and remove it 
                    GameObject unit = LevelManager.Instance.Tiles[curPos].transform.GetChild(0).gameObject;
                    
                    if (unit.GetComponent<UnitProperty>().Health <= 0)
                    {
                        Destroy(unit);
                        LevelManager.Instance.Tiles[curPos].IsEmpty = true;
                    }
                    else
                    {
                        // Change IsMove to false
                        unit.GetComponent<UnitProperty>().IsMoved = false;
                        // Remove old health bar
                        Destroy(unit.transform.GetChild(0).gameObject);
                        // Create new Health bar using unit's health
                        GameObject HealthBar = (GameObject)Instantiate(SpawnList.Instance.SPList[unit.GetComponent<UnitProperty>().Health+3], unit.transform.position, Quaternion.identity);
                        HealthBar.GetComponent<SpriteRenderer>().sortingOrder = unit.GetComponent<SpriteRenderer>().sortingOrder + 1;
                        HealthBar.transform.SetParent(unit.transform);
                        HealthBar.transform.position = new Vector3(HealthBar.transform.position.x+0.52F,HealthBar.transform.position.y+0.2F,HealthBar.transform.position.z);
                    }
                    
                }
            }
        }
    }

    // Defender Attack
    private void DefenderAttack()
    {
        // Loop through tiles
        for (int y =0 ; y < LevelManager.mapYSize; y++)
        {
            for (int x = 0; x < LevelManager.mapXSize; x++)
            {
                Point curPos = new Point(x,y);
                // If the current tile has an unit on it
                if(!LevelManager.Instance.Tiles[curPos].IsEmpty && LevelManager.Instance.Tiles[curPos].GridPosition != LevelManager.CenterPos)
                {
                    // Get this unit and check for Attacker or Dedender
                    GameObject unit = LevelManager.Instance.Tiles[curPos].transform.GetChild(0).gameObject;
                    if (unit.GetComponent<UnitProperty>().IsDefender)
                    {
                        // If is defender, loop the surrounding tiles in attack range
                        for (int attack_x = -unit.GetComponent<UnitProperty>().AttackRange; attack_x <= unit.GetComponent<UnitProperty>().AttackRange; attack_x++ )
                        {
                            for (int attack_y = -unit.GetComponent<UnitProperty>().AttackRange; attack_y <= unit.GetComponent<UnitProperty>().AttackRange; attack_y++ )
                            {
                                if ((Mathf.Abs(attack_x)+Mathf.Abs(attack_y))<=unit.GetComponent<UnitProperty>().AttackRange && !(attack_x ==0 && attack_y==0))
                                {
                                    Point attPos = new Point(attack_x + x, attack_y + y);
                                    if(!LevelManager.Instance.Tiles[attPos].IsEmpty)
                                    {
                                        // If unit is attacker then attack
                                        if(!LevelManager.Instance.Tiles[attPos].transform.GetChild(0).gameObject.GetComponent<UnitProperty>().IsDefender)
                                        {
                                            LevelManager.Instance.Tiles[attPos].transform.GetChild(0).gameObject.GetComponent<UnitProperty>().Health -= unit.GetComponent<UnitProperty>().AttackPower;
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
        }
    }

    private void AttackerAttack()
    {
        // Loop through tiles
        for (int y =0 ; y < LevelManager.mapYSize; y++)
        {
            for (int x = 0; x < LevelManager.mapXSize; x++)
            {
                Point curPos = new Point(x,y);
                // If the current tile has an unit on it
                if(!LevelManager.Instance.Tiles[curPos].IsEmpty && LevelManager.Instance.Tiles[curPos].GridPosition != LevelManager.CenterPos)
                {
                    // Get this unit and check for Attacker or Dedender
                    GameObject unit = LevelManager.Instance.Tiles[curPos].transform.GetChild(0).gameObject;
                    if (!unit.GetComponent<UnitProperty>().IsDefender)
                    {
                        // If is attacker, loop the surrounding tiles in attack range
                        for (int attack_x = -unit.GetComponent<UnitProperty>().AttackRange; attack_x <= unit.GetComponent<UnitProperty>().AttackRange; attack_x++ )
                        {
                            for (int attack_y = -unit.GetComponent<UnitProperty>().AttackRange; attack_y <= unit.GetComponent<UnitProperty>().AttackRange; attack_y++ )
                            {
                                if ((Mathf.Abs(attack_x)+Mathf.Abs(attack_y))<=unit.GetComponent<UnitProperty>().AttackRange && !(attack_x ==0 && attack_y==0))
                                {
                                    Point attPos = new Point(attack_x + x, attack_y + y);
                                    if(!LevelManager.Instance.Tiles[attPos].IsEmpty)
                                    {
                                        // If unit is defender then attack
                                        if(LevelManager.Instance.Tiles[attPos].transform.GetChild(0).gameObject.GetComponent<UnitProperty>().IsDefender)
                                        {
                                            LevelManager.Instance.Tiles[attPos].transform.GetChild(0).gameObject.GetComponent<UnitProperty>().Health -= unit.GetComponent<UnitProperty>().AttackPower;
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
