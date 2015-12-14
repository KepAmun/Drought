using UnityEngine;
using System.Collections;

public class TerrainTile : Tile
{
    public GrowthHandler Growth { get; set; }
    
    TileContent _content;
    public TileContent Content
    {
        get
        {
            return _content;
        }

        set
        {
            if(value == null && _content != null)
            {
                _content.ContainingTile = null;
            }

            _content = value;

            if(_content != null)
            {
                _content.ContainingTile = this;
            }
        }
    }


    protected override void Awake()
    {
        base.Awake();

        Growth = GetComponent<GrowthHandler>();
    }


    protected virtual void Start()
    {
        CheckHealth();
    }


    public virtual void Advance()
    {
        if(Content != null)
        {
            Content.Advance();
        }
    }


    public virtual void CheckHealth()
    {
        if(Content != null)
        {
            Content.CheckHealth();
        }
    }

}
