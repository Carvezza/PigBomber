using UnityEngine;

public class MapFeature : MonoBehaviour
{
    [SerializeField]
    protected GameMap _gameMap;
    protected MapFeaturesFactory _originFactory;
    protected void DeSpawn()
    {
        _originFactory.Reclaim(this);
    }
}
