using UnityEngine;

public class SpriteColliderAdjuster : MonoBehaviour
{
    [SerializeField] private float fixedRadius = 1.17f;
    [SerializeField] private Vector2 fixedOffset = new Vector2(0.01f, -0.15f);

    void Start()
    {
        AdjustColliderToSprite();
    }

    public void AdjustColliderToSprite()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius = fixedRadius;
            collider.offset = fixedOffset;
        }
        else
        {
            Debug.LogWarning("CircleCollider2D no encontrado en el objeto.");
        }
    }
}
