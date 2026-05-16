using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Threading;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    
    private int gridWidth;

    private int gridHeight;

    [SerializeField]
    private int gridCellSize = 1;

    [SerializeField,Range(0.03f,0.3f)]
    private float lineThickness = 0.03f;

    [SerializeField]
    private Material lineMaterial;


    private List<Vector3[]> linesToDraw = new List<Vector3[]>();

    private bool horizontalReadyToDraw = false;

    private bool verticalReadyToDraw = false;


    private void Start()
    {
        gridHeight = gameManager.GetGridHeigth();
        gridWidth = gameManager.GetGridWidth();
        Thread verticalLines = new Thread(GenerateVertivalLines);
        Thread horizontalLines = new Thread(GenerateHorizontalLines);
        verticalLines.Start();
        horizontalLines.Start();
    }

    private void Update()
    {
        if (horizontalReadyToDraw && verticalReadyToDraw)
        {
            foreach (var line in linesToDraw)
            {
                DrawLine(line[0], line[1]);
            }
            horizontalReadyToDraw = false;
            verticalReadyToDraw = false;
        }
    }

    private void GenerateVertivalLines()
    {
        for (int x = 0 - gridWidth / 2; x <= gridWidth / 2; x++)
        {
            linesToDraw.Add(new Vector3[]
            {
                new Vector3(x * gridCellSize, 0 - gridHeight / 2, 0),
                new Vector3(x * gridCellSize, gridHeight / 2 * gridCellSize, 0)
            });
        }
        verticalReadyToDraw = true;
    }

    private void GenerateHorizontalLines()
    {
        for (int y = 0 - gridHeight / 2; y <= gridHeight / 2; y++)
        {
            linesToDraw.Add(new Vector3[]
            {
                new Vector3(0 - gridWidth / 2, y * gridCellSize, 0),
                new Vector3(gridWidth / 2 * gridCellSize, y * gridCellSize, 0)
            });
        }
        horizontalReadyToDraw = true;
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject line = new GameObject("GridLine");
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.useWorldSpace = true;
    }
}
