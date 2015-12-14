using UnityEngine;
using System.Collections;
using System;

public class Tile : MonoBehaviour
{
    public enum TileType { Ground, Water, Seed, Sun, Fish }
    public TileType Type { get; protected set; }

    public GameBoard GameBoard { get; set; }
    Collider _collider;
    public bool Locked
    {
        get
        {
            return !_collider.enabled;
        }

        set
        {
            _collider.enabled = !value;
        }
    }

    public event System.Action<Tile> Activated;

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
    }


    void OnMouseDown()
    {
        if(!Locked)
        {
            StopAllCoroutines();
            StartCoroutine(DoDrag());
        }
    }


    System.Collections.IEnumerator DoDrag()
    {
        bool dragging = true;

        Vector3 startingPos = transform.position;

        //StopCoroutine("DoMoveTo");

        while(dragging)
        {
            if(Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
            else
            {
                RaycastHit hitInfo;
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(mouseRay, out hitInfo, 1000, LayerMask.GetMask("GameBoard")))
                {
                    transform.position = hitInfo.point + (Vector3.up * 0.8f);
                }

            }

            yield return null;
        }

        if(!Activate())
        {
            MoveTo(startingPos);
        }

    }


    public virtual bool Activate()
    {
        bool success = GameBoard.PlaceTile(this);

        if(success)
        {
            Locked = true;

            if(Activated != null)
                Activated(this);
        }

        return success;
    }


    public void MoveTo(Vector3 targetPosition, float delay = 0)
    {
        StopAllCoroutines();
        StartCoroutine(DoMoveTo(targetPosition, delay));
    }


    System.Collections.IEnumerator DoMoveTo(Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        while(Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 v = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref v, 0.05f);

            yield return null;
        }

        transform.position = targetPosition;
    }


    public virtual void CheckHealth()
    {

    }


    protected TerrainTile ChangeTo(Tile.TileType type)
    {
        TerrainTile tile = GameBoard.MakeTile(type) as TerrainTile;
        tile.transform.position = transform.position + Vector3.down;
        GameBoard.PlaceTile(tile, this);

        return tile;
    }


    public void Remove()
    {
        MoveTo(transform.position + (Vector3.down * 0.5f));
        Destroy(gameObject, 2);
    }
    
}
