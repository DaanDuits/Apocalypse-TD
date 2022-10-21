using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class BuildTower : MonoBehaviour
{
    public GameObject Overlay;

    public Color Red, Green;

   Tilemap tileMap;

    TowerShop shop;

    bool CanPlace(GameObject tower, Vector2 gridPos)
    {
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
        if (!tileMap.HasTile(tileMap.WorldToCell(gridPos)))
            return false;
        foreach (GameObject o in objects)
        {
            if (o != tower && (Vector2)o.transform.position == gridPos && o.CompareTag("Tower") || o.CompareTag("Base") && Vector2.Distance(o.transform.position, gridPos) <= 1f)
                return false;
        }
        return true;
    }

    private void Start()
    {
        tileMap = GameObject.Find("Level").transform.GetChild(1).GetComponent<Tilemap>();
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    public IEnumerator MoveBuildTower(Tower tower)
    {
        GameObject towerObject = Instantiate(tower.TowerPrefab);
        GameObject overlay = Instantiate(Overlay);
        shop.canBuild = false;

        Collider2D collider = towerObject.GetComponent<Collider2D>();
        if (collider != null)
        {
            towerObject.GetComponent<Collider2D>().enabled = false;
        }
        overlay.GetComponent<SpriteRenderer>().color = Green;
        while(true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            towerObject.transform.position = mousePos;
            overlay.transform.position = new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(overlay);
                Destroy(towerObject);
                shop.canBuild = true;
                break;
            }
            if (CanPlace(towerObject, overlay.transform.position))
            {
                overlay.GetComponent<SpriteRenderer>().color = Green;
                if (Input.GetMouseButtonDown(0))
                {
                    shop.counter.Removeresources(tower.price);
                    GameObject shooter = Instantiate(tower.shooterPrefab, towerObject.transform);
                    towerObject.transform.position = new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
                    shooter.transform.position = towerObject.transform.position;
                    Destroy(overlay);
                    shop.canBuild = true;
                    if (collider != null)
                    {
                        towerObject.GetComponent<Collider2D>().enabled = true;
                    }
                    break;
                }
            }
            else
            {
                overlay.GetComponent<SpriteRenderer>().color = Red;
            }
            yield return null;
        }
    }
}
