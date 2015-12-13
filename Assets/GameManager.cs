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
        GameObject[] testHand = new GameObject[3];
        testHand[0] = _gameBoard.MudPrefab;
        testHand[1] = _gameBoard.GrassPrefab;
        testHand[2] = _gameBoard.WaterPrefab;

        for(int i = 0; i < _handLimit; i++)
        {
            GameObject tileHost = Instantiate<GameObject>(testHand[i]);
            tileHost.transform.position = _handHost.transform.GetChild(i).transform.position;
            tileHost.transform.SetParent(_handHost.transform);
            Tile tile = tileHost.GetComponent<Tile>();
            tile.Locked = false;
            _hand.Add(tile);
            tile.MouseDown += OnTileStartDrag;
        }

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

        _state = GameState.Waiting;
    }

}
