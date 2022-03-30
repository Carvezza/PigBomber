using UnityEngine;

public class EntityDistanceSorting : MonoBehaviour
{
    [SerializeField]
    private int modifier;
    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetSortingOrder(int order) => _spriteRenderer.sortingOrder = order;
    public void Update()
    {
        SetSortingOrder((int)(100 - transform.position.y) * 2 + modifier);
    }
}
