using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SentenceValidation : MonoBehaviour
{
    private Vector2 initiPos;
    void Start()
    {
        initiPos = transform.position;
    }
    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag(this.gameObject.tag))
        {

        }
        else { }
    }
}
