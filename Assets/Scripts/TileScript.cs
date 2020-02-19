using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

    public bool IsEmpty { get; private set; }

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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Space) && IsMouseOver() && !IsEmpty)
        {
            IsEmpty = true;
        }
    }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }
    private void OnMouseOver()
	{
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickBtn != null)
        {
            if (IsEmpty)
            {
                ColorTile(emptyColor);
                if (Input.GetMouseButtonDown(0))
		        {
                    PlaceTower();
		        }
            }
            else if (!IsEmpty)
            {
                ColorTile(fullColor);
            }
        }
        
	}

    private void OnMouseExit()
    {
        ColorTile(Color.white);
    }
    private void PlaceTower()
	{
        GameObject tower = (GameObject)Instantiate(GameManager.Instance.ClickBtn.SpawnPrefab, transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y+1;
        tower.transform.SetParent(transform);

        IsEmpty = false;
        ColorTile(Color.white);

        GameManager.Instance.MoneyCost();
        
	}

    private void ColorTile(Color32 newColor)
    {
        spriteRenderer.color = newColor;
    }
}
