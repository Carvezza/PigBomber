using UnityEngine;

public abstract class GameObjectFactory : ScriptableObject
{
    protected T CreateGameObjectInstance<T>(T prefab, Vector2Int mapCoord, GameMap map) where T : MonoBehaviour
    {
        if (map.IsOnMap(mapCoord))
        {
            T instance = Instantiate(prefab, map.MapToWorldCoordinates(mapCoord), Quaternion.identity);
            return instance;
        }
        return null;
    }
}
