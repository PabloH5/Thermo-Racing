using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropOP : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Rigidbody2D rb2D;
    private Vector3 initialPos;
    private int answerValue;
    private AudioSource audioSource;

    [SerializeField]
    private bool canDrag = true;
    [SerializeField]
    private GameObject rewardGo;
    [SerializeField]
    private THEController tHEController;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
        rb2D = GetComponent<Rigidbody2D>();
        answerValue = 5;
    }

    void Start()
    {
        initialPos = transform.localPosition;
    }
    
    public void ToInitialState()
    {
        canvasGroup.blocksRaycasts = true;
        transform.localPosition = initialPos;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
        canDrag = true;
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
        if (other.CompareTag(this.tag))
        {
            transform.position = other.transform.position;
            canDrag = false;
            tHEController.CheckSentence(answerValue);
            rewardGo.SetActive(true);
            rewardGo.GetComponent<Animator>().Play("RewardTHEanim", 0, 0.0f);
            audioSource.Play();
            tHEController.RewardTimer();
        }
    }
}
