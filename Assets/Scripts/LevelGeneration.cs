using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField]


    int xSize;
    int ySize;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }
    
    void GenerateLevel()
    {
        xSize = Random.Range(10, 31);
        if (xSize < 20)
        {
            ySize = Random.Range(20, 31);
        }
        else
        {
            ySize = Random.Range(10, 21);
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {

            }
        }
    }
}
