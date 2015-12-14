using UnityEngine;
using System.Collections;

public class Tree : TileContent
{
    GameObject[] _leaves;

    void Awake()
    {
        Health = 3;

        transform.Rotate(Vector3.up, Random.Range(0, 360.0f));
        _leaves = new GameObject[3];

        for(int i = 0; i < 3; i++)
        {
            _leaves[i] = transform.GetChild(i).gameObject;
        }
    }


    void Start()
    {
        CheckHealth();
    }


    public override void Advance()
    {
        base.Advance();
        Health--;

        if(ContainingTile.Type == Tile.TileType.Ground && ContainingTile.Health > 0)
        {
            int healthDrain = Mathf.Min(ContainingTile.Health, 2);

            ContainingTile.Health -= healthDrain;
            Health += healthDrain;
        }
    }


    public override void CheckHealth()
    {
        base.CheckHealth();

        int level = Mathf.Clamp(Health / 4, 0, 4);

        for(int i = 0; i < 3; i++)
        {
            _leaves[i].SetActive(i+1 <= level);
        }
    }
}
