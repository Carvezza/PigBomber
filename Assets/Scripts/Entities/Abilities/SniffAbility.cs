using UnityEngine;
using System;
using System.Linq;

public class SniffAbility : EntityAbility
{
    public event Action<PigEntity> Sniffed;
    private Vector2Int _position;
    private float _radius;
    private PigEntity _pig;
    public void Sniff(float duration, Vector2Int position, float radius)
    {
        enabled = true;
        _startTime = Time.time;
        _duration = duration;
        _position = position;
        _radius = radius;
        _pig = null;
    }
    public override void GameUpdate()
    {
        if (Time.time > _startTime + _duration)
        {
            _pig = _map.GetEntitiesInRadius(_position, _radius).
                        FirstOrDefault(t => t is PigEntity) as PigEntity;
            OnSniffed();
        }
    }
    private void OnSniffed()
    {
        enabled = false;
        Sniffed?.Invoke(_pig);
        Sniffed = null;
    }
}
