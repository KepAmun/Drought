using UnityEngine;
using System.Collections.Generic;

public class Tile_Sun : Tile
{
    public override bool Activate()
    {
        bool success = false;

        TerrainTile targetTile = GameBoard.GetTileAt(transform.position);
        if(targetTile != null && targetTile.Type == TileType.Ground)
        {
            targetTile.GameBoard.GetNeighbors(targetTile);

            List<TerrainTile> targets = GameBoard.Instance.GetNeighbors(targetTile);
            targets.Add(targetTile);

            foreach(var target in targets)
            {
                if(target.Type == TileType.Ground)
                {
                    target.Growth.Level++;

                    if(target.Content != null)
                    {
                        target.Content.Growth.Level++;
                    }
                }
                else if(target.Type == TileType.Water)
                {
                    target.Growth.Level--;
                }
            }

            success = base.Activate();
        }

        return success;
    }

}
