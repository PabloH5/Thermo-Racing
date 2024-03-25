using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerWheelsView : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> spritesTH;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Jump2Time(0);
        SwitchSpeed(0);
    }

    public void SwitchSprite(int spritePos)
    {
        spriteRenderer.sprite = spritesTH[spritePos];
    }

    public void SwitchSpeed(float speed)
    {
        anim.speed = speed;
    }
    public void Jump2Time(float moment)
    {
        anim.PlayInFixedTime(stateName: "ThermoWheels", layer: -1, fixedTime: moment);
    }

    // public IEnumerator DecreaseThermometer(int coolDown, float currentFrame)
    // {
    //     yield return new WaitForSeconds(coolDown);
    //     Jump2Time(currentFrame);
    //     SwitchSpeed(1);
    // }

    public float CurrentFrame()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0); // 0 es para la capa base
        float normalizedTime = stateInfo.normalizedTime;
        float animationLength = stateInfo.length; // Obtiene la duración de la animación
        float currentTime = normalizedTime * animationLength; // Calcula el tiempo actual en segundos
        return currentTime;
    }

}
