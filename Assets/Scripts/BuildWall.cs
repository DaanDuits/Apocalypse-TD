using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class BuildWall : MonoBehaviour
{
    [SerializeField]
    GameObject Overlay;

    [SerializeField]
    Wall[] Walls;

    [SerializeField]
    Tilemap tileMap;

    bool Left, Right = false;

    TowerShop shop;

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
    }

    public IEnumerator MoveBuildWall(Wall wall, int price)
    {
        GameObject wallObject = Instantiate(wall.WallPrefab);
        shop.canBuild = false;
        BoxCollider2D collider = wallObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            wallObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wallObject.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);

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
                shop.counter.Addresources(price);
                Destroy(wallObject);
                shop.canBuild = true;
                break;
            }

            if (Input.GetMouseButton(0) && !GameObject.FindObjectsOfType<GameObject>().Any(c => c != wallObject && (c.name == "Wall(Clone)" && c.transform.GetChild(0).position == wallObject.transform.GetChild(0).position)) && tileMap.HasTile(tileMap.WorldToCell(mousePos)))
            { 
                wallObject.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
                shop.canBuild = true; if (collider != null)
                {
                    wallObject.GetComponent<BoxCollider2D>().enabled = true;
                }
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        
    }
}
