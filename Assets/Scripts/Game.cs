using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Start and manage game
public class Game : MonoBehaviour
{
    [SerializeField]
    private GameMap _gameMap;
    [SerializeField]
    private EntityFactory _entityFactory;
    [SerializeField]
    private MapFeaturesFactory _featuresFactory;
    [SerializeField]
    private Camera _camera;
    void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        _gameMap.Init(_featuresFactory);
        SpawnEntities();
    }
    public void ReStartGame()
    {
        var entities = new List<Entity>(_gameMap.GetEntities());
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].DeSpawn();
        }
        _gameMap.ClearReserved();
        SpawnEntities();
    }
    private void SpawnEntities()
    {
        PigEntity pig = _entityFactory.GetEntity(EntityType.Pig, new Vector2Int(0, 3), _gameMap) as PigEntity;
        pig.Camera = _camera;
        _entityFactory.GetEntity(EntityType.Dog, new Vector2Int(8, 2), _gameMap);
        FarmerEntity farmer = _entityFactory.GetEntity(EntityType.Farmer, new Vector2Int(4, 6), _gameMap) as FarmerEntity;
        farmer.PigCatched += ReStartGame;
    }
}
