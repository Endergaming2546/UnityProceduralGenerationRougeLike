using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GenerateMapSize : MonoBehaviour
{

    public GenerateStartingRoom GenerateStartingRoom;

    public GameObject[] CheckCollisonsArray;

    public GameObject MapBorderObject;
    public GameObject MapBorderObjectParent;

    public int BorderSizeX = 15;
    public int MaxBorderSizeX = 15;
    public int MinBorderSizeX = 3;

    public int BorderSizeY = 15;
    public int MaxBorderSizeY = 15;
    public int MinBorderSizeY = 3;

    public int StartingOffsetX = 5;
    public int StartingOffsetY = 5;

    int currentIndex = 0;

    //Offset for Bottom and Top on Y axis
    public float YOffset = 0.5f;
    //Offset for Left and Right on X axis
    public float XOffset = 0.5f;

    public bool RandomDungeonSize = true;

    private void Awake()
    {
        if (RandomDungeonSize)
        {
            BorderSizeX = Random.Range(MinBorderSizeX, MaxBorderSizeX);
            BorderSizeY = Random.Range(MinBorderSizeY, MaxBorderSizeY);
        }

        BorderSizeX = Mathf.Clamp(BorderSizeX, MinBorderSizeX, MaxBorderSizeX);
        BorderSizeY = Mathf.Clamp(BorderSizeY, MinBorderSizeY, MaxBorderSizeY);

        WaitToGenerateBorder();
    }

    async void WaitToGenerateBorder()
    {
        await Task.Delay(100);

        GenerateBorder();
    }

    void GenerateBorder()
    {
        BorderSizeX = Mathf.Clamp(BorderSizeX, MinBorderSizeX, MaxBorderSizeX);
        BorderSizeY = Mathf.Clamp(BorderSizeY, MinBorderSizeY, MaxBorderSizeY);

        //GenerateBottom
        for (int x = 0 + StartingOffsetX; x < BorderSizeX * 10; x += 10)
        {
            GameObject newGameObject = Instantiate(MapBorderObject, new Vector2(x, 0 + YOffset), Quaternion.identity);
            GenerateStartingRoom.BorderColliders[currentIndex] = newGameObject.GetComponent<Collider2D>();
            newGameObject.transform.parent = MapBorderObjectParent.transform;
            newGameObject.layer = MapBorderObjectParent.layer;

            currentIndex++;
        }

        //GenerateTop
        for (int x = 0 + StartingOffsetX; x < BorderSizeX * 10; x += 10)
        {
            GameObject newGameObject = Instantiate(MapBorderObject, new Vector2(x, BorderSizeY * 10 - YOffset), Quaternion.identity);
            GenerateStartingRoom.BorderColliders[currentIndex] = newGameObject.GetComponent<Collider2D>();
            newGameObject.transform.parent = MapBorderObjectParent.transform;
            newGameObject.layer = MapBorderObjectParent.layer;

            currentIndex++;
        }

        //GenerateLeft
        for (int y = 0 + StartingOffsetY; y < BorderSizeY * 10; y += 10)
        {
            GameObject newGameObject = Instantiate(MapBorderObject, new Vector2(0 + XOffset, y), Quaternion.identity);
            newGameObject.transform.eulerAngles = new Vector3(0, 0, 90);
            GenerateStartingRoom.BorderColliders[currentIndex] = newGameObject.GetComponent<Collider2D>();
            newGameObject.transform.parent = MapBorderObjectParent.transform;
            newGameObject.layer = MapBorderObjectParent.layer;

            currentIndex++;
        }

        //GenerateRight
        for (int y = 0 + StartingOffsetY; y < BorderSizeY * 10; y += 10)
        {
            GameObject newGameObject = Instantiate(MapBorderObject, new Vector2(BorderSizeX * 10 - XOffset, y), Quaternion.identity);
            newGameObject.transform.eulerAngles = new Vector3(0, 0, 90);
            GenerateStartingRoom.BorderColliders[currentIndex] = newGameObject.GetComponent<Collider2D>();
            newGameObject.transform.parent = MapBorderObjectParent.transform;
            newGameObject.layer = MapBorderObjectParent.layer;


            currentIndex++;
        }

        Physics2D.IgnoreLayerCollision(MapBorderObjectParent.layer, MapBorderObjectParent.layer, true);
    }

}
