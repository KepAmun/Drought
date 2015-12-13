using UnityEngine;
using System.Collections.Generic;

public class Tile_Water : Tile
{
    void Awake()
    {
        Type = TileType.Water;
    }

    public override void Advance()
    {
        base.Advance();

        List<Tile> neighbors = GameBoard.GetNeighbors(this);

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Desert)
            {
                GameBoard.PlaceTile(TileType.Mud, neighbors[i]);
            }
        }
    }
}
