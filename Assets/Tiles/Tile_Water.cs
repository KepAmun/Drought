using UnityEngine;
using System.Collections.Generic;

public class Tile_Water : Tile
{
    protected override void Awake()
    {
        base.Awake();

        Type = TileType.Water;

        Health = 16;
    }


    public override void Advance()
    {
        base.Advance();

        List<Tile> neighbors = GameBoard.GetNeighbors(this);

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Desert || neighbors[i].Type == TileType.Mud)
            {
                neighbors[i].Health++;
                Health--;
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
            ChangeTo(TileType.Grass);
        }
    }
}
