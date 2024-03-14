using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleView : MonoBehaviour
{
    [SerializeField]
    private Text txtBox;

    [SerializeField]
    private CheckWheelsController controller;

    private Animator animator;
    private AudioSource audioSource;
    private bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isOn = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Wheel") && isOn)
        {
            StartCoroutine(UpdateTxt(controller.WheelWeigh().ToString()));
            audioSource.Play();
        }
    }

    IEnumerator UpdateTxt(string text)
    {
        yield return new WaitForSeconds(.5f);
        txtBox.text = text;
    }

    public void TurnOnScale()
    {
        isOn = true;
        animator.SetBool("isOn", isOn);
    }
    public void CleanDisplay()
    {
        StartCoroutine(UpdateTxt("0000000"));
    }
}
