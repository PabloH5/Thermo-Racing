using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class THEController : MonoBehaviour
{
    [SerializeField]
    private ThermometerWheelsView thermometer;
    [SerializeField]
    private RandomSentence randomSentence;
    [SerializeField]
    private int totalSprites;
    [SerializeField]
    private Text timerTxt;

    [Tooltip("Put here Positive FeedBack")]
    [SerializeField]
    private GameObject positiveFeedBack;
    [Tooltip("Put here Negative FeedBack")]
    [SerializeField]
    private GameObject negativeFeedBack;
    private float timer = 15;
    private float changeInterval = 0.5f;
    private int lastSpriteIndex = -1;
    private bool isReady;
    public bool IsReady
    {
        get { return isReady; }
        set { isReady = value; }
    }

    void Update()
    {
        Temporizator();
    }
    private void StartGame()
    {
        if (IsReady)
        {
            Temporizator();
            // ShowSentences();
        }
    }
    private void Temporizator()
    {
        if (timer <= 0)
        {
            SendNegativeFdB();
        }
        else
        {
            timer -= Time.deltaTime;
            timer = Mathf.Max(timer, 0f);
        }

        int spriteIndex = Mathf.FloorToInt(timer / changeInterval) % totalSprites;
        timerTxt.text = Math.Round(timer).ToString();

        if (spriteIndex != lastSpriteIndex && timer < totalSprites)
        {
            SwitchThermoSprite(spriteIndex);
            lastSpriteIndex = spriteIndex;
        }
    }
    public void RewardTimer()
    {
        if (timer < 15f)
        {
            timer += 5f;
            timer = Mathf.Min(timer, 15f);
        }
    }
    public void ShowSentences()
    {
        int sentences = 0;
        if (sentences < randomSentence.SentenceAmount)
        {
            randomSentence.ActiveSentence(sentences);
            sentences++;
        }
    }
    private void SentenceValidation()
    {

    }
    private void SwitchThermoSprite(int sprite)
    {
        thermometer.SwitchImage(sprite);
    }
    private void SendPositiveFdB()
    {
        positiveFeedBack.SetActive(true);
    }
    private void SendNegativeFdB()
    {
        negativeFeedBack.SetActive(true);
    }
}
