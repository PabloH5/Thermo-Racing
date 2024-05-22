using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the behavior and display of a scale, including turning it on/off and updating the displayed weight.
/// This script should be attached to the GameObject representing the scale in the scene.
/// </summary>
public class ScaleView : MonoBehaviour
{
    [Header("Set in Inspector")]
    [Tooltip("UI Text component where the weight will be displayed.")]
    [SerializeField]
    private Text txtBox; // Reference to the UI Text component where the weight will be displayed.

    [Tooltip("The controller that provides the weight of the wheel.")]
    [SerializeField]
    private CheckWheelsController controller; // Reference to the controller that provides the weight of the wheel.

    [Tooltip("GameObject to show when the scale is off.")]
    [SerializeField]
    private GameObject offStateObject; // GameObject to show when the scale is off.

    [Tooltip("GameObject to show when the scale is on.")]
    [SerializeField]
    private GameObject onStateObject; // GameObject to show when the scale is on.

    [Tooltip("GameObject to show when wheel detected and scale is on.")]
    [SerializeField]
    private GameObject wheelDetectedObject; // GameObject to show when wheel detected and scale is on.

    private Animator animator; // Animator component to control scale animations.
    private AudioSource audioSource; // AudioSource component to play audio feedback.
    private bool isOn; // Indicates whether the scale is currently on or off.

    /// <summary>
    /// Start <c>method</c> for initialize values
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isOn = false;
        ToggleStateObjects(isOn); // Ensure initial state is set correctly
    }

    /// <summary>
    /// Provides public access to check or change the on/off state of the scale.
    /// </summary>
    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;
            ToggleStateObjects(isOn); // Toggle state objects based on the new state
        }
    }

    /// <summary>
    /// Detects collisions with wheels and updates the display if the scale is on, or resets the wheel to its initial state if the scale is off.
    /// </summary>
    /// <param name="other">Information about the collision and the colliding object.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Wheel") && isOn)
        {
            StartCoroutine(UpdateTxt(controller.WheelWeigh(0).ToString()));
            controller.HideArrows();
            audioSource.Play();

            // Desactiva el objeto offStateObject y activa el wheelDetectedObject
            onStateObject.SetActive(false);
            wheelDetectedObject.SetActive(true);
        }
        else if (!isOn)
        {
            controller.ToInitStateWheel();
            Debug.Log("Prendela");
        }
    }

    /// <summary>
    /// Coroutine to update the displayed weight with a slight delay.
    /// </summary>
    /// <param name="text">The weight value to display.</param>
    /// <returns>IEnumerator for coroutine handling.</returns>
    IEnumerator UpdateTxt(string text)
    {
        yield return new WaitForSeconds(0.5f);
        txtBox.text = text;
    }

    /// <summary>
    /// Turns the scale on, enabling it to display weights and play audio feedback.
    /// </summary>
    public void TurnOnScale()
    {
        IsOn = true;
        animator.SetBool("isOn", IsOn);
    }

    /// <summary>
    /// Resets the scale display to show a default value and resets the wheel to its initial state.
    /// </summary>
    public void CleanDisplay()
    {
        StartCoroutine(UpdateTxt("0000000"));
        controller.ToInitStateWheel();
    }

    /// <summary>
    /// Toggles the visibility of the state objects based on the scale state.
    /// </summary>
    /// <param name="state">True to show the onStateObject, false to show the offStateObject.</param>
    private void ToggleStateObjects(bool state)
    {
        if (offStateObject != null)
        {
            offStateObject.SetActive(!state);
        }

        if (onStateObject != null)
        {
            onStateObject.SetActive(state);
        }

        // Asegúrate de que el objeto wheelDetectedObject esté desactivado inicialmente
        if (wheelDetectedObject != null)
        {
            wheelDetectedObject.SetActive(false);
        }
    }
}