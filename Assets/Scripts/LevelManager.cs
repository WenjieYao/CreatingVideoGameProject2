using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/******************************************************/
/*** Script for initialzing the map and base **********/
/******************************************************/

public class LevelManager : Singleton<LevelManager>
{
    /*****Public static properties of the map*************/
    // Size of the map
    public static int mapXSize = 0;
    public static int mapYSize = 0;
    // Center position of the map
    public static Point CenterPos = new Point(0,0);
    // The inner square size that seperates defender and attacker
    public static int InnerX = 7;
    public static int InnerY = 7;
    /******************************************************/
    /******** LevelManager Class Component ****************/
    /******************************************************/

    // Private serilize field that can be controlled
    // using the LevelManager Object in Unity
    /******************************************************/
    // List of the different kinds of tiles
    [SerializeField]
    private GameObject[] tilePrefabs = null;

    // CameraMovement controlled, unused
    [SerializeField]
    private CameraMovement cameraMovement = null;

    // Map object that is the parent of all tiles
    // so that the hierarchy menu is nice and neat
    [SerializeField]
    private Transform map = null;

    // Base prefab
    [SerializeField]
    private GameObject BasePrefab = null;
    /******************************************************/
    // Dictionary mapping the tile script to the position
    public Dictionary<Point, TileScript> Tiles { get; set; }

    //A property for the size of the square tile
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    /******************************************************/
    /******** LevelManager Basic Functions ****************/
    /******************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialze map upon start
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    /******************************************************/
    /**** LevelManager Manual Function Controls ***********/
    /******************************************************/

    // Initialze a new map
    private void CreateLevel()
    {
        // Create a tile dictionary called Tiles
        Tiles = new Dictionary<Point, TileScript>();

        //read map form level text
        string[] mapData = ReadLevelText();
        mapXSize = mapData[0].ToCharArray().Length;
        mapYSize = mapData.Length;

        // Assign the center postion
        CenterPos.X = (mapXSize-1)/2;
        CenterPos.Y = (mapYSize-1)/2;

        // Maxtile position
        Vector3 maxTile = Vector3.zero;
        // Starting point of the tile in the screen
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/32*7, Screen.height));
        // Creat tiles using loop
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

        // Create the base
        SpawnBase();
    }
    
    // Function that can place a tile on screen
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        //Parses the tile type to an int
        int tileIndex = int.Parse(tileType);

        //Creates a new tile and makes a reference to that tile in the newTile variable
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Uses the new tile variable to change the position of the tile
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0),map);
    }

    // Function that read the level map from "Level.txt"
    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string tmpData = bindData.text.Replace(Environment.NewLine, string.Empty);
        return tmpData.Split('-');
    }

    // Function that is used to create the base
    private void SpawnBase()
	{
        // Instantiate base
        GameObject Base = (GameObject)Instantiate(BasePrefab, Tiles[CenterPos].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        Base.transform.SetParent(Tiles[CenterPos].transform);   
    }
}
