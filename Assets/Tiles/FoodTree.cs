using UnityEngine;
using System.Collections;


public class FoodTree : TileContent
{
    GameObject[] _levelMeshes;

    protected override void Awake()
    {
        base.Awake();

        Growth.MaxHealth = 11;
        Growth.Level = 0;

        transform.Rotate(Vector3.up, Random.Range(0, 360.0f));
        _levelMeshes = new GameObject[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            _levelMeshes[i] = transform.GetChild(i).gameObject;
        }

    }


    protected override void Start()
    {
        base.Start();
    }


    public override void Advance()
    {
        base.Advance();

        if(ContainingTile.Type == Tile.TileType.Ground)
        {
            // Trees on desert lose 1 health.
            // Trees on mud stay the same.
            // Trees on grass gain 1 health.
            Growth.Health += ContainingTile.Growth.Level - 1;
        }
    }


    public override void CheckHealth()
    {
        base.CheckHealth();

        if(Growth.Health < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            RefreshLeaves();
        }

    }


    void RefreshLeaves()
    {
        for(int i = 0; i < 4; i++)
        {
            _levelMeshes[i].SetActive(i <= Growth.Level);
        }
    }
}
