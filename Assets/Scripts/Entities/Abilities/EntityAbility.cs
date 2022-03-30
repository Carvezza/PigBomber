using UnityEngine;

public abstract class EntityAbility:MonoBehaviour
{
    [SerializeField]
    protected GameMap _map;
    public virtual void GameUpdate() { }
    public bool IsActive => enabled;
    protected float _startTime;
    protected float _duration;

    public virtual void Init(GameMap map) 
    {
        _map = map;
    }
}

