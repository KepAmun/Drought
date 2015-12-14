using UnityEngine;
using System.Collections;

public class Tree : TileContent
{
    public int Health { get; set; }

    void Awake()
    {
        Health = 2;
    }


    public override void Advance()
    {
        base.Advance();

        if(ContainingTile.Type == Tile.TileType.Ground)
        {
            Health--;
        }
    }
}
