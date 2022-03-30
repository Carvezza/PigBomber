using UnityEngine;

[CreateAssetMenu]
public class MapFeaturesFactory : GameObjectFactory
{
    [SerializeField]
    StoneFeature _stonePrefab;
    [SerializeField]
    BushFeature _bushPrefab;

    public MapFeature GetFeature(GridCellFeature type, Vector2Int position, GameMap map)
    {
        if (map.IsOccuoiedByEntity(position) || map.IsOccuoiedByFeature(position))
        {
            return null;
        }
        MapFeature feature;
        switch (type)
        {
            case GridCellFeature.Stone: feature = CreateGameObjectInstance<StoneFeature>(_stonePrefab, position, map); break;
            case GridCellFeature.Bush: feature = CreateGameObjectInstance<BushFeature>(_bushPrefab, position, map); break;
            case GridCellFeature.Dirt:
            default: feature = null; break;
        }
        if (feature != null)
        {
            map.AddFeature(feature, position);
        }
        return feature;
    }

    public virtual void Reclaim(MapFeature feature)
    {
        Destroy(feature.gameObject);
    }
}
