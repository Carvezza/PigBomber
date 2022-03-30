using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class FarmerEntity : MovableEntity
{
    [SerializeField]
    protected CatchPigAbility _catchAbility;
    [SerializeField]
    private PigEntity _target;
    [SerializeField]
    private float _cleaningDuration;
    [SerializeField]
    private float _catchDuration;
    public event Action PigCatched;
    [SerializeField]
    [Range(1, 5)]
    private float _sightRange;
    [SerializeField]
    [Range(0.1f, 1)]
    private float _sightFrequency;
    Coroutine _clean;
    public PigEntity Target
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null)
            {
                _currentState = EntityState.Angry;
                _entityView.MakeAngry();
            }
        }
    }
    private void Start()
    {
        StartCoroutine(LookingForPig());
    }
    public override void Init(GameMap map, Vector2Int position, EntityFactory entityFactory)
    {
        base.Init(map, position, entityFactory);
        _catchAbility.Init(_gameMap);
        _moveAbility.Init(_gameMap);
    }
    public override void Update()
    {
        base.Update();
        if (_ability != null)
        {
            return;
        }
        //Dirty state paralyse for some time
        if (_currentState == EntityState.Dirty)
        {
            return;
        }
        if (Target == null)
        {
            //Wander randomly
            MovementDirection direction = (MovementDirection)Random.Range(0, 4);
            MoveTo(direction);
        }
        else
        {
            if (_catchAbility.IsPigNearBy(PositionOnMap, Target.PositionOnMap))
            {
                TryCatchPig();
            }
            //Pursue pig
            MovementDirection direction = GetDirectionFromDestination(Target.transform.position);
            MoveTo(direction);
        }
    }
    public void TryCatchPig()
    {
        _ability = _catchAbility;
        _catchAbility.Catched += (t) => PigCatched?.Invoke();
        _catchAbility.Catch(_catchDuration, PositionOnMap, Target);
    }
    public override void TakeDamage()
    {
        base.TakeDamage();
        Target = null;
        if (_clean != null)
        {
            StopCoroutine(_clean);
        }
        _clean = StartCoroutine(CleanDirt(_cleaningDuration));
    }
    IEnumerator CleanDirt(float duration)
    {
        yield return new WaitForSeconds(duration);
        _entityView.MakeCalm();
        _currentState = EntityState.Calm;
    }
    private void LookForPig(float distance)
    {
        PigEntity potentialTarget = _gameMap.GetEntitiesInRadius(PositionOnMap, distance).FirstOrDefault(t => t is PigEntity) as PigEntity;
        if (potentialTarget != null)
        {
            if (CheckSightLine(PositionOnMap, potentialTarget.PositionOnMap))
            {
                Target = potentialTarget;
            }
        }
    }
    private IEnumerator LookingForPig()
    {
        while (true)
        {
            if (_currentState == EntityState.Calm)
            {
                LookForPig(_sightRange);
            }
            yield return new WaitForSeconds(_sightFrequency);
        }
    }
    public bool CheckSightLine(Vector2Int origin, Vector2Int target)
    {
        if (origin.y == target.y)
        {
            int start = origin.x < target.x ? origin.x : target.x;

            for (int i = 0; i < Mathf.Abs(origin.x - target.x); i++)
            {
                if (_gameMap.IsOccuoiedByFeature(new Vector2Int(start + i, origin.y)))
                {
                    return false;
                }
            }
            return true;
        }
        if (origin.x == target.x)
        {
            int start = origin.y < target.y ? origin.y : target.y;

            for (int i = 0; i < Mathf.Abs(origin.y - target.y); i++)
            {
                if (_gameMap.IsOccuoiedByFeature(new Vector2Int(origin.x, start + i)))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
