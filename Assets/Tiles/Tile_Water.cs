using UnityEngine;
using System.Collections.Generic;

public class Tile_Water : TerrainTile
{
    Material _waterMaterial;
    Color _waterFullColor;
    Color _waterEmptyColor;

    protected override void Awake()
    {
        base.Awake();

        Growth.MaxHealth = 11;
        Growth.Health = 11;

        Type = TileType.Water;
        
        MeshRenderer renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        
        _waterMaterial = renderer.materials[1];
        _waterFullColor = _waterMaterial.color;
        _waterEmptyColor = Color.Lerp(_waterFullColor, renderer.materials[0].color, 0.7f);
    }


    protected override void Start()
    {
        base.Start();
    }


    public override bool Activate()
    {
        bool success = false;

        Tile targetTile = GameBoard.GetTileAt(transform.position);
        if(targetTile != null)
        {
            success = base.Activate();
        }

        return success;
    }


    public override void Advance()
    {
        base.Advance();

        Growth.Health--;
    }


    public override void CheckHealth()
    {
        base.CheckHealth();

        if(Growth.Health <= 0)
        {
            TerrainTile tile = ChangeTo(TileType.Ground);
            tile.Growth.Level = 2;
            tile.CheckHealth();
        }
        else
        {
            UpdateWaterColor();
        }

    }


    void UpdateWaterColor()
    {
        _waterMaterial.color = Color.Lerp(_waterEmptyColor, _waterFullColor, Growth.Health / (float)Growth.MaxHealth);
    }
}
