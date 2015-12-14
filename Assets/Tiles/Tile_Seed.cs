using UnityEngine;
using System.Collections;

public class Tile_Seed : Tile
{
    public GameObject TreePrefab;

    public override bool Activate()
    {
        bool success = false;

        TerrainTile targetTile = GameBoard.GetTileAt(transform.position);
        if(targetTile != null && targetTile.Type == TileType.Ground)
        {
            Tree tree = Instantiate<GameObject>(TreePrefab).GetComponent<Tree>();
            tree.transform.SetParent(targetTile.transform, false);
            targetTile.Content = tree;

            success = base.Activate();
        }

        return success;
    }

}
