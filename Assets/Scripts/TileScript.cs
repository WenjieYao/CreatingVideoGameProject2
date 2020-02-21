using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/******************************************************/
/*** Each tile is associated with a TileScript ********/
/******************************************************/

public class TileScript : Singleton<TileScript>
{
    /******************************************************/
    /******** GameManager Class Component *****************/
    /******************************************************/
    
    // The position of the current tile
    public Point GridPosition { get; private set; }

    // Check if the current tile is empty
    public bool IsEmpty { get; set; }

    // Static variables used for moving an unit
    /******************************************************/
    // Check if the moving is started
    public static bool StartMoving = false;
    // Moving unit's health
    public static int mhealth = 0;
    // Movement distance
    public static int mv_dis = 0;
    // Is defender
    public static bool isd = true;
    // Unit prefab type associated with current tile
    // {0,1,2,3} = {Tower, Soldier1,2,3}
    public static int pfType = 0;
    // Starting point (anchor point)

    public static Point StartingPoint = LevelManager.CenterPos;
    /******************************************************/

    // Red color indicates an alert
    private Color32 fullColor = new Color32(255, 118, 118, 225);

    // Green color indicates an allowed position
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    // Current Spirte Renderer
    private SpriteRenderer spriteRenderer = null;
    
    // Transform the tile position to postion on screen
    public Vector2 WorldPosition
	{
        get
		{
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
		}
	}

    /******************************************************/
    /******** GameManager Basic Functions *****************/
    /******************************************************/

    // Start is called before the first frame update
    void Start()
    {
        // Get the current sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /******************************************************/
    /**** TileScript Manual Function Controls *************/
    /******************************************************/

    // Set values
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        // The center is by default the base so should not be empty
        if (gridPos == LevelManager.CenterPos)
        {
            IsEmpty = false;
        }
        else
        {
            IsEmpty = true;
        }
        
        this.GridPosition = gridPos;
        transform.position = worldPos;
        // Set all tiles as the child of the parent (map)
        transform.SetParent(parent);
        // Initiliaze the dictionary in LevelManager
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    // Actions when the mouse cursor is over current tile
    private void OnMouseOver()
	{
        // Spawn and place a unit on the tile
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null && !StartMoving)
        {
            // Can place if the tile is empty and the defender can only 
            // place inside while attacker can only place outside
            if (IsEmpty && IsInner()==GameManager.Instance.ClickedBtn.IsDefender)
            {
                // Green color indicates an allowed position
                ColorTile(emptyColor);
                // Place unit with left mouse click
                if (Input.GetMouseButtonDown(0))
		        {
                    PlaceUnit();
		        }
            }
            else
            {   // Red color indicates a not-allowed position
                ColorTile(fullColor);
            }
        }

        /**********************************************************************************/
        // ************  Moving a existing unit or completely delete it ********************
        /**********************************************************************************/

        if (!IsEmpty && this.GridPosition != LevelManager.CenterPos)
        {
            // Start moving the unit with right mouse
            // Check if the unit can be moved
            bool Movable = false;
            // The unit must have a movement distance > 0 and has not been moved in the current round
            if (transform.GetChild(0).gameObject.GetComponent<UnitProperty>().Movement>0 && !transform.GetChild(0).gameObject.GetComponent<UnitProperty>().IsMoved)
            {
                if (transform.GetChild(0).gameObject.GetComponent<UnitProperty>().IsDefender && GameManager.Instance.isdRound)
                {
                    // Defender can move if there's still money left
                    Movable = GameManager.Instance.pDMoney > 0;
                }
                else if (!transform.GetChild(0).gameObject.GetComponent<UnitProperty>().IsDefender && !GameManager.Instance.isdRound)
                {
                    // Attacker can move if there's still money left
                    Movable = GameManager.Instance.pAMoney > 0;
                }
            }
            // Move the unit with right mouse drag
            if(Input.GetMouseButtonDown(1) && Movable)
            {
                // Get the unit as the child of the tile
                GameObject obj = transform.GetChild(0).gameObject;
                // Initiate a hover using the sprite of current unit
                Hover.Instance.Activate(obj.GetComponent<UnitProperty>().Nsprite);
                // Start moving
                StartMoving = true;
                // Starting point set as the current position
                StartingPoint = this.GridPosition;
                // Set unit property values
                mhealth = obj.GetComponent<UnitProperty>().Health;
                isd = obj.GetComponent<UnitProperty>().IsDefender;
                mv_dis = obj.GetComponent<UnitProperty>().Movement;
                pfType = obj.GetComponent<UnitProperty>().PrefabType;
                // Remove the current unit
                Destroy(obj);
                IsEmpty = true;
            }
            ///////****Optional *****//////
            // Completely delete the placed object with space bar and left mouse
            if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Space))
            {
                Destroy(transform.GetChild(0).gameObject);
                IsEmpty = true;
            }
        }
        // Moving process
        // Coloring the allowed position
        if (IsEmpty && StartMoving && InRange(StartingPoint,mv_dis))
        {
            ColorTile(emptyColor);
        }
        else if (StartMoving)
        {
            ColorTile(fullColor);
        }
        // Place the unit to a new location
        if (IsEmpty && StartMoving && Input.GetMouseButtonUp(1) && InRange(StartingPoint,mv_dis))
        {
            // Move the object
            MoveUnit(pfType, isd);
            // Reset static values
            pfType = 0;
            isd = true;
            mv_dis = 0;
            StartMoving = false;
        }
        /**********************************************************************************/
        
	}

    // Actions when mouse cursor moves out of the tile
    private void OnMouseExit()
    {
        // Reset the color
        ColorTile(Color.white);
    }

    // Place a unit
    private void PlaceUnit()
	{
        // Create a unit object with the corresponding prefab
        GameObject unit = (GameObject)Instantiate(SpawnList.Instance.SPList[GameManager.Instance.ClickedBtn.PrefabType], transform.position, Quaternion.identity);
        // Unit sorting order so that they have the right overlap 
        unit.GetComponent<SpriteRenderer>().sortingOrder = 2*GridPosition.Y+1;
        // Set the new unit as a child of the current tile
        unit.transform.SetParent(transform);

        // Label as moved unit
        unit.GetComponent<UnitProperty>().IsMoved = true;

        // Create Health bar using unit's health
        GameObject HealthBar = (GameObject)Instantiate(SpawnList.Instance.SPList[unit.GetComponent<UnitProperty>().Health+3], transform.position, Quaternion.identity);
        HealthBar.GetComponent<SpriteRenderer>().sortingOrder = 2*GridPosition.Y+2;;
        HealthBar.transform.SetParent(unit.transform);
        HealthBar.transform.position = new Vector3(HealthBar.transform.position.x+0.52F,HealthBar.transform.position.y+0.2F,HealthBar.transform.position.z);

        // Tile full
        IsEmpty = false;
        // Reset color after unit placed
        ColorTile(Color.white);
        // Compute the spawn cost
        GameManager.Instance.SpawnCost();
        
	}

    // Move a unit
    private void MoveUnit(int pfType, bool isd)
	{
        // Create a unit object with the corresponding prefab
        GameObject unit = (GameObject)Instantiate(SpawnList.Instance.SPList[pfType], transform.position, Quaternion.identity);
        // Unit sorting order so that they have the right overlap 
        unit.GetComponent<SpriteRenderer>().sortingOrder = 2*GridPosition.Y+1;
        // Set the new unit as a child of the current tile
        unit.transform.SetParent(transform);

        // Label as moved unit if location changed
        unit.GetComponent<UnitProperty>().IsMoved = (this.GridPosition != StartingPoint);
        // Set health
        unit.GetComponent<UnitProperty>().Health = mhealth;

        // Create Health bar using unit's health
        GameObject HealthBar = (GameObject)Instantiate(SpawnList.Instance.SPList[unit.GetComponent<UnitProperty>().Health+3], transform.position, Quaternion.identity);
        HealthBar.GetComponent<SpriteRenderer>().sortingOrder = 2*GridPosition.Y+2;
        HealthBar.transform.SetParent(unit.transform);
        HealthBar.transform.position = new Vector3(HealthBar.transform.position.x+0.52F,HealthBar.transform.position.y+0.2F,HealthBar.transform.position.z);

        // Tile full
        IsEmpty = false;
        // Reset color after unit placed
        ColorTile(Color.white);
        
        // Check if position changed and compute the move cost
        GameManager.Instance.MoveCost(isd,(this.GridPosition != StartingPoint));
        
	}

    // Color the current tile
    private void ColorTile(Color32 newColor)
    {
        spriteRenderer.color = newColor;
    }

    // Check if the current position is inside the ground
    private bool IsInner()
    {
        return Mathf.Abs(this.GridPosition.X - LevelManager.CenterPos.X)<=(LevelManager.InnerX/2) && Mathf.Abs(this.GridPosition.Y - LevelManager.CenterPos.Y)<=(LevelManager.InnerY/2);
    }

    // Check if the current position can be reached with the movement distance
    private bool InRange(Point a, int mv_dis)
    {
        return (Mathf.Abs(this.GridPosition.X-a.X) + Mathf.Abs(this.GridPosition.Y-a.Y)) <= mv_dis;
    }
}
