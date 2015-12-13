using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    int level = 0;

    public GameBoard GameBoard { get; set; }
    public bool Locked { get; set; }

    public event System.Action<Tile> MouseDown;

    void Start()
    {

    }


    public void Advance()
    {
        Debug.Log("Advancing: " + (int)(transform.position.x) + ", " + (int)(transform.position.y));
    }


    void OnMouseDown()
    {
        if(!Locked && MouseDown != null)
            MouseDown(this);
    }
}
