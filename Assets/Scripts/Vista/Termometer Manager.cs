using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TermometerManager : MonoBehaviour
{
    [SerializeField] private int targetTime = 15;
    [SerializeField] private TextMeshProUGUI counterTimer;

    // Start is called before the first frame update
    private void Start() {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (targetTime > 0)
        {
            targetTime--;
            UpdateTimer(targetTime);
            yield return new WaitForSeconds(1); 
        }
    }

    private void UpdateTimer(int NumberToUpdate)
    {
        counterTimer.text = NumberToUpdate.ToString();
    }
}
