using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    GameBoard _gameBoard;
    Tile_Harvest _harvestTile;

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

    int _turnNumber;
    public int TurnNumber
    {
        get
        {
            return _turnNumber;
        }

        set
        {
            if(_turnNumber != value)
            {
                _turnNumber = value;

                TimeText.text = _turnNumber.ToString();
            }
        }
    }

    Text TimeText;
    GameObject GameOverPanel;

    // Hand of tiles
    List<Tile> _hand;
    int _handLimit = 3;
    GameObject _handHost;
    List<Tile.TileType> _tileTypeDistribution;

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

                        _harvestTile.Locked = true;

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

                        _harvestTile.Locked = false;

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
        _harvestTile = _handHost.transform.GetComponentInChildren<Tile_Harvest>();
        _harvestTile.GameBoard = _gameBoard;
        _harvestTile.TileContentHarvested += OnTileContentHarvested;
        _harvestTile.Activated += OnHarvestTileActivated;

        _hand = new List<Tile>();

        FoodText = GameObject.Find("FoodLabel").GetComponent<Text>();
        TimeText = GameObject.Find("TimeLabel").GetComponent<Text>();
        TurnNumber = 0;
        GameOverPanel = GameObject.Find("GameOverPanel");
        GameOverPanel.SetActive(false);

        _tileTypeDistribution = new List<Tile.TileType>();
        for(int i = 0; i < 30; i++)
        {
            _tileTypeDistribution.Add(Tile.TileType.Ground);
        }

        for(int i = 0; i < 10; i++)
        {
            _tileTypeDistribution.Add(Tile.TileType.Water);
        }

        for(int i = 0; i < 10; i++)
        {
            _tileTypeDistribution.Add(Tile.TileType.Seed);
        }

        for(int i = 0; i < 10; i++)
        {
            _tileTypeDistribution.Add(Tile.TileType.Sun);
        }
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


    private void OnHarvestTileActivated(Tile obj)
    {
        Advance();
    }


    private void OnTileContentHarvested(TileContent tileContent)
    {

        switch(tileContent.Growth.Level)
        {
            case 0:
                Food += 1;
                break;

            case 1:
                Food += 2;
                break;

            case 2:
                Food += 3;
                break;

            case 3:
                Food += 5;
                break;

            default:
                break;
        }
        
        tileContent.Growth.Level = 0;
        
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
        TurnNumber++;

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
            _hand[i].Remove();
        }

        _hand.Clear();
    }


    Tile RandomTile()
    {
        Tile tile = null;

        Tile.TileType tileType = _tileTypeDistribution[Random.Range(0, _tileTypeDistribution.Count)];

        tile = _gameBoard.MakeTile(tileType);

        TerrainTile terrainTile = tile as TerrainTile;
        if(terrainTile != null && terrainTile.Type == Tile.TileType.Ground)
        {
            terrainTile.Growth.Level = Random.Range(0, 3);
            terrainTile.CheckHealth();
        }

        return tile;
    }


    public void OnRestartClicked()
    {
        Start();
        _gameBoard.Restart();
    }
}
