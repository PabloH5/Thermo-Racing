using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        initialPos = transform.position;
    }

    public void ToInitialState()
    {
        transform.position = initialPos;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        canDrag = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canDrag)
        {
            canvasGroup.blocksRaycasts = true;
            ToInitialState();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Pos"))
        {
            transform.position = autoPos.transform.position;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            canDrag = false;
        }
    }
}
