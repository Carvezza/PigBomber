using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    protected EntityView _entityView;
    [SerializeField]
    protected EntityAbility _ability;
    [SerializeField]
    protected GameMap _gameMap;
    protected EntityFactory _originFactory;
    public Vector2Int PositionOnMap { get; protected set; }
    protected EntityState _currentState;

    public virtual void Init(GameMap map, Vector2Int position, EntityFactory entityFactory)
    {
        _entityView.Init();
        _entityView.SetDirection(EntityDirection.RIGHT);
        _gameMap = map;
        _originFactory = entityFactory;
        PositionOnMap = position;
        _currentState = EntityState.Calm;
    }
    public virtual void Update()
    {
        if (_ability != null)
        {
            if (_ability.IsActive)
            {
                _ability.GameUpdate();
            }
            else
            {
                _ability = null;
            }
        }
    }
    public virtual void TakeDamage()
    {
        _currentState = EntityState.Dirty;
        _entityView.MakeDirty();
    }   
    public virtual void DeSpawn()
    {
        _gameMap.RemoveEntity(PositionOnMap);
        _originFactory.Reclaim(this);
    }
}
