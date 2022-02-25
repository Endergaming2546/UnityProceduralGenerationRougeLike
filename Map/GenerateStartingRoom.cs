using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GenerateStartingRoom : MonoBehaviour
{

    public GenerateMapSize GenerateMapSize;

    public GameObject StartingRoomParent;
    public GameObject[] Tiles;

    public Collider2D[] BorderColliders;

    public LayerMask BorderLayer;

    //How many floor tiles is it
    public int StartingRoomSizeX;
    public int StartingRoomSizeY;

    public int MaxStartingRoomSizeX;
    public int MinStartingRoomSizeX;
    public int MaxStartingRoomSizeY;
    public int MinStartingRoomSizeY;

    public Vector2 StartingRoomSize;

    public bool RandomStartingRoomSize = true;

    //Remember, the starting room will start here, this is NOT the middle, but the south-east most point in the StartingRoom
    public Vector2 StartingRoomPos;

    private void Awake()
    {
        if (RandomStartingRoomSize)
        {
            StartingRoomSizeX = Random.Range(MinStartingRoomSizeX, MaxStartingRoomSizeX);
            StartingRoomSizeY = Random.Range(MinStartingRoomSizeY, MaxStartingRoomSizeY);
        }

        StartingRoomSizeX = Mathf.Clamp(StartingRoomSizeX, MinStartingRoomSizeX, MaxStartingRoomSizeX);
        StartingRoomSizeY = Mathf.Clamp(StartingRoomSizeY, MinStartingRoomSizeY, MaxStartingRoomSizeY);

        StartingRoomSize = new Vector2(StartingRoomSizeX + 1, StartingRoomSizeY + 1);

        WaitForGenerateMapSize();
        WaitToGenerateArraySize();
    }

    async void WaitToGenerateArraySize()
    {
        await Task.Delay(50);

        BorderColliders = new Collider2D[(GenerateMapSize.BorderSizeX + GenerateMapSize.BorderSizeY) * 2];
    }

    async void WaitForGenerateMapSize()
    {
        await Task.Delay(500);

        GenerateStartingRoomPos();
    }

    void GenerateStartingRoomPos()
    {
        int MaxX = GenerateMapSize.BorderSizeX * 10;
        int MaxY = GenerateMapSize.BorderSizeY * 10;
        int MinXYConstant = 1;

        int startingPosXInt = Random.Range(MinXYConstant, MaxX);
        int startingPosYInt = Random.Range(MinXYConstant, MaxY);

        float startingPosX = startingPosXInt;
        float startingPosY = startingPosYInt;

        if (startingPosXInt == 1)
        {
            startingPosX = 1.5f;
        }

        if (startingPosYInt == 1)
        {
            startingPosY = 1.5f;
        }

        if (startingPosXInt == GenerateMapSize.BorderSizeX * 10)
        {
            startingPosX -= 1.5f;
        }

        if (startingPosYInt == GenerateMapSize.BorderSizeY * 10)
        {
            startingPosY -= 1.5f;
        }

        StartingRoomPos = new Vector2(startingPosX + 0.5f, startingPosY + 0.5f);

        StartingRoomParent.transform.position = StartingRoomPos;

        if (Physics2D.OverlapArea(StartingRoomPos + new Vector2(-1, -1), StartingRoomPos + StartingRoomSize + new Vector2(1, 1), BorderLayer) == true)
        {
            Debug.LogWarning("Invalid StartingRoomLocation, retrying");
            GenerateStartingRoomPos();
        }

        else
        {
            int startingRoomForX;
            int startingRoomForY;

            for (int x = 0; x < StartingRoomSizeX; x++)
            {
                for (int y = 0; y < StartingRoomSizeY; y++)
                {
                    GameObject newGameObject = Instantiate(Tiles[0], new Vector2(x, y) + StartingRoomPos, Quaternion.identity);
                    newGameObject.transform.parent = StartingRoomParent.transform;

                    startingRoomForX = x;
                    startingRoomForY = y;
                }
            }

            //BottomWall
            for (int x = 0; x < StartingRoomSizeX; x++)
            {
                GameObject newGameObject = Instantiate(Tiles[1], new Vector2(x, -1) + StartingRoomPos, Quaternion.identity);
                newGameObject.transform.parent = StartingRoomParent.transform;
            }

            //LeftWall
            for (int y = 0; y < StartingRoomSizeY; y++)
            {
                GameObject newGameObject = Instantiate(Tiles[2], new Vector2(-1, y) + StartingRoomPos, Quaternion.identity);
                newGameObject.transform.parent = StartingRoomParent.transform;
            }

            //RightWall
            for (int y = 0; y < StartingRoomSizeY; y++)
            {
                GameObject newGameObject = Instantiate(Tiles[3], new Vector2(StartingRoomSizeX, y) + StartingRoomPos, Quaternion.identity);
                newGameObject.transform.parent = StartingRoomParent.transform;
            }

            //TopWall
            for (int x = 0; x < StartingRoomSizeX; x++)
            {
                GameObject newGameObject = Instantiate(Tiles[4], new Vector2(x, StartingRoomSizeY) + StartingRoomPos, Quaternion.identity);
                newGameObject.transform.parent = StartingRoomParent.transform;
            }

            //Generate NorthEastCorner
            GameObject newGameObject1 = Instantiate(Tiles[5], new Vector2(StartingRoomSizeX, StartingRoomSizeY) + StartingRoomPos, Quaternion.identity);
            newGameObject1.transform.parent = StartingRoomParent.transform;

            //Generate NorthWesternCorner
            GameObject newGameObject2 = Instantiate(Tiles[5], new Vector2(-1, StartingRoomSizeY) + StartingRoomPos, Quaternion.Euler(new Vector3(0, 0, 90)));
            newGameObject2.transform.parent = StartingRoomParent.transform;

            //Generate SouthWesternCorner
            GameObject newGameObject3 = Instantiate(Tiles[5], new Vector2(-1, -1) + StartingRoomPos, Quaternion.Euler(new Vector3(0, 0, 180)));
            newGameObject3.transform.parent = StartingRoomParent.transform;

            //Generate SouthEastCorner
            GameObject newGameObject4 = Instantiate(Tiles[5], new Vector2(StartingRoomSizeX, -1) + StartingRoomPos, Quaternion.Euler(new Vector3(0, 0, 270)));
            newGameObject4.transform.parent = StartingRoomParent.transform;
        }
    }

}
