using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameMap : MonoBehaviour
{
    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;
    private GridCell[,] _gridCells;
    [SerializeField]
    private Sprite _backGroundImage;
    [SerializeField]
    private SpriteRenderer _backGroundRenderer;
    [SerializeField]
    private Vector2 _lean;
    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private Vector2 _pacing;
    [SerializeField]
    private int _mapWorldZ;

    private Dictionary<Vector2Int, Entity> _entitiesOnMap;
    private Dictionary<Vector2Int, MapFeature> _featuresOnMap;
    private HashSet<Vector2Int> _reservedCells;

    public void Init(MapFeaturesFactory factory) 
    {
        _entitiesOnMap = new Dictionary<Vector2Int, Entity>();
        _featuresOnMap = new Dictionary<Vector2Int, MapFeature>();
        _reservedCells = new HashSet<Vector2Int>();
        _backGroundRenderer.sprite = _backGroundImage;
        _gridCells = new GridCell[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int ii = 0; ii < _height; ii++)
            {
                if (i % 2 == 1 && ii % 2 == 1)
                {
                    factory.GetFeature(GridCellFeature.Stone, new Vector2Int(i, ii), this);          
                    _gridCells[i, ii] = new GridCell(GridCellFeature.Stone);
                }
                else if ((i ==  8)&& (ii == 0) || (i == 4) && (ii == 8) || (i == 12) && (ii == 6))
                {
                    factory.GetFeature(GridCellFeature.Bush, new Vector2Int(i, ii), this);
                    _gridCells[i, ii] = new GridCell(GridCellFeature.Bush);
                }
                else
                {
                    _gridCells[i, ii] = new GridCell(GridCellFeature.Dirt);
                }
            }
        }
    }
    public Vector3 MapToWorldCoordinates(int mapX, int mapY) => new Vector3(mapX - _width / 2 + _lean.x * mapY + _offset.x + _pacing.x * mapX, mapY - _height / 2 + _lean.y * mapX + _offset.y + _pacing.y * mapY, _mapWorldZ);
    public Vector3 MapToWorldCoordinates(Vector2Int mapCoordinates) => MapToWorldCoordinates(mapCoordinates.x, mapCoordinates.y);
    public bool CanMoveTo(Vector2Int cell) => IsOnMap(cell) && 
                                              _gridCells[cell.x, cell.y].Feature != GridCellFeature.Stone && 
                                              !IsOccuoiedByEntity(cell) &&
                                              !IsReserved(cell);
    public bool IsOnMap(Vector2Int cell) => cell.x >= 0 && cell.x < _width && cell.y >= 0 && cell.y < _height;
    public bool IsOccuoiedByEntity(Vector2Int position) => _entitiesOnMap.ContainsKey(position);
    public bool IsOccuoiedByFeature(Vector2Int position) => _featuresOnMap.ContainsKey(position);
    public Entity GetEntity(Vector2Int position) => IsOccuoiedByEntity(position)?_entitiesOnMap[position]:null;
    public void AddEntity(Entity entity, Vector2Int position) => _entitiesOnMap.Add(position, entity);
    public void RemoveEntity(Vector2Int position) => _entitiesOnMap.Remove(position);
    public void UpdateEntityPosition(Vector2Int oldPosition, Vector2Int newPosition)
    {
        var entity = GetEntity(oldPosition);
        _entitiesOnMap.Add(newPosition, entity);
        _entitiesOnMap.Remove(oldPosition);
    }
    public IEnumerable<Entity> GetEntities() => _entitiesOnMap.Select(t => t.Value);
    public IEnumerable<Entity> GetEntitiesInRadius(Vector2Int origin, float radius)
    {
        return _entitiesOnMap.Where(t => Vector2Int.Distance(t.Value.PositionOnMap, origin) <= radius).
                              Select(t => t.Value);
    }
    public void AddFeature(MapFeature feature, Vector2Int position) => _featuresOnMap.Add(position, feature);
    public void RemoveFeature(Vector2Int position) => _featuresOnMap.Remove(position);
    public void Reserve(Vector2Int cell) => _reservedCells.Add(cell);
    public void UnReserve(Vector2Int cell) => _reservedCells.Remove(cell);
    public bool IsReserved(Vector2Int cell) => _reservedCells.Contains(cell);
    public void ClearReserved()
    {
        _reservedCells.Clear();
    }
}
