using UnityEngine;
using System;

public class MoveAbility : EntityAbility
{
    private Vector2 _start;
    private Vector2 _dest;
    public event Action Reached;

    public void MoveTo(Vector2 destination, float duration)
    {
        enabled = true;
        _startTime = Time.time;
        _dest = destination;
        _start = transform.position;
        _duration = duration;
    }
    public void Return()
    {
        if (IsActive)
        {
            _duration = Time.time - _startTime;
            _startTime = Time.time;
            _dest = _start;
            _start = transform.position;
        }
    }
    public override void GameUpdate()
    {
        if (Time.time > _startTime + _duration)
        {
            OnReached();
        }
        else
        {
            transform.position = Vector2.Lerp(_start, _dest, Time.time - _startTime);
        } 
    }
    private void OnReached()
    {
        transform.position = _dest;
        enabled = false;
        Reached?.Invoke();
        Reached = null;
    }
}
