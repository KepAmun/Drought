using UnityEngine;
using System.Collections.Generic;

public class MudTile : Tile
{
    void Awake()
    {
        Type = TileType.Mud;
    }


    public override void Advance()
    {
        base.Advance();
        /*
        List<Tile> neighbors = GameBoard.GetNeighbors(this);

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Desert)
            {
                Tile mudTile = GameBoard.MakeTile(TileType.Mud);
                mudTile.transform.position = transform.position + Vector3.down;
                GameBoard.PlaceTile(mudTile, neighbors[i]);
            }
        }
        */
    }


    public override bool Activate()
    {
        bool success = false;

        //if(GameBoard.GetTileAt(transform.position).Type != TileType.Water)
        {
            success = base.Activate();
        }

        return success;
    }
}
