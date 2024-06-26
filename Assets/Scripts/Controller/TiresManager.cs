using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TiresManager : MonoBehaviour
{

    [SerializeField] private TiresDataBase tireDB;
    [SerializeField] private TextMeshProUGUI tireName;
    [SerializeField] private TextMeshProUGUI heat;
    // [SerializeField] private SpriteRenderer artworkSrpite;
    [SerializeField] private Image image;
    [SerializeField] private float weight;

    [SerializeField]
    private CheckWheelsController controller;
    [SerializeField] ARLLManager aRLLManagerScript;
    private int selectedOption = 0;

    private WheelType wheelTypeTiresManager;
    [SerializeField]
    private WheelToInflate wheelInflate;

    // Start is called before the first frame update
    void Start()
    {
        updateTires(selectedOption);
    }

    public void NextOption()
    {
        selectedOption++;
        if (selectedOption >= tireDB.TiresCount)
        {
            selectedOption = 0;
        }

        updateTires(selectedOption);
    }

    public void BackOption()
    {
        selectedOption--;

        if (selectedOption < 0)
        {
            selectedOption = tireDB.TiresCount - 1;
        }
        updateTires(selectedOption);
    }
    
    private void updateTires(int selectedOption)
    {
        Tires tire = tireDB.GetTires(selectedOption);
        controller.WheelData = tire;
        // artworkSrpite.sprite = tire.tireSprite;
        weight = tire.weight;
        image.sprite = tire.tireSprite;
        tireName.text = tire.typetires;
        heat.text = "Cp = " + tire.specificHeat.ToString() + " J/(kg°C)";
        wheelInflate.SpritesWH = tire.inflateTireSprite;
        wheelTypeTiresManager = aRLLManagerScript.ParseWheelType(tireName.text);
        aRLLManagerScript.SetWheelTypeInController(wheelTypeTiresManager);
        Debug.Log($"Wheel Type {wheelTypeTiresManager} ");
    }

}