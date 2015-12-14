using UnityEngine;
using System.Collections.Generic;

public class GroundTile : TerrainTile
{
    protected override void Awake()
    {
        base.Awake();

        Type = TileType.Ground;
    }

    public override void Advance()
    {
        base.Advance();

        Health--;
    }

    public override void CheckHealth()
    {
        base.CheckHealth();

        Level = Mathf.Clamp(Health / 2, 0, 2);
    }
}
