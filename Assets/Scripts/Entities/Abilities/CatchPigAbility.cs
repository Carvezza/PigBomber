using UnityEngine;
using System;

public class CatchPigAbility : EntityAbility
{
    public event Action<PigEntity> Catched;
    public event Action<PigEntity> Escaped;
    private Vector2Int _position;
    private PigEntity _pig;
    public void Catch(float duration, Vector2Int position, PigEntity pig)
    {
        enabled = true;
        _startTime = Time.time;
        _duration = duration;
        _position = position;
        _pig = pig;
    }
    public override void GameUpdate()
    {
        if (Time.time > _startTime + _duration)
        {
            if (IsPigNearBy(_position, _pig.PositionOnMap))
            {
                OnCatched();
            }
            else
            {
                OnEscaped();
            }
        }
    }
    private void OnCatched()
    {
        enabled = false;
        Catched?.Invoke(_pig);
        Catched = null;
    }
    private void OnEscaped()
    {
        enabled = false;
        Escaped?.Invoke(_pig);
        Escaped = null;
    }
    public bool IsPigNearBy(Vector2Int position, Vector2Int pig)
    {
        return (position.x == pig.x && Mathf.Abs(position.y - pig.y) == 1) ||
               (position.y == pig.y && Mathf.Abs(position.x - pig.x) == 1);
    }
}
