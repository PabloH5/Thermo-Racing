using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Bomb Air <c>class</c> when update the position and the touches amount
/// </summary>
public class TouchAirBomb : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField]
    private CheckWheelsController controller;
    [SerializeField] private ConstantBankUpdate constantBankUpdateScript;
    /// <value>
    /// Handle Bar Gameobject
    /// </value>
    [Tooltip("Asign the Handle Bar Game Object")]
    [SerializeField]
    private GameObject handleBar;
    /// <value>
    /// New positon when the handle bar is pushed
    /// </value>
    [Tooltip("Asign the new value of the handle bar position")]
    [SerializeField]
    private Vector3 newPosHB;

    /// <value>
    /// The initial positon when the handle bar isn't pushed
    /// </value>
    private Vector3 initPosHB;

    /// <value>
    /// The main camera of the current scene
    /// </value>
    private Camera mainCamera;
    /// <value>
    /// The Rect Transform Component of Handle Bar
    /// </value>
    private RectTransform rectTransform;

    /// <value>
    /// The number of touches carried by the user
    /// </value>
    private int touchCount;

    /// <summary>
    /// Singleton <c>method</c> for <c>touchCount</c> private variable
    /// </summary>
    /// <value>The number of touches carried by the user</value>
    public int TouchCount
    {
        get { return touchCount; }
        set { touchCount = value; }
    }

    [SerializeField]
    private float coolDown;
    private float update;

    public UnityEvent feedbackAudioPositiveEvent;
    public UnityEvent updateConstantBank;
    public UnityEvent explosionFeedbackEvent;

    private bool _CanInteract = true;

    /// <summary>
    /// Start <c>method</c> for initialize values
    /// </summary>
    void Start()
    {
        initPosHB = transform.localPosition; //Initialize the initial position of the Handle Bar
        mainCamera = Camera.main; //Initialize Main Camera of the scene
        rectTransform = handleBar.GetComponent<RectTransform>();//Equalize the Rect Transform Component to a variable

        feedbackAudioPositiveEvent.AddListener(() =>
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ARLLManager>().ActivateGoodAudioBehaviour();
        });

        updateConstantBank.AddListener(() =>
        {
            constantBankUpdateScript.GetComponent<ConstantBankUpdate>().UpdateVolumme(0.025f);
        });

        explosionFeedbackEvent.AddListener(() =>
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ARLLManager>().ActiveExplosion();
        });

    }
    /// <summary>
    /// Update <c>method</c> for update and compare values 
    /// </summary>
    void Update()
    {
        if (_CanInteract)
        {
            update += Time.deltaTime;
            if (Input.touchCount > 0) //Verify if user touch the screen
            {
                Touch touch = Input.GetTouch(0); //save the current touch information in touch
                if (touch.phase == TouchPhase.Began) //compare the phase of touches in the screen with the last touch
                {
                    Vector2 pos = touch.position; //save de position of the touch
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos, mainCamera)) //compare if the pos of touch is in the RectTransform area
                    {
                        MoveHandleBar(); //Invoke MoveHandleBar
                        TouchCount++;// add 1 for each touch
                        controller.SwitchSpriteTH(TouchCount);
                        controller.SwitchSpriteWH(TouchCount);
                        update = 0.0f;
                        Debug.Log("Update Reload");
                    }
                }
            }

            if (touchCount >= 12 && touchCount < 23)
            {
                if (update >= 1.5 && update < 2)
                {
                    updateConstantBank.Invoke();
                    feedbackAudioPositiveEvent.Invoke();
                    _CanInteract = false;
                }
            }
            else if (touchCount > 29)
            {
                explosionFeedbackEvent.Invoke(); 
                touchCount = 0;
                controller.SwitchSpriteTH(0);
                controller.SwitchSpriteWH(0);
                controller.UpdateLocalScale();
            }

            if (update >= coolDown && TouchCount > 0)
            {
                controller.SwitchSpriteTH(TouchCount--);
                controller.SwitchSpriteWH(TouchCount--);
                update = 0.0f;
            }
        }
    }

    /// <summary>
    /// This method move the Handle Bar to different position of the current
    /// </summary>
    void MoveHandleBar()
    {
        //Compare if the current localposition is equal to the down position of Handle Bar
        if (transform.localPosition == newPosHB)
        {
            transform.localPosition = initPosHB;
        }
        else
        {
            transform.localPosition = newPosHB;
        }
    }
}
