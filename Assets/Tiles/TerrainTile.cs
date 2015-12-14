using UnityEngine;
using System.Collections;

public class TerrainTile : Tile
{

    public int Health { get; set; }

    int _level = 0;
    GameObject[] _levelModels;
    public int Level
    {
        get
        {
            return _level;
        }

        set
        {
            if(_level != value)
            {
                _levelModels[_level].SetActive(false);
                _level = value;
                _levelModels[_level].SetActive(true);
            }
        }
    }


    TileContent _content;
    public TileContent Content
    {
        get
        {
            return _content;
        }

        set
        {
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

        Health = 0;
        Level = 0;

        int MaxLevel = transform.childCount;
        _levelModels = new GameObject[MaxLevel];
        for(int i = 0; i < MaxLevel; i++)
        {
            _levelModels[i] = transform.GetChild(i).gameObject;
            _levelModels[i].SetActive(false);
        }

        _levelModels[0].SetActive(true);

    }

    
    public virtual void Advance()
    {
        Health--;

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
