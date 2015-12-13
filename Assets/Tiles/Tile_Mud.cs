using UnityEngine;
using System.Collections.Generic;

public class Tile_Mud : Tile
{
    protected override void Awake()
    {
        base.Awake();

        Health = 2;

        Type = TileType.Mud;
    }
    

    public override bool Activate()
    {
        bool success = false;

        Tile targetTile = GameBoard.GetTileAt(transform.position);
        if(targetTile.Type != TileType.Water && targetTile.Type != TileType.Mud)
        {
            success = base.Activate();
        }

        return success;
    }

    public override void Advance()
    {
        base.Advance();
        Health--;

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


    public override void CheckHealth()
    {
        base.CheckHealth();
        
        if(Health <= 0)
        {
            ChangeTo(TileType.Desert);
        }
    }


    public override void LevelUp()
    {
        base.LevelUp();
    }


    public override void LevelDown()
    {
        base.LevelDown();
    }
}
