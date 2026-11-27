using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TowerDefense.Building;
using TowerDefense.Construction;
using TowerDefense.Utility;

public class BuildWall : MonoBehaviour
{
    private Tilemap tileMap;

    private TowerShop shop;

    public GameObject cancelButton;

    public GameObject rotateInfo;

    private bool _cancelled;

    private bool CanPlace(Wall target)
    {
        if (!TileZoneBank.Get(TileZoneType.Build).HasTile(target.Center) || !TileZoneBank.Get(TileZoneType.Build).HasTile(target.Center - target.transform.right))
            return false;

        if (TileZoneBank.Get(TileZoneType.NoBuild).HasTile(target.Center) && TileZoneBank.Get(TileZoneType.NoBuild).HasTile(target.Center - target.transform.right))
            return false;
        if (TileZoneBank.Get(TileZoneType.MainBase).HasTile(target.Center) && TileZoneBank.Get(TileZoneType.MainBase).HasTile(target.Center - target.transform.right))
            return false;

        foreach (Wall wall in Registry<Wall>.Filter(w => w != target))
        {
            if (wall.Position == target.Position)
                return false;
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
        _cancelled = true;
    }

    public IEnumerator MoveBuildWall(WallBlueprint wall, bool isCopy)
    {
        return MoveBuildWall(wall, isCopy, Vector3.zero);
    }

    public IEnumerator MoveBuildWall(WallBlueprint wall, bool isCopy, Vector3 rotation)
    {
        cancelButton.SetActive(true);
        Button.ButtonClickedEvent cancelOnClick = cancelButton.GetComponent<Button>().onClick;
        cancelOnClick.AddListener(CancelBuilding);
        rotateInfo.SetActive(true);
        Wall wallObject = Instantiate(wall.WallPrefab);
        shop.canBuild = false;
        if (isCopy)
        {
            wallObject.transform.rotation = Quaternion.Euler(rotation);
        }
        BoxCollider2D collider = wallObject.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            wallObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        while (true)
        {
            Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wallObject.transform.position = new Vector3(Mathf.Floor(vector.x) + 0.5f, Mathf.Floor(vector.y) + 0.5f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                wallObject.transform.Rotate(0f, 0f, -90f);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                wallObject.transform.Rotate(0f, 0f, 90f);
            }
            if (Input.GetMouseButtonDown(1) || _cancelled)
            {
                cancelOnClick.RemoveAllListeners();
                cancelButton.SetActive(value: false);
                rotateInfo.SetActive(value: false);
                Destroy(wallObject.gameObject);
                _cancelled = false;
                shop.canBuild = true;
                yield break;
            }
            if (CanPlace(wallObject) && Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        cancelOnClick.RemoveAllListeners();
        cancelButton.SetActive(value: false);
        rotateInfo.SetActive(value: false);
        shop.canBuild = true;
        shop.counter.Removeresources(wall.Price);
        if (collider != null)
        {
            wallObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (shop.counter.CheckRemovedResources(wall.Price))
        {
            StartCoroutine(MoveBuildWall(wall, isCopy: true, wallObject.transform.rotation.eulerAngles));
        }
    }
}
