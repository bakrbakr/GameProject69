using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    protected int width;
    protected int height;
    protected float cellSize;
    protected T[,] zooGrid;
    protected Vector3 originPosition;
    protected TextMesh[,] debugText;

    public Grid(int width, int height, float cellSize, Vector3 originPosition)
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
                debugText[i, j] = GridUtils.CreateWorldText("", null, GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * .5f, 21, Color.red, TextAnchor.MiddleCenter, TextAlignment.Center);

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

    public void GetGridIndices(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    protected void GetGridIndices(Vector2 worldPosition, out int x, out int y)
    {
        GetGridIndices(new Vector3(worldPosition.x, worldPosition.y), out x, out y);
    }

    #region Setting Values

    public void SetValue(int x, int y, T value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            zooGrid[x, y] = value;
            debugText[x, y].text = (value is null) ? "Null" : value.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, T value)
    {
        GetGridIndices(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public void SetValueInRange(int startX, int endX, int startY, int endY, T value)
    {
        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                SetValue(i, j, value);
            }
        }
    }

    public void SetValueInRange(Vector3 location, int offset, T value)
    {
        GetGridIndices(location, out int x, out int y);

        for (int i = x; i < x + offset; i++)
        {
            for (int j = y; j < y + offset; j++)
            {
                SetValue(i, j, value);
            }
        }
    }

    #endregion

    #region Getting Values

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
        GetGridIndices(worldPosition, out int x, out int y);

        return GetValue(x, y);
    }

    public T[,] GetValuesInRange(int startX, int startY, int endX, int endY)
    {
        T[,] values = new T[endX - startX, endY - startY];

        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                values[i, j] = zooGrid[i, j];
            }
        }

        return values;
    }

    public T[,] GetValuesInRange(Vector2 start, Vector2 end)
    {
        return GetValuesInRange((int)start.x, (int)start.y, (int)end.x, (int)end.y);
    }
    public T[,] GetValuesInRange(Vector4 range)
    {
        return GetValuesInRange((int)range.x, (int)range.y, (int)range.z, (int)range.w);
    }

    public T[,] GetValuesInRange(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        GetGridIndices(startWorldPosition, out int startX, out int startY);
        GetGridIndices(endWorldPosition, out int endX, out int endY);

        return GetValuesInRange(startX, startY, endX, endY);
    }

    #endregion
}

public class ZooGrid<ZooGridObject> : Grid<ZooGridObject>
{
    Dictionary<ZooGridObject, Vector4> buildings;

    public ZooGrid(int width, int height, float cellSize, Vector3 originPosition) : base(width, height, cellSize, originPosition)
    {
        buildings = new Dictionary<ZooGridObject, Vector4>();
    }

    #region Building Manipulation
    public void AddBuilding(ZooGridObject building, Vector2 location, int size)
    {
        int x = (int)location.x, y = (int)location.y;

        if (!SpaceIsFree(x, x + size, y, y + size)) return;

        SetValueInRange(x, x + size, y, y + size, building);
        buildings.Add(building, new Vector4(x, x + size, y, y + size));
    }

    public void AddBuilding(ZooGridObject building, Vector3 location, int size)
    {
        GetGridIndices(location, out int x, out int y);

        if (!WithinBounds(x, x + size, y, y + size) || !SpaceIsFree(x, x + size, y, y + size)) return;

        SetValueInRange(x, x + size, y, y + size, building);
        buildings.Add(building, new Vector4(x, x + size, y, y + size));
    }

    public void RemoveBuilding(ZooGridObject building)
    {
        if (building is null) return;

        buildings.TryGetValue(building, out Vector4 location);

        SetValueInRange((int)location.x, (int)location.y, (int)location.z, (int)location.w, default);

        buildings.Remove(building);
    }

    #endregion

    #region Utilities

    public bool SpaceIsFree(int startX, int endX, int startY, int endY)
    {
        for (int i = startX; i < endX; i++)
        {
            for (int j = startY; j < endY; j++)
            {
                if (!(zooGrid[i, j] is null)) return false;
            }
        }

        return true;
    }

    public bool SpaceIsFree(Vector2 start, Vector2 end)
    {
        GetGridIndices(start, out int startX, out int startY);
        GetGridIndices(end, out int endX, out int endY);

        return SpaceIsFree(startX, endX, startY, endY);
    }

    public bool SpaceIsFree(Vector4 location)
    {
        return SpaceIsFree((int)location.x, (int)location.y, (int)location.z, (int)location.w);
    }

    public bool SpaceIsFree(Vector3 location, int offset)
    {
        GetGridIndices(new Vector2(location.x, location.y), out int startX, out int startY);

        return SpaceIsFree(startX, startX + offset, startY, startY + offset);
    }

    public bool SpaceIsFree(Vector3 location, int horizontalOffset, int verticalOffset)
    {
        GetGridIndices(new Vector2(location.x, location.y), out int startX, out int startY);

        return SpaceIsFree(startX, startX + horizontalOffset, startY, startY + verticalOffset);
    }

    public bool WithinBounds(int startX, int endX, int startY, int endY)
    {
        return startX >= 0
            && startY >= 0
            && endX <= width
            && endY <= height;
    }

    #endregion
}