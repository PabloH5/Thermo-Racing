using System.Collections.Generic;
using UnityEngine;

public class ThermometerWheelsView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private UnityEngine.UI.Image image;

    [Tooltip("List of sprites of different states of the thermometer.")]
    [SerializeField]
    private List<Sprite> spritesTH;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void SwitchSprite(int spritePos)
    {
        spriteRenderer.sprite = spritesTH[spritePos];
    }

    public void SwitchImage(int spritePos)
    {
        image.sprite = spritesTH[spritePos];
    }

}
