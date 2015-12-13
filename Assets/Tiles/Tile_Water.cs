﻿using UnityEngine;
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

        Vector3 mudOffset = new Vector3(0, -0.6f, 0);

        List<Tile> neighbors = GameBoard.GetNeighbors(this);

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Desert)
            {
                Tile mudTile = GameBoard.MakeTile(TileType.Mud);
                mudTile.transform.position = transform.position + mudOffset;
                GameBoard.PlaceTile(mudTile, neighbors[i]);
            }
        }
    }


    public override bool Activate()
    {
        bool success = false;

        if(GameBoard.GetTileAt(transform.position).Type != TileType.Water)
        {
            success = base.Activate();
        }

        return success;
    }
}
