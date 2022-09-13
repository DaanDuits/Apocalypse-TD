using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildTower : MonoBehaviour
{
    [SerializeField]
    GameObject Overlay;

    [SerializeField]
    Tower[] Towers;

    [SerializeField]
    Color Red, Green;

    bool canBuild = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && canBuild)
        {
            StartCoroutine(MoveBuildTower(Towers[0]));
        }
        if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && canBuild)
        {
            StartCoroutine(MoveBuildTower(Towers[1]));
        }
        if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && canBuild)
        {
            StartCoroutine(MoveBuildTower(Towers[2]));
        }
    }

    IEnumerator MoveBuildTower(Tower tower)
    {
        GameObject towerObject = Instantiate(tower.TowerPrefab);
        GameObject overlay = Instantiate(Overlay);
        canBuild = false;
        overlay.GetComponent<SpriteRenderer>().color = Green;
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            towerObject.transform.position = mousePos;
            overlay.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);

            if (GameObject.FindObjectsOfType<GameObject>().Any(c => c.name != "PlacingOverlay(Clone)" && c.transform.position == new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f)))
            {
                overlay.GetComponent<SpriteRenderer>().color = Red;
            }
            else
            {
                overlay.GetComponent<SpriteRenderer>().color = Green;
            }

            if (Input.GetMouseButton(0) && !GameObject.FindObjectsOfType<GameObject>().Any(c => c.name != "PlacingOverlay(Clone)" && c.transform.position == new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f)))
            {
                towerObject.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
                Destroy(overlay);
                canBuild = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
