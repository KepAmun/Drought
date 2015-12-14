using UnityEngine;
using System.Collections;

public abstract class TileContent : MonoBehaviour
{
    public TerrainTile ContainingTile { get; set; }

    public int Health { get; set; }
    public int MaxHealth { get; protected set; }
    public abstract int Level { get; set; }

    public virtual void Advance()
    {

    }


    public virtual void CheckHealth()
    {

    }
}
