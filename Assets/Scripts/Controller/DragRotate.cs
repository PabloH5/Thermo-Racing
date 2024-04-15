using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragRotate : MonoBehaviour
{
    private Camera myCam; // Reference to the main camera
    private Vector3 screenPoint; // Position of the mouse on the screen
    private float angleOffset; // Offset angle for dragging
    private Collider2D col; // Collider of the object
    private float previousZRotation; // Previous rotation around the Z axis
    private float _totalRotation; // Total rotation of the object
    public float totalRotation
    {
        get { return _totalRotation; }
        set { _totalRotation = value; }
    }
    private float currentRotation; // Current rotation of the object
    private bool isDragging = false; // Flag indicating if the object is being dragged

    [SerializeField] private float countDownToDowngradeSprite; // Countdown to downgrade the sprite
    // [SerializeField] private TextMeshProUGUI debugSeconds; // Debug text for seconds
    private int currentIndexSprite = 0; // Index of the current sprite

    [SerializeField] private WheelType wheelType; // Type of wheel
    private SpriteRenderer spriteRenderer; // Sprite renderer component
    [SerializeField] private List<Sprite> spritesNaturalWheel; // List of sprites for the natural wheel
    [SerializeField] private List<Sprite> spritesSynthecthicWheel; // List of sprites for the synthetic wheel
    [SerializeField] private List<Sprite> spritesSemisynthecticWheel; // List of sprites for the semisynthetic wheel

    private int _totalCompleteTurns; // Total number of complete turns
    public int totalCompleteTurns
    {
        get { return _totalCompleteTurns; }
        set { _totalCompleteTurns = value; }
    }
    private bool _isCountDownFinished; // Flag indicating if the countdown is finished
    [SerializeField] private Thermometer thermometer; // Reference to the thermometer object

    private float lastInteractionTime = 1.5f;
    public UnityEvent feedbackPositiveEvent;

    private void Start()
    {
        myCam = Camera.main; // Get the main camera
        col = GetComponent<Collider2D>(); // Get the collider component
        previousZRotation = transform.eulerAngles.z; // Get the initial rotation around the Z axis
        countDownToDowngradeSprite = 3f; // Set the countdown timer
        totalRotation = 0; // Initialize total rotation
        spriteRenderer = GetComponent<SpriteRenderer>(); // Cache sprite renderer on start

        LoadWheelSprites(currentIndexSprite); // Load the initial sprites for the wheel

        feedbackPositiveEvent.AddListener(() => {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<ARLLManager>().ActivatePositiveFeedbackGUI();
        });   
    }

    private void Update()
    {
        if (totalCompleteTurns >= 19 && totalCompleteTurns <= 23)
        {
            lastInteractionTime -= Time.deltaTime;
            if (lastInteractionTime <= 0) { feedbackPositiveEvent.Invoke(); }
        }

        _isCountDownFinished = CalculateTheCountDown(); // Calculate the countdown

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosInitial = myCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myCam.WorldToScreenPoint(transform.position).z));

            if (col == Physics2D.OverlapPoint(mousePosInitial))
            {
                isDragging = true; // Set dragging flag to true
                screenPoint = myCam.WorldToScreenPoint(transform.position); // Get the position of the object on the screen
                Vector3 vec3 = Input.mousePosition - screenPoint;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg; // Calculate the angle offset for dragging
                previousZRotation = transform.eulerAngles.z; // Store the previous rotation around the Z axis
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // Set dragging flag to false when mouse button is released
            CalculateWinScenario();
        }

        if (isDragging)
        {
            Vector3 mousePos = myCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myCam.WorldToScreenPoint(transform.position).z));

            if (col == Physics2D.OverlapPoint(mousePos))
            {
                Vector3 vec3 = Input.mousePosition - screenPoint;
                float angle = Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg; // Calculate the angle based on mouse position
                transform.eulerAngles = new Vector3(0, 0, angle + angleOffset); // Set the rotation of the object

                float currentZRotation = transform.eulerAngles.z; // Get the current rotation around the Z axis
                float rotationDifference = Mathf.Abs(Mathf.DeltaAngle(currentZRotation, previousZRotation)); // Calculate the rotation difference

                UpdateTotalRotation(rotationDifference); // Update the total rotation of the object
            }
        }
    }

    // Update the total rotation of the object
    private void UpdateTotalRotation(float rotationDifference)
    {
        totalRotation += rotationDifference; // Update the total rotation
        previousZRotation = transform.eulerAngles.z; // Update the previous rotation

        // Calculate the total number of complete turns
        int newTotalCompleteTurns = Mathf.FloorToInt(totalRotation / 360);
        if (newTotalCompleteTurns != totalCompleteTurns)
        {
            totalCompleteTurns = newTotalCompleteTurns; // Update the total number of complete turns
            Debug.Log("Complete Turn: " + totalCompleteTurns); // Log the complete turn
            UpdateWheelState(); // Update the wheel state
        }

        // Update the thermometer sprite based on the total rotation
        if (thermometer != null)
        {
            int currentFrame = Mathf.FloorToInt(totalRotation / 360f);
            currentFrame = Mathf.Clamp(currentFrame, 0, 29); // Make sure the frame is within the limits
            thermometer.SwitchSprite(currentFrame);
        }
    }

    // Update the wheel state based on the total number of complete turns
    private void UpdateWheelState()
    {
        if (totalCompleteTurns >= 0 && totalCompleteTurns < 11)
        {
            LoadWheelSprites(0);
            currentIndexSprite = 0;
        }
        else if (totalCompleteTurns >= 11 && totalCompleteTurns < 25)
        {
            LoadWheelSprites(1);
            currentIndexSprite = 1;
        }
        else if (totalCompleteTurns >= 25 && totalCompleteTurns <= 29)
        {
            LoadWheelSprites(2);
            currentIndexSprite = 2;
        }
        else
        {
            Debug.Log("EXPLOSIONNNNNNNN");
        }
        countDownToDowngradeSprite = 5; // Reset the countdown timer after updating the state
    }

    // Update the thermometer sprite based on the total rotation
    private void UpdateThermometerBasedOnRotation()
    {
        if (thermometer != null)
        {
            int currentFrame = Mathf.FloorToInt(totalRotation / 360f);
            currentFrame = Mathf.Clamp(currentFrame, 0, 29); // Make sure the frame is within the limits
            thermometer.SwitchSprite(currentFrame);
        }
    }

    // Load the wheel sprites based on the current index
    private void LoadWheelSprites(int currentIndexSprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load the sprite based on the wheel type
        switch (wheelType)
        {
            case WheelType.Natural:
                spriteRenderer.sprite = spritesNaturalWheel[currentIndexSprite];
                break;
            case WheelType.Synthectic:
                spriteRenderer.sprite = spritesSynthecthicWheel[currentIndexSprite];
                break;
            case WheelType.Semisynthectic:
                spriteRenderer.sprite = spritesSemisynthecticWheel[currentIndexSprite];
                break;
            default:
                Debug.Log("Invalid wheel type");
                break;
        }
    }

    // Decrease the number of turns by one
    private int DecreaseNumberOfTurns(int currentTurn)
    {
        return currentTurn > 0 ? currentTurn - 1 : 0;
    }

    // Decrease the number of turns and update the rotations
    private void DecreaseTurnsAndRotations()
    {
        if (totalCompleteTurns > 0)
        {
            totalCompleteTurns = DecreaseNumberOfTurns(totalCompleteTurns); // Decrease the total number of complete turns by one
            totalRotation -= 360; // Decrease the total rotation by 360 degrees to reflect the loss of one complete turn
            totalRotation = Mathf.Max(0, totalRotation); // Make sure total rotation doesn't become negative
            Debug.Log($"Total Rotation after decreasing: {totalRotation}"); // Log the total rotation after decreasing
            Debug.Log($"Complete Turns after decreasing: {totalCompleteTurns}"); // Log the complete turns after decreasing
        }

        // Update the wheel state and thermometer based on the new total number of complete turns
        UpdateWheelState();
        UpdateThermometerBasedOnRotation();

    }

    // Calculate the current rotation
    private float CalculateCurrentRotation()
    {
        Debug.Log($"Current total rotations before decrease: {totalRotation}");

        // Assuming you want to reset totalRotation to reflect complete turns more accurately after some event, e.g., a penalty.
        // Here, you adjust totalRotation based on totalCompleteTurns
        totalRotation = totalCompleteTurns;

        Debug.Log($"Current total rotations after decrease: {totalRotation}");
        return totalRotation;
    }

    // Calculate the countdown timer
    private bool CalculateTheCountDown()
    {
        countDownToDowngradeSprite -= Time.deltaTime;

        if (countDownToDowngradeSprite < 0)
        {
            _isCountDownFinished = true;
            DecreaseTurnsAndRotations(); // Call the new function here
            countDownToDowngradeSprite = 3; // Reset the timer
        }
        else
        {
            _isCountDownFinished = false;
        }
        return _isCountDownFinished;
    }

    private void CalculateWinScenario()
    {
        lastInteractionTime -= Time.deltaTime;
        int totalTurns = totalCompleteTurns;
        if (lastInteractionTime == 0f) 
        {
            if (totalTurns >=19 || totalTurns <=23)
            {
                feedbackPositiveEvent.Invoke();
            }
        }
    }

}