using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    GameBoard _gameBoard;

    // UI
    int _food;
    public int Food
    {
        get
        {
            return _food;
        }

        set
        {
            _food = value;
            FoodText.text = _food.ToString();

            if(_food == 0)
            {
                FoodText.color = Color.red;
            }
            else if(_food < 0)
            {
                State = GameState.GameOver;
                FoodText.text = string.Empty;
            }
            else
            {
                FoodText.color = Color.white;
            }
        }
    }
    Text FoodText;
    GameObject GameOverPanel;

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
                switch(_state)
                {
                    case GameState.Starting:
                        break;
                    case GameState.Waiting:
                        for(int i = 0; i < _hand.Count; i++)
                        {
                            _hand[i].Locked = true;
                        }
                        break;
                    case GameState.Advancing:
                        break;
                    case GameState.GameOver:
                        GameOverPanel.SetActive(false);
                        break;
                    default:
                        break;
                }

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
                        break;
                    case GameState.GameOver:
                        GameOverPanel.SetActive(true);
                        ClearHand();
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

        FoodText = GameObject.Find("FoodLabel").GetComponent<Text>();
        GameOverPanel = GameObject.Find("GameOverPanel");
        GameOverPanel.SetActive(false);
    }


    void Start()
    {
        Food = 20;

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
        yield return new WaitForSeconds(0.4f);
        
        _gameBoard.Advance();

        yield return new WaitForSeconds(0.4f);

        Food--;

        if(State != GameState.GameOver)
        {
            ResetHand();

            State = GameState.Waiting;
        }
    }


    void ResetHand()
    {
        ClearHand();
        
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


    void ClearHand()
    {
        for(int i = 0; i < _hand.Count; i++)
        {
            Destroy(_hand[i].gameObject);
        }

        _hand.Clear();
    }


    Tile RandomTile()
    {
        Tile tile = null;

        Tile.TileType tileType = (Tile.TileType)(Random.Range(0, System.Enum.GetValues(typeof(Tile.TileType)).Length));

        tile = _gameBoard.MakeTile(tileType);

        return tile;
    }


    public void OnRestartClicked()
    {
        Start();
        _gameBoard.Restart();
    }
}
