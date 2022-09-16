using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class BuildTower : MonoBehaviour
{
    [SerializeField]
    GameObject Overlay;

    [SerializeField]
    Tower[] Towers;

    [SerializeField]
    Color Red, Green;

    [SerializeField]
    Tilemap tileMap;

    TowerShop shop;

    private void Start()
    {
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    public IEnumerator MoveBuildTower(Tower tower)
    {
        GameObject towerObject = Instantiate(tower.TowerPrefab);
        GameObject overlay = Instantiate(Overlay);
        shop.canBuild = false;
        overlay.GetComponent<SpriteRenderer>().color = Green;
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            towerObject.transform.position = mousePos;
            overlay.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);

            if (GameObject.FindObjectsOfType<GameObject>().Any(c => c.name != "PlacingOverlay(Clone)" && c.name != "Wall(Clone)" && c.transform.position == new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f)))
            {
                overlay.GetComponent<SpriteRenderer>().color = Red;
            }
            else
            {
                overlay.GetComponent<SpriteRenderer>().color = Green;
            }

            if (Input.GetMouseButton(0) && !GameObject.FindObjectsOfType<GameObject>().Any(c => c.name != "PlacingOverlay(Clone)" && c.name != "Wall(Clone)" && c.transform.position == new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f)) && tileMap.HasTile(tileMap.WorldToCell(mousePos)))
            {
                towerObject.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
                shop.canBuild = true;
                Destroy(overlay);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
