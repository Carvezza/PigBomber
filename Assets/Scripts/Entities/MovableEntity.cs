using UnityEngine;
using System;

public class MovableEntity : Entity
{
    [SerializeField]
    protected MoveAbility _moveAbility;
    [SerializeField]
    [Range(0.1f, 2f)]
    private float _moveSpeed;
    protected void MoveTo(MovementDirection direction)
    {
        Vector2Int mapDisplacement;
        EntityDirection entityDirection;
        switch (direction)
        {
            case MovementDirection.UP: 
                mapDisplacement = Vector2Int.up;
                entityDirection = EntityDirection.UP;
                break;
            case MovementDirection.DOWN: 
                mapDisplacement = Vector2Int.down;
                entityDirection = EntityDirection.DOWN;
                break;
            case MovementDirection.LEFT: 
                mapDisplacement = Vector2Int.left;
                entityDirection = EntityDirection.LEFT;
                break;
            case MovementDirection.RIGHT:
                mapDisplacement = Vector2Int.right; 
                entityDirection = EntityDirection.RIGHT;
                break;
            default: 
                mapDisplacement = Vector2Int.zero;
                entityDirection = EntityDirection.RIGHT;
                break;
        }
        var destination = PositionOnMap + mapDisplacement;
        if (_gameMap.CanMoveTo(destination))
        {
            _gameMap.Reserve(destination);
            _entityView.SetDirection(entityDirection);
            _ability = _moveAbility;
            Vector3 worldDestination = _gameMap.MapToWorldCoordinates(destination);
            _moveAbility.MoveTo(worldDestination, _moveSpeed);
            _moveAbility.Reached += () =>
            {
                _gameMap.UpdateEntityPosition(PositionOnMap, PositionOnMap + mapDisplacement);
                PositionOnMap += mapDisplacement;
                _gameMap.UnReserve(destination);
            };
        }
    }
    protected MovementDirection GetDirectionFromDestination(Vector2 destination)
    {
        Vector2 generalDirection = destination - (Vector2)transform.position;
        if (Math.Abs(generalDirection.x) > Math.Abs(generalDirection.y))
        {
            if (generalDirection.x > 0)
            {
                return MovementDirection.RIGHT;
            }
            else
            {
                return MovementDirection.LEFT;
            }
        }
        else
        {
            if (generalDirection.y > 0)
            {
                return MovementDirection.UP;
            }
            else
            {
                return MovementDirection.DOWN;
            }
        }
    }
}
