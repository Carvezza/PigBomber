using UnityEngine;

public class DirtyBomb : Entity
{
    [SerializeField]
    DirtBoomAbility _boomAbility;
    [SerializeField]
    [Range(1,5)]
    private float _radius;
    [SerializeField]
    [Range(1, 5)]
    private float _fuseTime;

    public override void Init(GameMap map, Vector2Int position, EntityFactory entityFactory)
    {
        _gameMap = map;
        _originFactory = entityFactory;
        PositionOnMap = position;
        _boomAbility.Init(_gameMap);
        _ability = _boomAbility;
        _boomAbility.SetFuse(_fuseTime, _radius, PositionOnMap, _gameMap);
        _boomAbility.Boom += DeSpawn;
    }
    public override void TakeDamage()
    {
        
    }
    public override void DeSpawn()
    {
        _originFactory.Reclaim(this);
    }
}
