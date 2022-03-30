using UnityEngine;

[CreateAssetMenu]
public class EntityFactory : GameObjectFactory
{
    [SerializeField]
    PigEntity _pigPrefab;
    [SerializeField]
    DogEntity _dogPrefab;
    [SerializeField]
    FarmerEntity _farmerPrefab;
    [SerializeField]
    DirtyBomb _bombPrefab;

    public Entity GetEntity(EntityType type, Vector2Int position, GameMap map)
    {
        if ((type != EntityType.Bomb) &&
            (map.IsOccuoiedByEntity(position) || map.IsOccuoiedByFeature(position)))
        {
            return null;
        }
        Entity entity;
        switch (type)
        {
            case EntityType.Pig: entity = CreateGameObjectInstance<PigEntity>(_pigPrefab, position, map); break;
            case EntityType.Dog: entity = CreateGameObjectInstance<DogEntity>(_dogPrefab, position, map); break;
            case EntityType.Farmer: entity = CreateGameObjectInstance<FarmerEntity>(_farmerPrefab, position, map); break;
            case EntityType.Bomb: entity = CreateGameObjectInstance<DirtyBomb>(_bombPrefab, position, map); break;
            default: entity = null; break;
        }
        if (entity != null)
        {
            entity.Init(map, position, this);
            if (type != EntityType.Bomb)
            {
                map.AddEntity(entity, position);
            }   
        }
        return entity;
    }
    public virtual void Reclaim(Entity entity)
    {
        Destroy(entity.gameObject);
    }
}
