using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BasePlacing : MonoBehaviour
{
    [SerializeField]
    GameObject baseObject, shopObject;

    GameObject newBase;

    [SerializeField]
    Tilemap map;

    bool build = false;
    // Start is called before the first frame update
    void Start()
    {
        shopObject.SetActive(false);
        newBase = Instantiate(baseObject);
        StartCoroutine(BuildBase());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            build = true;
        }
    }

    IEnumerator BuildBase()
    {
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = Mathf.Round(mousePos.x); 
            float y = Mathf.Round(mousePos.y);
            newBase.transform.position = new Vector2(x, y);

            if (build && CanPlace(x, y))
            {
                shopObject.SetActive(true);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    bool CanPlace(float x, float y)
    {
        if (map.HasTile(map.WorldToCell(new Vector2(x, y))) && map.HasTile(map.WorldToCell(new Vector2(x - 1, y))) && map.HasTile(map.WorldToCell(new Vector2(x, y - 1))) && map.HasTile(map.WorldToCell(new Vector2(x - 1, y - 1))))
        {
            return true;
        }
        build = false;
        return false;
    }
}
