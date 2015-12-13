using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    enum GameState { Starting, Waiting, Advancing, GameOver, }
    GameState _state = GameState.Starting;

    GameBoard _gameBoard;
    int _food;

    // Hand of tiles
    List<Tile> _hand;
    int _handLimit = 3;
    GameObject _handHost;


    void Awake()
    {
        _gameBoard = Transform.FindObjectOfType<GameBoard>();
        _handHost = GameObject.Find("Hand");

        _hand = new List<Tile>();

    }


    void Start()
    {
        ResetHand();

        _state = GameState.Waiting;
    }


    public void OnTileStartDrag(Tile tile)
    {
        if(_state == GameState.Waiting)
        {
            StartCoroutine(DoDrag(tile));
        }
    }


    System.Collections.IEnumerator DoDrag(Tile tile)
    {
        bool dragging = true;

        Vector3 startingPos = tile.transform.position;

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
                    tile.transform.position = hitInfo.point + (Vector3.up * 0.5f);
                }

            }

            yield return null;
        }

        if(_gameBoard.PlaceTile(tile))
        {
            _hand.Remove(tile);
            tile.Locked = true;
            Advance();
        }
        else
        {
            tile.transform.position = startingPos;
        }

    }


    void Advance()
    {
        _state = GameState.Advancing;

        StartCoroutine(DoAdvance());
    }


    System.Collections.IEnumerator DoAdvance()
    {
        yield return new WaitForSeconds(0.2f);

        _gameBoard.Advance();

        ResetHand();

        _state = GameState.Waiting;
    }


    void ResetHand()
    {
        for(int i = 0; i < _hand.Count; i++)
        {
            Destroy(_hand[i].gameObject);
        }

        _hand.Clear();
        
        for(int i = 0; i < _handLimit; i++)
        {
            Tile tile = RandomTile();
            tile.transform.position = _handHost.transform.GetChild(i).transform.position;
            tile.transform.SetParent(_handHost.transform);
            tile.MouseDown += OnTileStartDrag;
            tile.Locked = false;

            _hand.Add(tile);
        }

    }


    Tile RandomTile()
    {
        Tile tile = null;

        Tile.TileType tileType = (Tile.TileType)(Random.Range(0, System.Enum.GetValues(typeof(Tile.TileType)).Length));

        tile = _gameBoard.MakeTile(tileType);

        return tile;
    }
}
