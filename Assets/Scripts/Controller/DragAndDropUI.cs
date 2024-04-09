using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Rigidbody2D rb2D;
    private Vector3 initialPos;

    [SerializeField]
    private GameObject autoPos;

    [SerializeField]
    private bool canDrag = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
        rb2D = GetComponent<Rigidbody2D>();
        initialPos = transform.localPosition;
    }

    public void ToInitialState()
    {
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = initialPos;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        canDrag = true;
        // this.enabled = false;
        // this.enabled = true;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            canvasGroup.blocksRaycasts = true;
            ToInitialState();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pos"))
        {
            transform.position = autoPos.transform.position;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            canDrag = false;
        }
    }
}
