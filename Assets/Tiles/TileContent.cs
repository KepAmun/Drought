using UnityEngine;
using System.Collections;

public class TileContent : MonoBehaviour
{
    public TerrainTile ContainingTile { get; set; }

    public virtual void Advance()
    {

    }


    public virtual void CheckHealth()
    {

    }
}
