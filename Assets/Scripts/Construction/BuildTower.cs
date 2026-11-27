using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TowerDefense.Building;
using TowerDefense.Construction;
using TowerDefense.Operator;

public class BuildTower : MonoBehaviour
{
    public GameObject Overlay;

    public Color Red;

    public Color Green;

    public GameObject rangeCircle;

    private TowerShop shop;

    public GameObject cancelButton;

    private bool cancelled;

    private bool CanPlace(Structure tower)
    {
        if (TileZoneBank.Get(TileZoneType.NoBuild).HasTile(tower.Center)) 
            return false;
        if (TileZoneBank.Get(TileZoneType.Occupied).HasTile(tower.Center))
            return false;
        if (!TileZoneBank.Get(TileZoneType.Build).HasTile(tower.Center))
            return false;

        return true;
    }

    private void Start()
    {
        shop = GameObject.Find("ShopController").GetComponent<TowerShop>();
    }

    public void CancelBuilding()
    {
        cancelled = true;
    }

    public IEnumerator MoveBuildTower(TowerBlueprint tower)
    {
        cancelButton.SetActive(true);
        Button.ButtonClickedEvent cancelOnClick = cancelButton.GetComponent<Button>().onClick;
        cancelOnClick.AddListener(CancelBuilding);
        Structure towerObject = Instantiate(tower.TowerPrefab);
        GameObject overlay = Instantiate(Overlay);
        shop.canBuild = false;
        Collider2D collider = towerObject.GetComponent<Collider2D>();
        if (collider != null)
        {
            towerObject.GetComponent<Collider2D>().enabled = false;
        }
        overlay.GetComponent<SpriteRenderer>().color = Green;
        if (tower.OperatorPrefab != null)
        {
            ITowerOperator component = tower.OperatorPrefab.GetComponent<ITowerOperator>();
            if (component.Range >= 8f)
            {
                rangeCircle.transform.localScale = new Vector2(2f, 2f);
            }
            else
            {
                rangeCircle.transform.localScale = new Vector2(component.Range * 2f, component.Range * 2f);
            }
        }
        Vector2 vector;
        while (true)
        {
            vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            towerObject.transform.position = vector;
            if (tower.OperatorPrefab != null)
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
                Destroy(overlay);
                Destroy(towerObject.gameObject);
                cancelled = false;
                shop.canBuild = true;
                yield break;
            }
            if (CanPlace(towerObject))
            {
                overlay.GetComponent<SpriteRenderer>().color = Green;
                if (Input.GetMouseButtonDown(0))
                {
                    TileZoneBank.Get(TileZoneType.Occupied).SetTile(towerObject.Center);
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
        shop.counter.Removeresources(tower.Price);
        towerObject.transform.position = new Vector3(Mathf.Floor(vector.x) + 0.5f, Mathf.Floor(vector.y) + 0.5f);
        if (tower.OperatorPrefab != null)
        {
            Instantiate(tower.OperatorPrefab, towerObject.transform).transform.position = towerObject.transform.position;
        }
        Destroy(overlay);
        shop.canBuild = true;
        if (collider != null)
        {
            towerObject.GetComponent<Collider2D>().enabled = true;
        }
    }
}
