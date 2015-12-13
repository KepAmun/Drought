using UnityEngine;
using System.Collections.Generic;

public class Tile_Grass : Tile
{
    void Awake()
    {
        Type = TileType.Grass;
    }

    public override void Advance()
    {
        base.Advance();
    }
}
