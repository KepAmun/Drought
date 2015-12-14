using UnityEngine;
using System.Collections;

public abstract class TileContent : MonoBehaviour
{
    public TerrainTile ContainingTile { get; set; }
    public GrowthHandler Growth;


    protected virtual void Awake()
    {
        Growth = GetComponent<GrowthHandler>();
    }


    protected virtual void Start()
    {
        CheckHealth();
    }


    public virtual void Advance()
    {

    }


    public virtual void CheckHealth()
    {

    }
}
