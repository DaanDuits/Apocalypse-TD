using UnityEngine;
using UnityEngine.Tilemaps;
using TowerDefense.Building;

namespace TowerDefense.Construction
{
    [RequireComponent(typeof(Tilemap))]
    public class TileZone : MonoBehaviour
    {
        [SerializeField] private TileBase _tile;
        [SerializeField] private TileZoneType _zoneType;

        private Tilemap _tilemap;

        private void Awake()
        {
            TileZoneBank.Register(_zoneType, this);
            _tilemap = GetComponent<Tilemap>();
        }

        public void SetTile(Vector3 position)
        {
            _tilemap.SetTile(_tilemap.WorldToCell(position), _tile);
        }
        public bool HasTile(Vector3 position)
        {
            return _tilemap.HasTile(_tilemap.WorldToCell(position));
        }

        public void SetTileMatrix2x2(TileMatrix2x2 matrix)
        {
            _tilemap.SetTile(_tilemap.WorldToCell(matrix.TopLeft), _tile);
            _tilemap.SetTile(_tilemap.WorldToCell(matrix.TopRight), _tile);
            _tilemap.SetTile(_tilemap.WorldToCell(matrix.BottomLeft), _tile);
            _tilemap.SetTile(_tilemap.WorldToCell(matrix.BottomRight), _tile);
        }
        public bool HasTileMatrix2x2(TileMatrix2x2 matrix)
        {
            return _tilemap.HasTile(_tilemap.WorldToCell(matrix.TopLeft)) &&
                _tilemap.HasTile(_tilemap.WorldToCell(matrix.TopRight)) &&
                _tilemap.HasTile(_tilemap.WorldToCell(matrix.BottomLeft)) &&
                _tilemap.HasTile(_tilemap.WorldToCell(matrix.BottomRight));
        }
        public bool HasAnyTileMatrix2x2(TileMatrix2x2 matrix)
        {
            return _tilemap.HasTile(_tilemap.WorldToCell(matrix.TopLeft)) ||
                _tilemap.HasTile(_tilemap.WorldToCell(matrix.TopRight)) ||
                _tilemap.HasTile(_tilemap.WorldToCell(matrix.BottomLeft)) ||
                _tilemap.HasTile(_tilemap.WorldToCell(matrix.BottomRight));
        }
    }
}
