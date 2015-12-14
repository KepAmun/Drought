using UnityEngine;
using System.Collections.Generic;

public class Tile_Water : TerrainTile
{
    protected override void Awake()
    {
        base.Awake();

        Type = TileType.Water;

        Health = 32;
    }


    public override void Advance()
    {
        base.Advance();

        List<TerrainTile> neighbors = GameBoard.GetNeighbors(this);

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Ground)
            {
                neighbors[i].Health += 2;
                Health -= 2;
            }
        }
    }


    public override bool Activate()
    {
        bool success = false;

        Tile targetTile = GameBoard.GetTileAt(transform.position);
        if(targetTile != null && targetTile.Type != TileType.Water)
        {
            success = base.Activate();
        }

        return success;
    }


    public override void CheckHealth()
    {
        base.CheckHealth();

        if(Health < 0)
        {
            TerrainTile tile = ChangeTo(TileType.Ground);
            tile.Health = 6;
            tile.CheckHealth();
        }
    }
}
