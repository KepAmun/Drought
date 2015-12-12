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
        for(int i = 0; i < _handLimit; i++)
        {
            GameObject tileHost = Instantiate<GameObject>(_gameBoard.TilePrefab);
            tileHost.transform.position = _handHost.transform.GetChild(i).transform.position;
            Tile tile = tileHost.GetComponent<Tile>();
            _hand.Add(tile);
            tile.MouseDown += OnTileStartDrag;
        }
    }


    void Start()
    {

    }


    public void OnTileStartDrag(Tile tile)
    {
        StartCoroutine(DoDrag(tile));
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
                    tile.transform.position = hitInfo.point + (Vector3.back * 1.0f);
                }

            }

            yield return null;
        }

        if(_gameBoard.PlaceTile(tile))
        {
            _hand.Remove(tile);
            _state = GameState.Advancing;
        }
        else
        {
            tile.transform.position = startingPos;
        }

    }


    void Advance()
    {
        _gameBoard.Advance();
    }


}
