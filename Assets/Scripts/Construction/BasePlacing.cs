using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BasePlacing : MonoBehaviour
{
    public GameObject baseObject, shopObject, overlay;
    public Color Green, Red;

    GameObject newBase;

    public Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Level").transform.GetChild(1).GetComponent<Tilemap>();
        shopObject.SetActive(false);
        newBase = Instantiate(baseObject);
        StartCoroutine(BuildBase());
    }

    IEnumerator BuildBase()
    {
        GameObject newOverlay = Instantiate(overlay);
        newOverlay.transform.localScale = newOverlay.transform.localScale * 2;

        newOverlay.GetComponent<SpriteRenderer>().color = Green;

        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = Mathf.Round(mousePos.x); 
            float y = Mathf.Round(mousePos.y);
            newBase.transform.position = new Vector2(x, y);
            newOverlay.transform.position = new Vector2(x, y);

            if (CanPlace(x, y))
            {
                newOverlay.GetComponent<SpriteRenderer>().color = Green;
                if (Input.GetMouseButtonDown(0))
                {
                    shopObject.SetActive(true);
                    Destroy(newOverlay);
                    break;
                }
            }
            else
                newOverlay.GetComponent<SpriteRenderer>().color = Red;

            yield return null;
        }
    }

    bool CanPlace(float x, float y)
    {
        x += 0.5f;
        y += 0.5f;
        if (BuildWall.instance.noBuildArea.Any(p => p == new Vector2(x, y) || p == new Vector2(x - 1, y) || p == new Vector2(x - 1, y - 1) || p == new Vector2(x, y - 1)))
        {
            return false;
        }
        if (map.HasTile(map.WorldToCell(new Vector2(x, y))) && map.HasTile(map.WorldToCell(new Vector2(x - 1, y))) && map.HasTile(map.WorldToCell(new Vector2(x, y - 1))) && map.HasTile(map.WorldToCell(new Vector2(x - 1, y - 1))))
        {
            return true;
        }
        return false;
    }
}
