using UnityEngine;
using System.Collections;
using System;

public class GameBoard : MonoBehaviour
{
    public const int BOARD_WIDTH = 6;
    public const int BOARD_HEIGHT = 6;

    public GameObject TilePrefab;

    Tile[,] _map = new Tile[BOARD_WIDTH, BOARD_HEIGHT];

    void Awake()
    {
        for(int y = 0; y < BOARD_HEIGHT; y++)
        {
            for(int x = 0; x < BOARD_WIDTH; x++)
            {
                GameObject tileHost = Instantiate<GameObject>(TilePrefab);
                tileHost.transform.parent = transform;
                tileHost.transform.position = CoordsToPosition(x, y) + Vector3.forward;

            }
        }
    }


    public bool PlaceTile(Tile tile)
    {
        return PlaceTile(tile, Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y));
    }


    public bool PlaceTile(Tile tile, int x, int y)
    {
        bool success = false;

        if(x >= 0 && x < BOARD_WIDTH && y >= 0 && y < BOARD_HEIGHT && _map[x, y] == null)
        {
            _map[x, y] = tile;
            tile.transform.position = new Vector3(x, y, 0);
            tile.GameBoard = this;
            success = true;
        }

        return success;
    }


    Vector3 CoordsToPosition(int x, int y)
    {
        return new Vector2(x, y);
    }


    public void Advance()
    {
        foreach(Tile tile in _map)
        {
            tile.Advance();
        }
    }
}
