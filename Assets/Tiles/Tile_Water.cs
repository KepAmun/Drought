using UnityEngine;
using System.Collections.Generic;

public class Tile_Water : TerrainTile
{
    protected override void Awake()
    {
        base.Awake();

        Type = TileType.Water;

        Health = 100;
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
