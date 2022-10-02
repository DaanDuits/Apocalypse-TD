using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class BuildWall : MonoBehaviour
{

    [SerializeField]
    Tilemap tileMap;

    bool Left, Right = false;

    TowerShop shop;

    bool build = false;
    bool CanPlace(GameObject wall, Vector2 gridPos)
    {
        //Check if the given wall can be placed on the given position
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        if (!tileMap.HasTile(tileMap.WorldToCell(gridPos)))
            return false;
        foreach (GameObject o in objects)
        {
            if (o != wall && o.CompareTag("Wall") && o.transform.GetChild(0).position == wall.transform.GetChild(0).position || o.CompareTag("Base") && Vector2.Distance(o.transform.position, wall.transform.GetChild(0).position) <= 1f)
                return false;
        }
        return true;
    }

    private void Start()
    {
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Right = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Left = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            build = true;
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

            if (Right)
            {
                Right = false;
                wallObject.transform.Rotate(0, 0, -90);
            }
            if (Left)
            {
                Left = false;
                wallObject.transform.Rotate(0, 0, 90);
            }


            if (Input.GetMouseButton(1))
            {
                Destroy(wallObject);
                shop.canBuild = true;
                break;
            }

            if (CanPlace(wallObject, wallObject.transform.position))
            {
                if (build)
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
                        build = false;
                        StartCoroutine(MoveBuildWall(wall, true, wallObject.transform.rotation.eulerAngles));
                        break;
                    }
                }
            }

            build = false;

            yield return new WaitForEndOfFrame();
        }
    }
}
