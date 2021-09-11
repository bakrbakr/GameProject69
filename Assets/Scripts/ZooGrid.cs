using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooGrid<T>
{
    private int width;
    private int height;
    private float cellSize;
    private T[,] zooGrid;
    private Vector3 originPosition;
    private TextMesh[,] debugText;

    public ZooGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        zooGrid = new T[width, height];
        debugText = new TextMesh[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                debugText[i, j] = GridUtils.CreateWorldText(zooGrid[i, j].ToString(), null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 21, Color.red, TextAnchor.MiddleCenter, TextAlignment.Center);
                SetValue(i, j, default);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.blue, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.blue, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) 
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
     
    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            zooGrid[x, y] = value;
            debugText[x, y].text = value.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public T GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return zooGrid[x, y];
        }

        return default;
    }

    public T GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);

        return GetValue(x, y);
    }
}
