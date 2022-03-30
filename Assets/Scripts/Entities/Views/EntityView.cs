using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    protected Animator _animator;
    protected EntityDirection _direction;
    protected const string DIRECTION_X_KEY = "DirXFloat";
    protected const string DIRECTION_Y_KEY = "DirYFloat";
    protected const string CALM_TRIGGER_KEY = "MakeCalm";
    protected const string ANGRY_TRIGGER_KEY = "MakeAngry";
    protected const string DIRTY_TRIGGER_KEY = "MakeDirty";
    protected const string DEAD_TRIGGER_KEY = "MakeDead";

    public virtual void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void SetDirection(EntityDirection direction)
    {
        _direction = direction;
        switch (_direction)
        {
            case EntityDirection.UP:
                _animator.SetFloat(DIRECTION_X_KEY, 0f);
                _animator.SetFloat(DIRECTION_Y_KEY, 1f);
                break;
            case EntityDirection.DOWN:
                _animator.SetFloat(DIRECTION_X_KEY, 0f);
                _animator.SetFloat(DIRECTION_Y_KEY, -1f);
                break;
            case EntityDirection.LEFT:
                _animator.SetFloat(DIRECTION_X_KEY, -1f);
                _animator.SetFloat(DIRECTION_Y_KEY, 0f);
                break;
            case EntityDirection.RIGHT:
                _animator.SetFloat(DIRECTION_X_KEY, 1f);
                _animator.SetFloat(DIRECTION_Y_KEY, 0f);
                break;
            default:
                break;
        }
    }
    public virtual void MakeCalm() => _animator.SetTrigger(CALM_TRIGGER_KEY);
    public virtual void MakeAngry() => _animator.SetTrigger(ANGRY_TRIGGER_KEY);
    public virtual void MakeDirty() => _animator.SetTrigger(DIRTY_TRIGGER_KEY);
    public virtual void MakeDead() => _animator.SetTrigger(DEAD_TRIGGER_KEY);
}
