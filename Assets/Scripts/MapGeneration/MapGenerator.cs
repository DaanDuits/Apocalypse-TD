using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth, mapHeight;
    public float noiseScale;

    public int octaves;
    public float persistance, lacunarity;

    public Vector2 offset;
    public string seed;

    [SerializeField]
    GameObject[] levelObjs;
    [SerializeField]
    int levelAmount;

    [SerializeField]
    HeightMaterial[] materials;

    [System.Serializable]
    public class HeightMaterial
    {
        public float minHeight, maxHeight;
        public Material mat;
    }

    private void Awake()
    {
        seed = PlayerPrefs.GetString("Seed");
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed.GetHashCode(), noiseScale, octaves, persistance, lacunarity, offset);

        MeshGenerator meshGen = GetComponent<MeshGenerator>();
        for (int i = 0; i < materials.Length; i++)
        {
            meshGen.GenerateMesh(createHeightSegment(noiseMap, materials[i].minHeight, materials[i].maxHeight), 1, materials[i].mat, i >= 4);
        }
        GenerateLevels();
    }
    void GenerateLevels()
    {

        System.Random rnd = new System.Random(seed.GetHashCode());
        List<Transform> levels = new List<Transform>();
        for (float x = -6; x < 6; x += 0.2f)
        {
            for (float y = -4.5f; y < 4.5f; y += 0.2f)
            {
                bool spawn = rnd.Next(0, levelAmount) == 1;
                if (spawn)
                {
                    if (Physics.Raycast(new Vector3(x, y, -4), Vector3.forward, 9f))
                    {
                        int levelType = rnd.Next(0, levelObjs.Length);
                        Vector3 pos = new Vector2(x, y);
                        if (levels.Count > 0)
                        {
                            if (GetClosestLevel(levels, 0.5f, pos) == null)
                            {
                                GameObject level = Instantiate(levelObjs[levelType], transform);
                                level.transform.position = pos;
                                levels.Add(level.transform);
                            }
                        }
                        else
                        {
                            GameObject level = Instantiate(levelObjs[levelType], transform);
                            level.transform.position = pos;
                            levels.Add(level.transform);
                        }
                    }
                }
            }
        }
    }

    Transform GetClosestLevel(List<Transform> levels, float range, Vector2 level)
    {
        Transform closest = null;
        foreach (Transform t in levels)
        {
            if (Vector3.Distance(t.position, level) < range)
            {
                closest = t;
            }
        }
        return closest;
    }

    int[,] createHeightSegment(float[,] map, float min, float max)
    {
        int[,] mapSegment = new int[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] <= max && map[x, y] >= min)
                    mapSegment[x, y] = 1;
                else
                    mapSegment[x, y] = 0;
            }
        }
        return mapSegment;
    }
}
