using UnityEngine;

public class PigEntity : MovableEntity
{
    [SerializeField]
    private PlaceBombAbility _placeBomb;
    [SerializeField]
    private Camera _camera;
    public Camera Camera { get => _camera; set => _camera = value; }
    [SerializeField]
    [Range(0.1f, 1f)]
    private float _placeBombSpeed;

    public override void Init(GameMap map, Vector2Int position, EntityFactory entityFactory)
    {
        base.Init(map, position, entityFactory);
        _placeBomb.Init(_gameMap);
        _moveAbility.Init(_gameMap);
    }
    public override void Update()
    {
        base.Update();
        if (_ability != null)
        {
            return;
        }
        if (Input.touchCount == 1)
        {
            if (_ability == _moveAbility)
            {
                return;
            }
            Vector2 touchPosition = Input.GetTouch(0).position;
            if (touchPosition.x > Screen.width * 0.9 && touchPosition.y < Screen.height * 0.2)
            {
                PlaceBomp();
            }
            else
            {
                MovementDirection direction = GetDirectionFromDestination(Camera.ScreenToWorldPoint(touchPosition));
                MoveTo(direction);
            }
        }  
    }
    public void PlaceBomp()
    {
        _ability = _placeBomb;
        _placeBomb.Place(_placeBombSpeed, PositionOnMap, _originFactory);
    }
}
