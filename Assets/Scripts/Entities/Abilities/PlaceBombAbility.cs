using UnityEngine;
using System;

public class PlaceBombAbility : EntityAbility
{
    public event Action Placed;
    private EntityFactory _entityFactory;
    private Vector2Int _position;
    public void Place(float duration, Vector2Int position, EntityFactory factory)
    {
        enabled = true;
        _startTime = Time.time;
        _duration = duration;
        _entityFactory = factory;
        _position = position;
    }
    public override void GameUpdate()
    {
        if (Time.time > _startTime + _duration)
        {
            _entityFactory.GetEntity(EntityType.Bomb, _position, _map);
            OnPlaced();
        }
    }
    private void OnPlaced()
    {
        enabled = false;
        Placed?.Invoke();
        Placed = null;
    }
}
