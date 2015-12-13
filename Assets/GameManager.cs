using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    GameBoard _gameBoard;
    int _food;

    // Hand of tiles
    List<Tile> _hand;
    int _handLimit = 3;
    GameObject _handHost;

    enum GameState { Starting, Waiting, Advancing, GameOver, }
    GameState _state = GameState.Starting;
    private GameState State
    {
        get
        {
            return _state;
        }

        set
        {
            if(_state != value)
            {
                _state = value;

                switch(_state)
                {
                    case GameState.Starting:
                        break;
                    case GameState.Waiting:
                        for(int i = 0; i < _hand.Count; i++)
                        {
                            _hand[i].Locked = false;
                        }
                        break;
                    case GameState.Advancing:
                        for(int i = 0; i < _hand.Count; i++)
                        {
                            _hand[i].Locked = true;
                        }
                        break;
                    case GameState.GameOver:
                        break;
                    default:
                        break;
                }
            }
        }
    }


    void Awake()
    {
        _gameBoard = Transform.FindObjectOfType<GameBoard>();
        _handHost = GameObject.Find("Hand");

        _hand = new List<Tile>();

    }


    void Start()
    {
        ResetHand();

        State = GameState.Waiting;
    }


    public void OnHandTileActivated(Tile tile)
    {
        tile.Activated -= OnHandTileActivated;
        _hand.Remove(tile);
        Advance();
    }


    void Advance()
    {
        State = GameState.Advancing;

        StartCoroutine(DoAdvance());
    }


    System.Collections.IEnumerator DoAdvance()
    {
        yield return new WaitForSeconds(0.2f);

        _gameBoard.Advance();

        ResetHand();

        State = GameState.Waiting;
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
            Vector3 targetPosition = _handHost.transform.GetChild(i).transform.position;
            tile.transform.position = targetPosition + Vector3.back * 10;
            tile.MoveTo(targetPosition, 0.2f * i);
            tile.transform.SetParent(_handHost.transform);
            tile.Activated += OnHandTileActivated;
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
