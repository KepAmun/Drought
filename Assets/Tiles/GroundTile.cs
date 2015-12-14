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

        List<TerrainTile> neighbors = GameBoard.GetNeighbors(this);

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Water && neighbors[i].Health > Health)
            {
                int healthDrain = Mathf.Min(neighbors[i].Health, 4);

                neighbors[i].Health -= healthDrain;
                Health += healthDrain;
            }
        }
    }

    public override void CheckHealth()
    {
        base.CheckHealth();

        Level = Mathf.Clamp(Health / 2, 0, 2);
    }
}
