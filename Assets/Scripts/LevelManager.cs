using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs = null;//tile kinds

    [SerializeField]
    private CameraMovement cameraMovement = null;

    [SerializeField]
    private Transform map = null;

    private Point CoinSpawn;

    [SerializeField]
    private GameObject CoinPrefab = null;


    public Dictionary<Point, TileScript> Tiles { get; set; }

    //A property for the size of the square tile
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }


    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public static Point CenterPos = new Point(0,0);
    public static int InnerX = 7;
    public static int InnerY = 7;
    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        //read map form level text
        string[] mapData = ReadLevelText();
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;
        CenterPos.X = (mapXSize-1)/2;
        CenterPos.Y = (mapYSize-1)/2;

        Vector3 maxTile = Vector3.zero;

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/32*7, Screen.height));
        for (int y =0 ; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(),x,y,worldStart);
            }
        }

        maxTile = Tiles[new Point(mapXSize - 1, mapYSize - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x+TileSize, maxTile.y - TileSize));

        SpawnPortal();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        //Parses the tile type to an int
        int tileIndex = int.Parse(tileType);

        //Creates a new tile and makes a reference to that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Uses the new tile variable to change the position of the tile

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0),map);

        
        
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string tmpData = bindData.text.Replace(Environment.NewLine, string.Empty);
        return tmpData.Split('-');
    }

    private void SpawnPortal()
	{
        CoinSpawn = CenterPos;

        Instantiate(CoinPrefab, Tiles[CoinSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

        //CoinSpawn2 = new Point(0, 0);

        //Instantiate(Coin2Prefab, Tiles[CoinSpawn2].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
    }
}
