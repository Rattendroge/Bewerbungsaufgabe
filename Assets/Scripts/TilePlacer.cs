using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private TileMapManager tileMapManager;

    [SerializeField]
    private GameManager gameManager;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameManager.GetGameOverState())
            {
                return;
            }
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            Vector2Int cellPosition2D = new Vector2Int(cellPosition.x, cellPosition.y);

            if (tileMapManager.IsOccupied(cellPosition2D) || tileMapManager.IsOutOfBoundaries(cellPosition2D))
            {
                return;
            }
            else
            {
                tileMapManager.PlaceTileOnMap(cellPosition2D);
                gameManager.AddTileToList(cellPosition2D);
                gameManager.ChangePlayer();
            }
        }
    }
}
