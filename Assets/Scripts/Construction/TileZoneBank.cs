using System.Collections.Generic;

namespace TowerDefense.Construction
{
    public enum TileZoneType
    {
        Occupied,
        NoBuild,
        Build,
        MainBase
    }
    public static class TileZoneBank
    {
        private static Dictionary<TileZoneType, TileZone> _zones = new Dictionary<TileZoneType, TileZone>();

        public static void Register(TileZoneType zoneType, TileZone instance)
        {
            _zones.TryAdd(zoneType, instance);
        }
        public static void Clear()
        {
            _zones.Clear();
        }

        public static TileZone Get(TileZoneType zoneType)
        {
            return _zones[zoneType];
        }
    }
}
