using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{

    public GameObject FloorTileObject;

    private void Awake()
    {
        GameObject newFloorTile = Instantiate(FloorTileObject, transform.position, Quaternion.identity);

        newFloorTile.transform.parent = this.gameObject.transform;
    }

}
