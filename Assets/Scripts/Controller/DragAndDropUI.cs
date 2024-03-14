using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Rigidbody2D rb2D;
    private Collider2D itemCollider;
    [SerializeField]
    private Vector3 autoPos;

    [SerializeField]
    private bool canDrag = true;

    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
        rb2D = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<Collider2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(canDrag)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

     // Añade esto en tu inspector o busca dinámicamente

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
        }
    }
    private void Update() 
    {
        if(canDrag){
        //     if(rectTransform.transform.position.x >= 5 || rectTransform.transform.position.x >= 234 
        //     && rectTransform.transform.position.y <= 57 ||rectTransform.transform.position.y >= -111)
        //     {
        //         rectTransform.position = autoPos;
        //         rb2D.bodyType = RigidbodyType2D.Dynamic;
        //         rb2D.mass = 100;
        //         canDrag = false;
        //     }
        // }
        
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Pos")
        {
            transform.position = autoPos;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.mass = 100;
            canDrag = false;
        }
    }
}
