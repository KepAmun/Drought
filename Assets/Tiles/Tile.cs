using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public enum TileType { Desert, Grass, Mud, Water}
    public TileType Type { get; protected set; }

    int level = 0;

    public GameBoard GameBoard { get; set; }
    public bool Locked { get; set; }

    public event System.Action<Tile> MouseDown;
    

    public virtual void Advance()
    {
    }


    void OnMouseDown()
    {
        if(!Locked && MouseDown != null)
            MouseDown(this);
    }
}
