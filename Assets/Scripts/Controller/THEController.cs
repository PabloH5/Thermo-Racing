using System;
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
    private ThermoEcuacionadorController setUpQuestionController;
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
    [SerializeField]
    private GameObject startGameBt;
    [SerializeField]
    private GameObject sentenceParent;
    private float timer = 15;
    private float changeInterval = 0.5f;
    private int lastSpriteIndex = -1;
    private bool isReady = false;
    private int currentSentence;

    private List<SentenseSO> sentenceList = new();
    private List<GameObject> sentencePrefab = new();

    private List<TEQuestionModel> questionList;
    // private List<String> answerPrefab = new();
    public bool IsReady
    {
        get { return isReady; }
        set { isReady = value; }
    }
    public int CurrentSentence
    {
        get { return currentSentence; }
        set { currentSentence = value; }
    }
     void Start()
    {
        // CurrentSentence = randomSentence.SentenceAmount;
        for (int i = 0; i < randomSentence.SentenceAmount; i++)
        {
            sentenceList.Add(randomSentence.ActiveSentence(i));
            GameObject prefab = Instantiate(sentenceList[i].SentenceSO, new Vector2(1325, 985), Quaternion.identity, sentenceParent.transform);
            prefab.SetActive(false);
            sentencePrefab.Add(prefab);
        }
        // Create an array with the question ids selected.
        int[] questionsSelectedIds = { sentenceList[0].IdSentence, sentenceList[1].IdSentence, sentenceList[2].IdSentence };
        // Fill the list with the question information from DB.
        questionList = TEQuestionModel.GetThreeTEQuestionsById(questionsSelectedIds);
        Debug.Log(questionList[0].te_question_id);
        Debug.Log(questionList[1].te_question_id);
        Debug.Log(questionList[2].te_question_id);

    }
    void Update()
    {
        // Temporizator();
        if (IsReady)
        {
            Temporizator();
        }
    }
    public void StartGame()
    {
        IsReady = true;
        CurrentSentence = 1;
        ShowSentences(CurrentSentence);
        startGameBt.SetActive(false);
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
    public void ShowSentences(int sentences)
    {
        // int sentences = 0;
        if (sentences < randomSentence.SentenceAmount)
        {
            // randomSentence.ActiveSentence(sentences);
            // SentenseSO sentense1 = randomSentence.ActiveSentence(1);
            SentenseSO sentense1 = sentenceList[0];
            //setUpQuestionController.SetupTESentence(sentense1.IdSentence);
            sentencePrefab[0].SetActive(true);

            setUpQuestionController.SetUpTEQuestion(questionList[0]);

            // Instantiate(sentense1.SentenceSO, new Vector2(1325, 985), Quaternion.identity, sentenceParent.transform);
            UnityEngine.Debug.Log($"Active {sentences} sentences");
            // sentences++;
        }
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
