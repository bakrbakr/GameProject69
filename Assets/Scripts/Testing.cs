using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private ZooGrid<ZooGridObject> zooGrid;
    public GameObject validPrefab;
    public GameObject invalidPrefab;
    void Start()
    {
        zooGrid = new ZooGrid<ZooGridObject>(5, 5, 10f, new Vector3(0, 0)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            zooGrid.GetGridIndices(GridUtils.GetMouseWorldPosition(), out int x, out int y);
            zooGrid.AddBuilding(new ZooGridObject(validPrefab, new Vector2(x, y), 2), GridUtils.GetMouseWorldPosition(), 2);
        }

        if (Input.GetMouseButtonDown(1))
        {
            zooGrid.RemoveBuilding(zooGrid.GetValue(GridUtils.GetMouseWorldPosition()));
        }
    }
}
