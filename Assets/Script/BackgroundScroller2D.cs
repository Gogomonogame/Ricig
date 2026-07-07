using UnityEngine;

public class BackgroundScroller2D : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 2f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 size = spriteRenderer.size;

        size.y += scrollSpeed * Time.deltaTime;
        

        spriteRenderer.size = size;
    }
}