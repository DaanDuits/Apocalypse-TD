using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class BuildWall : MonoBehaviour
{
    Tilemap tileMap;

    TowerShop shop;

    public List<Vector2> noBuildArea = new List<Vector2>();

    public static BuildWall instance; 

    bool CanPlace(GameObject wall, Vector2 gridPos)
    {
        //Check if the given wall can be placed on the given position
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();

        int right = tileMap.HasTile(tileMap.WorldToCell(gridPos)) ? 1 : 0;
        int left = tileMap.HasTile(tileMap.WorldToCell(gridPos - (Vector2)wall.transform.right)) ? 1 : 0;

        switch (right, left)
        {
            case (1, 1):
                if (noBuildArea.Any(p => p == gridPos) && noBuildArea.Any(p => p == gridPos - (Vector2)wall.transform.right))
                {
                    return false;
                }
                break;
            case (0, 0):
                return false;
            case (1, 0):
                return false;
            case (0, 1):
                return false;
        }
        foreach (GameObject o in objects)
        {
            if (o != wall && o.CompareTag("Wall") && o.transform.GetChild(0).position == wall.transform.GetChild(0).position || o.CompareTag("Base") && Vector2.Distance(o.transform.position, wall.transform.GetChild(0).position) <= 1f)
                return false;
        }
        return true;
    }

    private void Start()
    {
        tileMap = GameObject.Find("Level").transform.GetChild(1).GetComponent<Tilemap>();
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
        instance = this;
        GameObject[] area = GameObject.FindGameObjectsWithTag("NoBuild");
        for (int i = 0; i < area.Length; i++)
        {
            Transform areaT = area[i].transform;
            for (float x = areaT.position.x - (areaT.localScale.x / 2) + .5f; x <= areaT.position.x + (areaT.localScale.x / 2) - .5f; x++)
            {
                for (float y = areaT.position.y - (areaT.localScale.y / 2) + .5f; y <= areaT.position.y + (areaT.localScale.y / 2) - .5f; y++)
                {
                    noBuildArea.Add(new Vector2(x, y));
                }
            }
        }
    }

    public IEnumerator MoveBuildWall(Wall wall, bool isCopy) => MoveBuildWall(wall, isCopy, Vector3.zero);
    public IEnumerator MoveBuildWall(Wall wall, bool isCopy, Vector3 rotation)
    {
        GameObject wallObject = Instantiate(wall.WallPrefab);
        shop.canBuild = false; 
        if (isCopy)
            wallObject.transform.rotation = Quaternion.Euler(rotation);
        BoxCollider2D collider = wallObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            wallObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wallObject.transform.position = new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);

            if (Input.GetKeyDown(KeyCode.E))
            {
                wallObject.transform.Rotate(0, 0, -90);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                wallObject.transform.Rotate(0, 0, 90);
            }


            if (Input.GetMouseButtonDown(1))
            {
                Destroy(wallObject);
                shop.canBuild = true;
                break;
            }

            if (CanPlace(wallObject, wallObject.transform.position))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    shop.canBuild = true;
                    shop.counter.Removeresources(wall.price);
                    if (collider != null)
                        wallObject.GetComponent<BoxCollider2D>().enabled = true;

                    if (!shop.counter.CheckRemovedResources(wall.price))
                    {
                        break;
                    }
                    else
                    {
                        StartCoroutine(MoveBuildWall(wall, true, wallObject.transform.rotation.eulerAngles));
                        break;
                    }
                }
            }
            yield return null;
        }
    }
}
