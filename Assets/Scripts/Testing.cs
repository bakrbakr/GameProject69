using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private ZooGrid<bool> zooGrid;
    void Start()
    {
        zooGrid = new ZooGrid<bool>(5, 5, 10f, new Vector3(0, 0)); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            zooGrid.SetValue(GridUtils.GetMouseWorldPosition(), true);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(zooGrid.GetValue(GridUtils.GetMouseWorldPosition()));
        }
    }
}
