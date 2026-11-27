using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using TowerDefense.Building;
using TowerDefense.Construction;
using System.Xml.Serialization;

public class BuildBase : MonoBehaviour
{
    [SerializeField] private MainBase _basePrefab;
    public GameObject shopObject, overlay;
    public Color Green, Red;

    private MainBase _base;

    // Start is called before the first frame update
    private void Start()
    {
        shopObject.SetActive(false);
        _base = Instantiate(_basePrefab);
        StartCoroutine(MoveBuildBase());
    }

    private IEnumerator MoveBuildBase()
    {
        GameObject newOverlay = Instantiate(overlay);
        newOverlay.transform.localScale = newOverlay.transform.localScale * 2;

        newOverlay.GetComponent<SpriteRenderer>().color = Green;

        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = Mathf.Round(mousePos.x); 
            float y = Mathf.Round(mousePos.y);
            _base.transform.position = new Vector2(x, y);
            newOverlay.transform.position = new Vector2(x, y);

            _base.TileMatrix = new TileMatrix2x2(new Vector3(x + 0.5f, y + 0.5f));

            if (!CanPlace())
            {
                newOverlay.GetComponent<SpriteRenderer>().color = Red;
                yield return null;
                continue;
            }

            newOverlay.GetComponent<SpriteRenderer>().color = Green;

            if (Input.GetMouseButtonDown(0))
            {
                shopObject.SetActive(true);

                TileZoneBank.Get(TileZoneType.Occupied).SetTileMatrix2x2(_base.TileMatrix);
                TileZoneBank.Get(TileZoneType.MainBase).SetTileMatrix2x2(_base.TileMatrix);
                Destroy(newOverlay);
                break;
            }

            yield return null;
        }
    }

    private bool CanPlace()
    {
        if (TileZoneBank.Get(TileZoneType.NoBuild).HasAnyTileMatrix2x2(_base.TileMatrix))
            return false;
        if (TileZoneBank.Get(TileZoneType.Build).HasTileMatrix2x2(_base.TileMatrix))
            return true; 

        return false;
    }
}
