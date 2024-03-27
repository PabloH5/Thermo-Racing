using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum WheelType
{
    Natural,
    Synthectic,
    Semisynthectic,
}

public class NewBehaviourScript : MonoBehaviour
{
    //public Thermometer thermometer;
    private Camera myCam;
    private Vector3 screenPoint;
    private float angleOffset;
    private Collider2D col;
    private float previousZRotation; // Almacena la rotaci�n en Z del frame anterior
    private float totalRotation = 0f; // Acumulador de la rotaci�n total

    [SerializeField]
    private float countDownToDowngradeSprite = 0f;
    private int currentIndexSprite = 0;

    [SerializeField]
    private WheelType wheelType;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> spritesNaturalWheel;
    [SerializeField]
    private List<Sprite> spritesSynthecthicWheel;
    [SerializeField]
    private List<Sprite> spritesSemisynthecticWheel;

    // Variable para almacenar el total de rotaciones completas
    private int totalCompleteTurns = 0;
    // Variable para recordar la última rotación total al completar una vuelta
    private float lastTotalRotationAtCompleteTurn = 0;


    private void Start()
    {
        myCam = Camera.main;
        col = GetComponent<Collider2D>();
        previousZRotation = transform.eulerAngles.z; // Inicializa con la rotación actual en Z

        LoadWheelSprites(currentIndexSprite);
    }

    private void Update()
    {
        // The wheel change state each 8 seconds if the user doesn't turn the wheel.
        countDownToDowngradeSprite -= Time.deltaTime;

        Debug.Log(currentIndexSprite);
        // The countdown doesn't have to lower than zero.
        if (countDownToDowngradeSprite < 0)
        {
            if (currentIndexSprite == 0)
            {
                LoadWheelSprites(0);
            }
            else
            {
                // Change to previous state
                LoadWheelSprites(currentIndexSprite - 1);

            }
            //totalCompleteTurns -= 10;
            countDownToDowngradeSprite = 0;
        }

        Debug.Log($"KBOOOOOOOOOM: {countDownToDowngradeSprite}");
        Vector3 mousePos = myCam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (col == Physics2D.OverlapPoint(mousePos))
            {
                //Debug.Log(Physics2D.OverlapPoint(mousePos));
                screenPoint = myCam.WorldToScreenPoint(transform.position);
                Vector3 vec3 = Input.mousePosition - screenPoint;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, vec3.x)) * Mathf.Rad2Deg;
                previousZRotation = transform.eulerAngles.z; // Actualiza la rotación previa en Z al comenzar a interactuar
            }
            //Debug.Log("HOLA");
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

                //Debug.Log($"Current {currentZRotation}");

                // Acumula la rotaci�n total
                totalRotation += rotationDifference;

                //Debug.Log(totalRotation);
                // Actualiza previousZRotation para el pr�ximo frame
                previousZRotation = currentZRotation;

                // Calcula el número de frame actual basado en la rotaci�n total
                int currentFrame = Mathf.FloorToInt(totalRotation / 360f);
                currentFrame = Mathf.Clamp(currentFrame, 0, 30); // Asegura que el frame est� entre 0 y 30

                //Debug.Log(currentFrame);

                // Aqu� puedes implementar la l�gica para actualizar tu term�metro
                // Por ejemplo: thermometer.Update(totalRotation);

                CheckTotalWheelTurns(totalRotation);
            }
        }

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

    private void CheckTotalWheelTurns(float totalRotation)
    {
        Debug.Log(totalCompleteTurns);

        if (totalRotation - lastTotalRotationAtCompleteTurn >= 360)
        {
            // Incrementa el contador de vueltas completas
            totalCompleteTurns++;
            // Actualiza la última rotación total al completar una vuelta
            lastTotalRotationAtCompleteTurn += 360;
            Debug.Log("Vuelta completa: " + totalCompleteTurns);

            if (totalCompleteTurns >= 0 && totalCompleteTurns <= 10)
            {
                LoadWheelSprites(0);
                countDownToDowngradeSprite = 8;
            }
            else if (totalCompleteTurns >= 11 && totalCompleteTurns <= 20)
            {
                countDownToDowngradeSprite = 8;
                LoadWheelSprites(1);
            }
            else if (totalCompleteTurns >= 21 && totalCompleteTurns <= 29)
            {
                countDownToDowngradeSprite = 8;
                LoadWheelSprites(2);
            }
            else
            {
                Debug.Log("EXPLOTAAAAAAAAAAAA");
            }
        }


    }
}