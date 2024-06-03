using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class URLmanager : MonoBehaviour
{
    [SerializeField] Button PabloButton;
    [SerializeField] Button AndresButton;
    [SerializeField] Button HectorButton;
    [SerializeField] Button FelipeButton;
    [SerializeField] Button GersonButton;

    // Start is called before the first frame update
    void Start()
    {
        PabloButton.onClick.AddListener(() => {
            OpenLinkedIn("https://www.linkedin.com/in/jpablo-martinez/");
        });
        
        AndresButton.onClick.AddListener(() => {
            OpenLinkedIn("https://www.linkedin.com/in/andres-bonilla-galindo/");
        });

        HectorButton.onClick.AddListener(() => {
            OpenLinkedIn("https://www.linkedin.com/in/hector-f-romero/");
        });

        FelipeButton.onClick.AddListener(() => {
            OpenLinkedIn("https://www.linkedin.com/in/felipearistizabal/");
        });

        GersonButton.onClick.AddListener(() => {
            OpenLinkedIn("https://www.linkedin.com/in/gerson-lópez-solis/");
        });
    }

    private void OpenLinkedIn(string url)
    {
        Application.OpenURL(url);
    }
}
