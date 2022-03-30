using System.Collections;
using UnityEngine;

public class DogEntity : MovableEntity
{
    [SerializeField]
    protected SniffAbility _sniffAbility;
    [SerializeField]
    [Range(1,5)]
    private float _sniffDistance;
    [SerializeField]
    [Range(1, 5)]
    private float _sniffSpeed;
    [SerializeField]
    [Range(1, 5)]
    private float _sniffCooldown;
    [SerializeField]
    private PigEntity _target;
    private float _nextSniffTime;
    [SerializeField]
    private float _cleaningDuration;

    private PigEntity Target 
    { 
        get => _target; 
        set
        {
            _target = value;
            if(_target != null)
            {
                _currentState = EntityState.Angry;
                _entityView.MakeAngry();
            }
        }
    }
    public override void Init(GameMap map, Vector2Int position, EntityFactory entityFactory)
    {
        base.Init(map, position, entityFactory);
        _sniffAbility.Init(_gameMap);
        _moveAbility.Init(_gameMap);
        _nextSniffTime = Time.time;
    }
    public override void Update()
    {
        base.Update();
        if (_ability != null)
        {
            return;
        }
        //Dirty state paralyse dog for some time
        if (_currentState == EntityState.Dirty)
        {
            return;
        }
        if (Target == null)
        {
            //Wander randomly
            if (Time.time > _nextSniffTime)
            {
                _nextSniffTime += _sniffCooldown;
                SniffPig();        
            }
            else
            {
                MovementDirection direction = (MovementDirection)Random.Range(0, 4);
                MoveTo(direction);
            }
        }
        else
        {
            //Pursue pig
            MovementDirection direction = GetDirectionFromDestination(Target.transform.position);
            MoveTo(direction);
        }
    }
    public void SniffPig()
    {
        _ability = _sniffAbility;
        _sniffAbility.Sniffed += (t) => Target = t;
        _sniffAbility.Sniff(_sniffSpeed, PositionOnMap, _sniffDistance);
    }
    public override void TakeDamage()
    {     
        Target = null;
        base.TakeDamage();
        StartCoroutine(CleanDirt(_cleaningDuration));
    }
    IEnumerator CleanDirt(float duration)
    {
        yield return new WaitForSeconds(duration);
        _entityView.MakeCalm();
        _currentState = EntityState.Calm;
    }
}
