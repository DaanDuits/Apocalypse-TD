using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildTower : MonoBehaviour
{
    public GameObject Overlay;

    public Color Red;

    public Color Green;

    public GameObject rangeCircle;

    private Tilemap tileMap;

    private TowerShop shop;

    public GameObject cancelButton;

    private bool cancelled;

    private bool CanPlace(GameObject tower, Vector2 gridPos)
    {
        GameObject[] array = Object.FindObjectsOfType<GameObject>();
        if (!tileMap.HasTile(tileMap.WorldToCell(gridPos)) || BuildWall.instance.noBuildArea.Any((Vector2 p) => p == gridPos))
        {
            return false;
        }
        GameObject[] array2 = array;
        foreach (GameObject gameObject in array2)
        {
            if ((gameObject != tower && (Vector2)gameObject.transform.position == gridPos && gameObject.CompareTag("Tower")) || (gameObject.CompareTag("Base") && Vector2.Distance(gameObject.transform.position, gridPos) <= 1f))
            {
                return false;
            }
        }
        return true;
    }

    private void Start()
    {
        tileMap = GameObject.Find("Level").transform.GetChild(1).GetComponent<Tilemap>();
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    public void CancelBuilding()
    {
        cancelled = true;
    }

    public IEnumerator MoveBuildTower(Tower tower)
    {
        cancelButton.SetActive(value: true);
        Button.ButtonClickedEvent cancelOnClick = cancelButton.GetComponent<Button>().onClick;
        cancelOnClick.AddListener(CancelBuilding);
        GameObject towerObject = Object.Instantiate(tower.TowerPrefab);
        GameObject overlay = Object.Instantiate(Overlay);
        shop.canBuild = false;
        Collider2D collider = towerObject.GetComponent<Collider2D>();
        if (collider != null)
        {
            towerObject.GetComponent<Collider2D>().enabled = false;
        }
        overlay.GetComponent<SpriteRenderer>().color = Green;
        if (tower.shooterPrefab != null)
        {
            TowerBehaviour component = tower.shooterPrefab.GetComponent<TowerBehaviour>();
            if (component.range >= 8f)
            {
                rangeCircle.transform.localScale = new Vector2(2f, 2f);
            }
            else
            {
                rangeCircle.transform.localScale = new Vector2(component.range * 2f, component.range * 2f);
            }
        }
        Vector2 vector;
        while (true)
        {
            vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            towerObject.transform.position = vector;
            if (tower.shooterPrefab != null)
            {
                rangeCircle.SetActive(value: true);
                rangeCircle.transform.position = vector;
            }
            overlay.transform.position = new Vector3(Mathf.Floor(vector.x) + 0.5f, Mathf.Floor(vector.y) + 0.5f);
            if (Input.GetMouseButtonDown(1) || cancelled)
            {
                rangeCircle.SetActive(value: false);
                cancelOnClick.RemoveAllListeners();
                cancelButton.SetActive(value: false);
                Object.Destroy(overlay);
                Object.Destroy(towerObject);
                cancelled = false;
                shop.canBuild = true;
                yield break;
            }
            if (CanPlace(towerObject, overlay.transform.position))
            {
                overlay.GetComponent<SpriteRenderer>().color = Green;
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
            }
            else
            {
                overlay.GetComponent<SpriteRenderer>().color = Red;
            }
            yield return null;
        }
        rangeCircle.SetActive(value: false);
        cancelOnClick.RemoveAllListeners();
        cancelButton.SetActive(value: false);
        shop.counter.Removeresources(tower.price);
        towerObject.transform.position = new Vector3(Mathf.Floor(vector.x) + 0.5f, Mathf.Floor(vector.y) + 0.5f);
        if (tower.shooterPrefab != null)
        {
            Object.Instantiate(tower.shooterPrefab, towerObject.transform).transform.position = towerObject.transform.position;
        }
        Object.Destroy(overlay);
        shop.canBuild = true;
        if (collider != null)
        {
            towerObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
