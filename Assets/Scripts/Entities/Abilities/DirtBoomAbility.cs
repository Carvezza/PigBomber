using UnityEngine;
using System;

public class DirtBoomAbility : EntityAbility
{
    private float _radius;
    public event Action Boom;
    private Vector2Int _origin;

    public void SetFuse(float duration, float radius, Vector2Int origin, GameMap gameMap)
    {
        _startTime = Time.time;
        _duration = duration;
        _map = gameMap;
        _radius = radius;
        _origin = origin;
    }
    public override void GameUpdate()
    {
        if (Time.time > _startTime + _duration)
        {
            //Generate some explosion effects

            //Apply damage to all
            foreach (Entity entity in _map.GetEntitiesInRadius(_origin, _radius))
            {
                entity.TakeDamage();
            }
            OnBoom();
        }
    }
    private void OnBoom()
    {
        enabled = false;
        Boom?.Invoke();
        Boom = null;
    }
}
