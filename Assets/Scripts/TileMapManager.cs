using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TileMapManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    private Tilemap tilemap;
    public Tilemap GetTilemap(){ return tilemap; }

    [SerializeField]
    private TileBase tileCross;

    [SerializeField]
    private TileBase tileCircle;

    private int gridHeight;

    private int gridWidth;

    void Start()
    {
        gridHeight = gameManager.GetGridHeigth();
        gridWidth = gameManager.GetGridWidth();
    }

    private TileBase GetTileBaseForPlayer()
    {
        int player = gameManager.GetCurrentPlayer();

        if(player == 1)
        {
            return tileCross;
        }
        else
        {
            return tileCircle;
        }
    }

    public void PlaceTileOnMap(Vector2Int _position)
    {
        tilemap.SetTile(new Vector3Int(_position.x, _position.y, 0),GetTileBaseForPlayer());
    }

    public void RemoveTileFromMap(Vector2Int _position)
    {
        tilemap.SetTile(new Vector3Int(_position.x, _position.y, 0), null);
    }

    public bool IsOccupied(Vector2Int _position)
    {
        if(tilemap.GetTile(new Vector3Int(_position.x,_position.y,0)) != null)
        {
            return true;
        }
        return false;
    }

    public bool IsOutOfBoundaries(Vector2Int _position)
    {
        if(_position.x > gridHeight / 2 - 1 || _position.y > gridWidth / 2 - 1 || _position.x < gridHeight / 2 * -1 || _position.y < gridWidth / 2 * -1)
        {
            return true;
        }
        return false;
    }
}
