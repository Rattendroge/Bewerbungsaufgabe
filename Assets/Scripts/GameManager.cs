using System.Collections.Generic;
using UnityEngine;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum TileType
{
    Cross = 0,
    Circle = 1,
    None = 2,
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TileMapManager tileMapManager;

    [SerializeField]
    private int gridHeight;
    public int GetGridHeigth(){ return gridHeight; }
    
    [SerializeField]
    private int gridWidth;
    public int GetGridWidth(){ return gridWidth; }

    private int currentPlayer = 1;
    public int GetCurrentPlayer(){ return currentPlayer; }

    private bool gameOver = false;
    public bool GetGameOverState(){ return gameOver; }

    [SerializeField]
    TMP_Text turnText;

    [SerializeField]
    TMP_Text looseText;


    Dictionary<Vector2Int, TileType> currentTilePosAndType = new Dictionary<Vector2Int, TileType>();


    public void ChangePlayer()
    {
        if(currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else if(currentPlayer == 2)
        {
            currentPlayer = 1;
        }

        if (!gameOver)
        {
            turnText.text = "Player " + currentPlayer + " Turn";
        }
        else
        {
            turnText.text = "";
        }
        
    }

    public void AddTileToList(Vector2Int _position)
    {
        currentTilePosAndType.Add(_position,GetCurrentPlayerTileType());
        if(CheckForLoose(_position, GetCurrentPlayerTileType()))
        {
            gameOver = true;
            looseText.text = "Player " + currentPlayer + " Lost";
            //Loose Logic current Player
        }
        else
        {
            if(currentTilePosAndType.Count == gridHeight * gridWidth)
            {
                gameOver = true;
                looseText.text = "Draw";
                turnText.text = "";
            }
        }
    }

    private TileType GetCurrentPlayerTileType()
    {
        if(currentPlayer == 1)
        {
            return TileType.Cross;
        }
        else if(currentPlayer == 2)
        {
            return TileType.Circle;
        }
        return TileType.None;
    }

    private bool CheckForLoose(Vector2Int _posFrom, TileType _tileType)
    {
        List<Vector2Int> allNeighborDirections = new List<Vector2Int>()
        {
            new Vector2Int (0, 1),
            new Vector2Int (1, 1),
            new Vector2Int (1, 0),
            new Vector2Int (1, -1),
            new Vector2Int (0, -1),
            new Vector2Int (-1, -1),
            new Vector2Int (-1, 0),
            new Vector2Int (-1, 1),
        };

        TileType foundTilesType;

        foreach(Vector2Int direction in allNeighborDirections)
        {
            if(currentTilePosAndType.TryGetValue(_posFrom + direction, out foundTilesType))
            {
                if(foundTilesType == _tileType)
                {
                    if(CheckForTileDirectional(_posFrom + direction, direction, _tileType))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool CheckForTileDirectional(Vector2Int _posFrom, Vector2Int _direction, TileType _tileType)
    {
        TileType foundTilesType;
        if(currentTilePosAndType.TryGetValue(_posFrom + _direction, out foundTilesType))
        {
            if(foundTilesType == _tileType)
            {
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    public void ResetGame()
    {
        currentTilePosAndType.Clear();
        tileMapManager.GetTilemap().ClearAllTiles();
        looseText.text = "";
        gameOver = false;
        currentPlayer = 1;
        turnText.text = "Player " + currentPlayer + " Turn";
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    } 
}
