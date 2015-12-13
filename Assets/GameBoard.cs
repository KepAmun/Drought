using UnityEngine;
using System.Collections;
using System;

public class GameBoard : MonoBehaviour
{
    public struct Coords { public int x, y; }

    public const int BOARD_WIDTH = 6;
    public const int BOARD_HEIGHT = 6;

    public GameObject DesertPrefab;
    public GameObject WaterPrefab;
    public GameObject MudPrefab;
    public GameObject GrassPrefab;

    Tile[,] _map = new Tile[BOARD_WIDTH, BOARD_HEIGHT];

    void Awake()
    {
        for(int y = 0; y < BOARD_HEIGHT; y++)
        {
            for(int x = 0; x < BOARD_WIDTH; x++)
            {
                GameObject tileHost = Instantiate<GameObject>(DesertPrefab);
                PlaceTile(tileHost.GetComponent<Tile>(), new Coords() { x = x, y = y });

            }
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

        if(coords.x >= 0 && coords.x < BOARD_WIDTH && coords.y >= 0 && coords.y < BOARD_HEIGHT)
        {
            if(_map[coords.x, coords.y] != null)
                Destroy(_map[coords.x, coords.y].gameObject);

            _map[coords.x, coords.y] = tile;
            tile.transform.SetParent(transform);
            tile.transform.position = CoordsToPosition(coords);
            tile.GameBoard = this;
            success = true;
        }

        return success;
    }


    Vector3 CoordsToPosition(Coords coords)
    {
        return new Vector3(coords.x - 2.5f, 0, coords.y - 2.5f);
    }


    Coords PositionToCoords(Vector3 position)
    {
        return new Coords { x = Mathf.RoundToInt(position.x + 2.5f), y = Mathf.RoundToInt(position.z + 2.5f) };
    }


    public void Advance()
    {
        foreach(Tile tile in _map)
        {
            tile.Advance();
        }
    }
}
