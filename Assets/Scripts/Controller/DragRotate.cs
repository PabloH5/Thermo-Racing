using System.Collections.Generic;
using UnityEngine;

public class DragRotate : MonoBehaviour
{
    private Camera myCam;
    private Vector3 screenPoint;
    private float angleOffset;
    private Collider2D col;
    private float previousZRotation; 
    private float _totalRotation;
    public float totalRotation
    {
        get { return _totalRotation; }
        set { _totalRotation = value; }
    }
    private float currentRotation;

    [SerializeField] private float countDownToDowngradeSprite;
    // [SerializeField] private TextMeshProUGUI debugSeconds;
    private int currentIndexSprite = 0;

    [SerializeField] private WheelType wheelType;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> spritesNaturalWheel;
    [SerializeField] private List<Sprite> spritesSynthecthicWheel;
    [SerializeField] private List<Sprite> spritesSemisynthecticWheel;

    private int _totalCompleteTurns;
    public int totalCompleteTurns
    {
        get { return _totalCompleteTurns; }
        set { _totalCompleteTurns = value; }
    }
    private int currentTurns;
    private float lastTotalRotationAtCompleteTurn = 0;
    private bool _isCountDownFinished;
    [SerializeField] private Thermometer thermometer;


    private void Start()
    {
        myCam = Camera.main;
        col = GetComponent<Collider2D>();
        previousZRotation = transform.eulerAngles.z; // Inicializa con la rotación actual en Z
        countDownToDowngradeSprite = 5f;
        totalRotation = 0;

        LoadWheelSprites(currentIndexSprite);
    }

    private void Update()
    {
        _isCountDownFinished = CalculateTheCountDown();

        Vector3 mousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (col == Physics2D.OverlapPoint(mousePos))
            {
                screenPoint = myCam.WorldToScreenPoint(transform.position);
                Vector3 vec3 = Input.mousePosition - screenPoint;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
                previousZRotation = transform.eulerAngles.z;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (col == Physics2D.OverlapPoint(mousePos))
            {
                Vector3 vec3 = Input.mousePosition - screenPoint;
                float angle = Mathf.Atan2(vec3.y, vec3.x) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);

                // Calcula la diferencia de rotación respecto al frame anterior
                float currentZRotation = transform.eulerAngles.z;
                float rotationDifference = Mathf.Abs(Mathf.DeltaAngle(currentZRotation, previousZRotation));

                totalRotation += rotationDifference;
                previousZRotation = currentZRotation;

                int currentFrame = Mathf.FloorToInt(totalRotation / 360f);
                currentFrame = Mathf.Clamp(currentFrame, 0, 30); 
                thermometer.SwitchSprite(currentFrame);
                // CheckTotalWheelTurns();
            }
        }

        if (_isCountDownFinished == true)
        {
            totalRotation = currentRotation;
            Debug.Log($"Rotations completed current after countdown: {totalRotation}");
            totalCompleteTurns = currentTurns;
            Debug.Log($"Turns completed current after countdown: {totalCompleteTurns}");
        }

        Debug.Log(_isCountDownFinished);

        CheckTotalWheelTurns();
    }

    private void CheckTotalWheelTurns()
    {
        if (totalRotation - lastTotalRotationAtCompleteTurn >= 360)
        {
            // Incrementa el contador de vueltas completas
            totalCompleteTurns++;
            // Actualiza la última rotación total al completar una vuelta
            lastTotalRotationAtCompleteTurn += 360;
            Debug.Log("Rotación Completa: " + lastTotalRotationAtCompleteTurn);
            Debug.Log("Vuelta completa: " + totalCompleteTurns);

            UpdateWheelState();
        }
    }

    private void UpdateWheelState()
    {
        if (totalCompleteTurns >= 0 && totalCompleteTurns < 11)
        {
            LoadWheelSprites(0);
            currentIndexSprite = 0;
        }
        else if (totalCompleteTurns >= 11 && totalCompleteTurns < 21)
        {
            LoadWheelSprites(1);
            currentIndexSprite = 1;
        }
        else if (totalCompleteTurns >= 21 && totalCompleteTurns <= 29)
        {
            LoadWheelSprites(2);
            currentIndexSprite = 2;              
        }
        else
        {
            Debug.Log("EXPLOTAAAAAAAAAAAA");
        }
        countDownToDowngradeSprite = 5; // Resetear el contador después de actualizar el estado.
    }

    private void LoadWheelSprites(int currentIndexSprite)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load the first element of sprite list in accordance with wheel type.
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

    private int DecreaseNumberOfTurns(int currentTurn)
    {
        return currentTurn > 0 ? currentTurn-=1 : currentTurn = 0;
    }

    private float DecreaseNumberOfRotations(float currentRotation)
    {
        if (currentRotation > 0f)
        {
            float remainder = currentRotation % 360;
            if (remainder != 0)
            {
                currentRotation += 360 - remainder;
            }
            currentRotation -= 720f;
        }
        else
        {
            currentRotation = 0f;
        }
        return currentRotation;
    }

    private int CalculateCurrentTurn()
    {
        // Debug.Log($"Current total turns before decrease: {totalCompleteTurns}");
        int numberTurnsAfterDecrease = DecreaseNumberOfTurns(totalCompleteTurns);
        totalCompleteTurns = numberTurnsAfterDecrease;
        // Debug.Log($"Current total turns after decrease: {totalCompleteTurns}");
        return totalCompleteTurns;
    }

    private float CalculateCurrentRotation()
    {
        Debug.Log($"Current total rotations before decrease: {totalRotation}");
        float numberRotationsAfterDecrease = DecreaseNumberOfRotations(totalRotation);
        totalRotation = numberRotationsAfterDecrease;
        Debug.Log($"Current total rotations after decrease: {totalRotation}");
        return totalRotation;
    }

    private bool CalculateTheCountDown()
    {
        // The wheel change state each 8 seconds if the user doesn't turn the wheel.
        countDownToDowngradeSprite -= Time.deltaTime;
        // debugSeconds.text = $"Seconds: {countDownToDowngradeSprite.ToString()}"; 

        // The countdown doesn't have to lower than zero.
        if (countDownToDowngradeSprite < 0)
        {
            _isCountDownFinished = true;
            if (currentIndexSprite == 0)
            {
                LoadWheelSprites(0);
                currentTurns = CalculateCurrentTurn();
                currentRotation = CalculateCurrentRotation();
            }
            else
            {
                if (totalCompleteTurns == 10) { LoadWheelSprites(0); } 
                else if (totalCompleteTurns == 20) { LoadWheelSprites(1); }
                
                currentTurns = CalculateCurrentTurn();
                currentRotation = CalculateCurrentRotation();
            }
            countDownToDowngradeSprite = 5;
        }
        else 
        {
            _isCountDownFinished = false;
        }
        return _isCountDownFinished;
    }

}