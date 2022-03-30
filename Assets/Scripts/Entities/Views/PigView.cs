using UnityEngine;

public class PigView : EntityView
{
    [SerializeField]
    ParticleSystem _particleSystem;

    public override void MakeDirty()
    {
        _particleSystem.Play(true);
    }
}
