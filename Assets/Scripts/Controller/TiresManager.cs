using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TiresManager : MonoBehaviour
{

    public TiresDataBase tireDB;
    public TextMeshProUGUI tireName;
    public TextMeshProUGUI Calor;
    // public SpriteRenderer artworkSrpite;
    public Image image;
    public float weigh;

    [SerializeField]
    private CheckWheelsController controller;
    private int selectedOption = 0;

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
        weigh = tire.weight;
        image.sprite = tire.tireSprite;
        tireName.text = tire.typetires;
        Calor.text = "Cp = " + tire.specificHeat.ToString() + " J/(kg°C)";
    }

}