﻿using UnityEngine;
using System.Collections.Generic;

public class GameBoard : MonoBehaviour
{
    public struct Coords { public int x, y; }

    public const int BOARD_WIDTH = 6;
    public const int BOARD_HEIGHT = 6;

    public GameObject GroundPrefab;
    public GameObject WaterPrefab;
    public GameObject SeedPrefab;
    public GameObject SunPrefab;

    TerrainTile[,] _map = new TerrainTile[BOARD_WIDTH, BOARD_HEIGHT];

    public static GameBoard Instance{ get; private set; }


    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        for(int y = 0; y < BOARD_HEIGHT; y++)
        {
            for(int x = 0; x < BOARD_WIDTH; x++)
            {
                TerrainTile tile;
                if(Random.value > 0.9f)
                {
                    tile = MakeTile(Tile.TileType.Water) as TerrainTile;
                }
                else
                {
                    tile = MakeTile(Tile.TileType.Ground) as TerrainTile;
                    tile.Growth.Level = Random.Range(0, 3);
                    tile.CheckHealth();
                }

                tile.gameObject.SetActive(false);

                Coords targetCoords = new Coords() { x = x, y = y };
                Vector3 targetPosition = CoordsToPosition(targetCoords);
                tile.transform.position = targetPosition + Vector3.up * 10;

                StartCoroutine(PlaceTileDelayed(tile.GetComponent<Tile>(), targetCoords, Random.Range(0, 1.0f)));
            }
        }
    }


    public void Restart()
    {
        foreach(Tile tile in _map)
        {
            tile.Remove();
        }

        Start();
    }


    public void PlaceTile(Tile tile, Tile replaceTile)
    {
        PlaceTile(tile, PositionToCoords(replaceTile.transform.position));
    }


    public void PlaceTile(Tile.TileType tileType, Coords coords)
    {
        Tile tile = MakeTile(tileType);

        if(tile != null)
        {
            PlaceTile(tile, coords);
        }
    }


    public bool PlaceTile(Tile tile)
    {
        Coords coords = PositionToCoords(tile.transform.position);

        return PlaceTile(tile, coords);
    }


    public bool PlaceTile(Tile tile, Coords coords)
    {
        bool success = false;

        if(InRange(coords))
        {
            success = true;

            if(tile is TerrainTile)
            {
                TerrainTile terrainTile = tile as TerrainTile;

                if(_map[coords.x, coords.y] != null)
                    _map[coords.x, coords.y].Remove();

                _map[coords.x, coords.y] = terrainTile;
                tile.transform.SetParent(transform);
                tile.MoveTo(CoordsToPosition(coords));

                tile.name = tile.name + "(" + coords.x + ", " + coords.y + ")";
                tile.Locked = true;
            }
            else if(!(tile is Tile_Harvest))
            {
                tile.Remove();
            }

        }

        return success;
    }


    public Vector3 CoordsToPosition(Coords coords)
    {
        return new Vector3(coords.x - 2.5f, 0, coords.y - 2.5f);
    }


    public Coords PositionToCoords(Vector3 position)
    {
        return new Coords { x = Mathf.RoundToInt(position.x + 2.5f), y = Mathf.RoundToInt(position.z + 2.5f) };
    }


    public void Advance()
    {
        foreach(TerrainTile tile in _map)
        {
            tile.Advance();
        }
        
        foreach(TerrainTile tile in _map)
        {
            tile.CheckHealth();
        }
    }


    System.Collections.IEnumerator PlaceTileDelayed(Tile tile, Coords coords, float delay)
    {
        yield return new WaitForSeconds(delay);
        tile.gameObject.SetActive(true);
        PlaceTile(tile, coords);
    }


    public List<TerrainTile> GetNeighbors(Tile tile)
    {
        List<TerrainTile> neighbors = new List<TerrainTile>();

        Coords center = PositionToCoords(tile.transform.position);

        Coords[] sides = new Coords[4];
        sides[0] = new Coords() { x = -1, y = 0 };
        sides[1] = new Coords() { x = 0, y = 1 };
        sides[2] = new Coords() { x = 1, y = 0 };
        sides[3] = new Coords() { x = 0, y = -1 };

        for(int i = 0; i < 4; i++)
        {
            Coords coords = new Coords() { x = center.x + sides[i].x, y = center.y + sides[i].y };

            if(InRange(coords))
            {
                neighbors.Add(_map[coords.x, coords.y]);
            }
        }


        return neighbors;
    }
    

    public Tile MakeTile(Tile.TileType tileType)
    {
        Tile tile = null;
        GameObject tileHost = null;

        switch(tileType)
        {
            case Tile.TileType.Ground:
                tileHost = Instantiate<GameObject>(GroundPrefab);
                break;
            case Tile.TileType.Water:
                tileHost = Instantiate<GameObject>(WaterPrefab);
                break;
            case Tile.TileType.Seed:
                tileHost = Instantiate<GameObject>(SeedPrefab);
                break;
            case Tile.TileType.Sun:
                tileHost = Instantiate<GameObject>(SunPrefab);
                break;
            case Tile.TileType.Fish:
                //tileHost = Instantiate<GameObject>(FishPrefab);
                break;
            default:
                break;
        }


        if(tileHost != null)
        {
            tile = tileHost.GetComponent<Tile>();
            tile.GameBoard = this;
        }

        return tile;
    }


    public TerrainTile GetTileAt(Vector3 position)
    {
        return GetTileAt(PositionToCoords(position));
    }


    public TerrainTile GetTileAt(Coords coords)
    {
        TerrainTile tile = null;

        if(InRange(coords))
        {
            tile = _map[coords.x, coords.y];
        }

        return tile;
    }


    public bool InRange(Coords coords)
    {
        return (coords.x >= 0 && coords.x < BOARD_WIDTH && coords.y >= 0 && coords.y < BOARD_HEIGHT);
    }
}
