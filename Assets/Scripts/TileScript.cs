using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : Singleton<TileScript>
{
    public Point GridPosition { get; private set; }

    public bool IsEmpty { get; private set; }

    // moving variables
    public static bool StartMoving = false;
    public static int mv_dis = 0;
    public static bool isd = true;
    public static int prefabType = 0;

    public static Point StartingPoint = LevelManager.CenterPos;
    private Color32 fullColor = new Color32(255, 118, 118, 225);

    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer = null;
    public Vector2 WorldPosition
	{
        get
		{
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2));
		}
	}


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
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
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    private void OnMouseOver()
	{
        // placing towers from the panel
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickBtn != null && !StartMoving)
        {
            if (IsEmpty && IsInner()==GameManager.Instance.ClickBtn.IsDefender)
            {
                ColorTile(emptyColor);
                if (Input.GetMouseButtonDown(0))
		        {
                    PlaceTower();
		        }
            }
            else
            {
                ColorTile(fullColor);
            }
        }

        // ************  moving a existing object or completely delete it ********************
        if (!IsEmpty && this.GridPosition != LevelManager.CenterPos)
        {
            // start moving the object with shift key
            bool Movable = false;
            if (transform.GetChild(0).gameObject.GetComponent<ObjectFun>().Movement>0)
            {
                if (transform.GetChild(0).gameObject.GetComponent<ObjectFun>().IsDefender)
                {
                    Movable = GameManager.Instance.pDMoney > 0;
                }
                else
                {
                    Movable = GameManager.Instance.pAMoney > 0;
                }
            }
            if(Input.GetMouseButtonDown(1) && Movable)
            {
                GameObject obj = transform.GetChild(0).gameObject;
                Hover.Instance.Activate(obj.GetComponent<ObjectFun>().Nsprite);
                StartMoving = true;
                StartingPoint = this.GridPosition;
                isd = obj.GetComponent<ObjectFun>().IsDefender;
                mv_dis = obj.GetComponent<ObjectFun>().Movement;
                prefabType = obj.GetComponent<ObjectFun>().PrefabType;
                Destroy(obj);
                IsEmpty = true;
            }
            ///////****optional *****//////
            // completely delete the placed object with space bar and right mouse
            if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Space))
            {
                Destroy(transform.GetChild(0).gameObject);
                IsEmpty = true;
            }
        }
        if (IsEmpty && StartMoving && InRange(StartingPoint,mv_dis))
        {
            //Debug.Log("Move");
            ColorTile(emptyColor);
        }
        else if (StartMoving)
        {
            ColorTile(fullColor);
        }

        if (IsEmpty && StartMoving && Input.GetMouseButtonUp(1) && InRange(StartingPoint,mv_dis))
        {
            MoveObj(prefabType, isd);
            prefabType = 0;
            isd = true;
            mv_dis = 0;
            StartMoving = false;
        }
        
        
	}

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }
    private void PlaceTower()
	{
        GameObject tower = (GameObject)Instantiate(PrefabList.Instance.PfList[GameManager.Instance.ClickBtn.SpawnPrefab], transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y+1;
        tower.transform.SetParent(transform);

        IsEmpty = false;
        ColorTile(Color.white);

        GameManager.Instance.MoneyCost();
        
	}

    private void MoveObj(int prefabType, bool isd)
	{
        GameObject obj = (GameObject)Instantiate(PrefabList.Instance.PfList[prefabType], transform.position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y+1;
        obj.transform.SetParent(transform);

        IsEmpty = false;
        ColorTile(Color.white);

        GameManager.Instance.MoveCost(isd);
        
	}

    private void ColorTile(Color32 newColor)
    {
        spriteRenderer.color = newColor;
    }

    private bool IsInner()
    {
        return Mathf.Abs(this.GridPosition.X - LevelManager.CenterPos.X)<=(LevelManager.InnerX/2) && Mathf.Abs(this.GridPosition.Y - LevelManager.CenterPos.Y)<=(LevelManager.InnerY/2);
    }

    private bool InRange(Point a, int mv_dis)
    {
        return (Mathf.Abs(this.GridPosition.X-a.X) + Mathf.Abs(this.GridPosition.Y-a.Y)) <= mv_dis;
    }
}
