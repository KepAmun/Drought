using UnityEngine;
using System.Collections.Generic;

public class GroundTile : TerrainTile
{
    GameObject[] _levelModels;

    protected override void Awake()
    {
        base.Awake();
        
        Growth.MaxHealth = 8;

        Type = TileType.Ground;

        int maxLevel = 3;
        _levelModels = new GameObject[maxLevel];
        for(int i = 0; i < maxLevel; i++)
        {
            _levelModels[i] = transform.GetChild(i).gameObject;
            _levelModels[i].SetActive(false);
        }
    }


    protected override void Start()
    {
        base.Start();
    }


    public override void Advance()
    {
        List<TerrainTile> neighbors = GameBoard.GetNeighbors(this);

        bool waterFound = false;

        for(int i = 0; i < neighbors.Count; i++)
        {
            if(neighbors[i].Type == TileType.Water)
            {
                waterFound = true;

                break;
            }
        }
        
        if(waterFound)
        {
            Growth.Health++;
        }
        else
        {
            Growth.Health--;
        }

        base.Advance();
    }


    public override void CheckHealth()
    {
        base.CheckHealth();

        for(int i = 0; i < _levelModels.Length; i++)
        {
            _levelModels[i].SetActive(i == Growth.Level);
        }
    }
}
