using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TowerDefense.Building;

public class BuildWall : MonoBehaviour
{
    private Tilemap tileMap;

    private TowerShop shop;

    public List<Vector2> noBuildArea = new List<Vector2>();

    public static BuildWall instance;

    public GameObject cancelButton;

    public GameObject rotateInfo;

    private bool cancelled;

    private bool CanPlace(Structure wall, Vector2 gridPos)
    {
        GameObject[] array = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int num = (tileMap.HasTile(tileMap.WorldToCell(gridPos)) ? 1 : 0);
        int num2 = (tileMap.HasTile(tileMap.WorldToCell(gridPos - (Vector2)wall.transform.right)) ? 1 : 0);
        switch (num)
        {
            case 1:
                switch (num2)
                {
                    case 1:
                        if (noBuildArea.Any((Vector2 p) => p == gridPos) && noBuildArea.Any((Vector2 p) => p == gridPos - (Vector2)wall.transform.right))
                        {
                            return false;
                        }
                        break;
                    case 0:
                        return false;
                }
                break;
            case 0:
                switch (num2)
                {
                    case 0:
                        return false;
                    case 1:
                        return false;
                }
                break;
        }
        foreach (GameObject gameObject in array)
        {
            if ((gameObject != wall && gameObject.CompareTag("Wall") && gameObject.transform.GetChild(0).position == wall.transform.GetChild(0).position) || (gameObject.CompareTag("Base") && Vector2.Distance(gameObject.transform.position, wall.transform.GetChild(0).position) <= 1f))
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
        instance = this;
        GameObject[] array = GameObject.FindGameObjectsWithTag("NoBuild");
        for (int i = 0; i < array.Length; i++)
        {
            Transform transform = array[i].transform;
            for (float num = transform.position.x - transform.localScale.x / 2f + 0.5f; num <= transform.position.x + transform.localScale.x / 2f - 0.5f; num++)
            {
                for (float num2 = transform.position.y - transform.localScale.y / 2f + 0.5f; num2 <= transform.position.y + transform.localScale.y / 2f - 0.5f; num2++)
                {
                    noBuildArea.Add(new Vector2(num, num2));
                }
            }
        }
    }

    public void CancelBuilding()
    {
        cancelled = true;
    }

    public IEnumerator MoveBuildWall(Wall wall, bool isCopy)
    {
        return MoveBuildWall(wall, isCopy, Vector3.zero);
    }

    public IEnumerator MoveBuildWall(Wall wall, bool isCopy, Vector3 rotation)
    {
        cancelButton.SetActive(value: true);
        Button.ButtonClickedEvent cancelOnClick = cancelButton.GetComponent<Button>().onClick;
        cancelOnClick.AddListener(CancelBuilding);
        rotateInfo.SetActive(value: true);
        Structure wallObject = Instantiate(wall.WallPrefab);
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
            if (Input.GetMouseButtonDown(1) || cancelled)
            {
                cancelOnClick.RemoveAllListeners();
                cancelButton.SetActive(value: false);
                rotateInfo.SetActive(value: false);
                Object.Destroy(wallObject);
                cancelled = false;
                shop.canBuild = true;
                yield break;
            }
            if (CanPlace(wallObject, wallObject.transform.position) && Input.GetMouseButtonDown(0))
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
