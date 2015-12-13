using UnityEngine;
using System.Collections.Generic;

public class Tile_Desert : Tile
{
    protected override void Awake()
    {
        base.Awake();

        Type = TileType.Desert;
    }


    public override void CheckHealth()
    {
        base.CheckHealth();

        if(Health > 0)
        {
            ChangeTo(TileType.Mud);
        }
    }


    public override void LevelUp()
    {
        base.LevelUp();
    }


    public override void LevelDown()
    {
        base.LevelDown();
    }
}
