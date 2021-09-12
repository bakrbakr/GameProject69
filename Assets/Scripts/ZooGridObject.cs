using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooGridObject
{
    public GameObject GameObject { get; }
    public Vector2 StartLocation { get; }
    public Vector2 EndLocation { get; }

    #region Constructors
    public ZooGridObject(GameObject gameObject, Vector4 location)
    {
        GameObject = gameObject;
        StartLocation = new Vector2(location.x, location.z);
        EndLocation = new Vector2(location.y, location.w);
    }

    public ZooGridObject(GameObject gameObject, Vector2 baseLocation, int size)
    {
        GameObject = gameObject;
        StartLocation = baseLocation;
        EndLocation = new Vector2(StartLocation.x + size, StartLocation.y + size);
    }

    public ZooGridObject(GameObject gameObject, Vector2 baseLocation, int horizontalSize, int verticalSize)
    {
        GameObject = gameObject;
        StartLocation = baseLocation;
        EndLocation = new Vector2(StartLocation.x + horizontalSize, StartLocation.y + verticalSize);
    }

    #endregion

    public override string ToString()
    {
        return GameObject.name;
    }
}
