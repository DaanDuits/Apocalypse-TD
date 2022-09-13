using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildWall : MonoBehaviour
{
    [SerializeField]
    GameObject Overlay;

    [SerializeField]
    Wall[] Walls;

    [SerializeField]
    Color Red, Green;

    bool canBuild = true;
    bool Left, Right = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canBuild)
        {
            StartCoroutine(MoveBuildWall(Walls[0]));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Right = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Left = true;
        }
    }

    IEnumerator MoveBuildWall(Wall wall)
    {
        GameObject wallObject = Instantiate(wall.WallPrefab);
        GameObject overlay = Instantiate(Overlay);
        canBuild = false;
        overlay.GetComponent<SpriteRenderer>().color = Green;
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wallObject.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
            overlay.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);

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

            if (GameObject.FindObjectsOfType<GameObject>().Any(c => c.name != "PlacingOverlay(Clone)" && c.name != wall.ObjectName && c.transform.position == new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f)))
            {
                overlay.GetComponent<SpriteRenderer>().color = Red;
            }
            else
            {
                overlay.GetComponent<SpriteRenderer>().color = Green;
            }

            if (Input.GetMouseButton(0) && !GameObject.FindObjectsOfType<GameObject>().Any(c => c.name != "PlacingOverlay(Clone)" && c.name != wall.ObjectName && c.transform.position == new Vector3(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f)))
            {
                wallObject.name = "Wall";
                wallObject.transform.position = new Vector2(Mathf.Floor(mousePos.x) + 0.5f, Mathf.Floor(mousePos.y) + 0.5f);
                Destroy(overlay);
                canBuild = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
