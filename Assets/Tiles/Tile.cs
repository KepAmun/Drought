using UnityEngine;
using System.Collections.Generic;
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

    AudioSource _audio;
    Light _light;
    public bool LightOn
    {
        get
        {
            return _light.enabled;
        }

        set
        {
            _light.enabled = value;
        }
    }


    Coroutine _moveToCoroutine;


    protected virtual void Awake()
    {
        _collider = GetComponent<Collider>();
        _audio = GetComponent<AudioSource>();
        _light = GetComponentInChildren<Light>();
        LightOn = false;
    }


    void OnMouseDown()
    {
        LightOn = false;
        StopAllCoroutines();
        StartCoroutine(DoDrag());
    }


    System.Collections.IEnumerator DoDrag()
    {
        bool dragging = true;

        Vector3 startingPos = transform.position;

        //StopCoroutine("DoMoveTo");

        GameBoard.Coords prevTargetCoords = new GameBoard.Coords() { x = -1, y = -1 };
        List<TerrainTile> targetTiles = new List<TerrainTile>();

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
                    MoveTo(hitInfo.point + (Vector3.up * 0.85f));
                }
                
                GameBoard.Coords targetCoords = GameBoard.PositionToCoords(transform.position);
                if(targetCoords.x != prevTargetCoords.x || targetCoords.y != prevTargetCoords.y)
                {
                    foreach(var tile in targetTiles)
                    {
                        tile.LightOn = false;
                    }

                    targetTiles = GetTargets(targetCoords);

                    foreach(var tile in targetTiles)
                    {
                        tile.LightOn = true;
                    }

                    prevTargetCoords = targetCoords;
                }

            }

            yield return null;
        }

        foreach(var tile in targetTiles)
        {
            tile.LightOn = false;
        }

        if(!Activate())
        {
            MoveTo(startingPos);
        }
        else if(_audio != null)
        {
            _audio.Play();
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
        if(_moveToCoroutine != null)
        {
            StopCoroutine(_moveToCoroutine);
        }

        _moveToCoroutine = StartCoroutine(DoMoveTo(targetPosition, delay));
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


    protected TerrainTile ChangeTo(Tile.TileType type)
    {
        TerrainTile tile = GameBoard.MakeTile(type) as TerrainTile;
        tile.transform.position = transform.position + Vector3.down;
        GameBoard.PlaceTile(tile, this);

        return tile;
    }


    public void Remove()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = -1;

        LightOn = false;

        MoveTo(targetPosition);

        Destroy(gameObject, 2);
    }


    protected virtual List<TerrainTile> GetTargets(GameBoard.Coords targetCoords)
    {
        List<TerrainTile> targetTiles = new List<TerrainTile>();

        TerrainTile targetTile = GameBoard.Instance.GetTileAt(targetCoords);

        if(targetTile != null)
        {
            targetTiles.Add(targetTile);
        }

        return targetTiles;
    }
}
