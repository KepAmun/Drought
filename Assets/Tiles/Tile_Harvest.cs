using UnityEngine;
using System.Collections;

public class Tile_Harvest : Tile
{
    public event System.Action<TileContent> TileContentHarvested;

    Vector3 _startingPosition;

    void Start()
    {
        _startingPosition = transform.position;
    }


    public override bool Activate()
    {
        bool success = false;

        TerrainTile targetTile = GameBoard.GetTileAt(transform.position);

        if(targetTile != null && targetTile.Type == TileType.Ground && targetTile.Content != null)
        {
            if(TileContentHarvested != null)
            {
                TileContentHarvested(targetTile.Content);
            }

            Destroy(targetTile.Content.gameObject);
            targetTile.Content = null;

            success = true;

            MoveTo(_startingPosition);
        }

        return success;
    }

}
