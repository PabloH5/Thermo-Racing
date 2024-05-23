using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.ParticleSystem;

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
    [SerializeField] private Text points;


    [SerializeField]
    private GameObject startGameBt;
    [SerializeField]
    private GameObject sentenceParent;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private VideoPlayer backgroundVideo;
    [Tooltip("Put here Positive FeedBack")]
    [SerializeField]
    private GameObject positiveFeedBack;
    [Tooltip("Put here Negative FeedBack")]
    [SerializeField]
    private GameObject negativeFeedBack;
    [SerializeField] private ParticleSystem explotionParticleSystem;
    private bool hasExploted = false;

    private float timer = 15;
    private float changeInterval = 0.5f;
    private int lastSpriteIndex = -1;
    private bool isReady = false;
    private int currentSentence;
    private int savedScore;
    private List<SentenseSO> sentenceList = new();
    private List<GameObject> sentencePrefab = new();

    private List<TEQuestionModel> questionList;

    [Space(10)]
    [Header("Rewards")]
    [SerializeField] private AwardsController awardController;

    public void Awake()
    {
        backgroundVideo.Prepare();
    }

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
        backgroundVideo.Play();
        backgroundVideo.Pause();
        backgroundVideo.frame = 0;
        // CurrentSentence = randomSentence.SentenceAmount;
        for (int i = 0; i < randomSentence.SentenceAmount; i++)
        {
            sentenceList.Add(randomSentence.ActiveSentence(i));
            GameObject prefab = Instantiate(sentenceList[i].SentenceSO, sentenceParent.transform.position, Quaternion.identity, sentenceParent.transform);
            prefab.SetActive(false);
            sentencePrefab.Add(prefab);
        }
        // Create an array with the question ids selected.
        int[] questionsSelectedIds = { sentenceList[0].IdSentence, sentenceList[1].IdSentence, sentenceList[2].IdSentence };
        // Fill the list with the question information from DB.
        questionList = TEQuestionModel.GetThreeTEQuestionsById(questionsSelectedIds);

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
        CurrentSentence = 0;
        ShowSentences(CurrentSentence, true);
        backgroundVideo.Play();
        startGameBt.SetActive(false);

    }
    private void Temporizator()
    {
        //Debug.Log($"-------VIDEO TIME: {backgroundVideo.time}");
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
            // Go back the background video 5 seconds.
            backgroundVideo.time = MathF.Max((float)backgroundVideo.time - 5, 0);
        }
    }
    public void ShowSentences(int sentences, bool isFirst)
    {
        if (sentences < randomSentence.SentenceAmount)
        {
            if (isFirst)
            {
                sentencePrefab[sentences].SetActive(true);
                setUpQuestionController.SetUpTEQuestion(questionList[sentences]);
            }
            else
            {
                sentencePrefab[sentences - 1].SetActive(false);
                for (int i = 0; i < setUpQuestionController.dragAndDropOP.Count; i++)
                {
                    setUpQuestionController.dragAndDropOP[i].ToInitialState();
                }
                sentencePrefab[sentences].SetActive(true);
                setUpQuestionController.SetUpTEQuestion(questionList[sentences]);
            }
            Debug.Log($"Active the {sentences} sentence");
        }
        else
        {
            #region WIN_THE_GAME
            // Show the positive feedback and the user win the minigame
            SendPositiveFdB();
            explotionParticleSystem.Play();
            IsReady = false;
            backgroundVideo.Stop();

            awardController.GetRandomAward();

            #endregion
        }
    }

    public bool CheckSentence(int sentenceScore)
    {
        savedScore += sentenceScore;
        if (savedScore >= sentenceList[currentSentence].TotalScore)
        {
            //! CHANGE THE NUMBER COUNTER
            CurrentSentence++;
            points.text = CurrentSentence.ToString("0");
            ShowSentences(currentSentence, false);
            savedScore = 0;
            Debug.Log("TU MALDITA MADRE");
            return true;
        }
        else
        {
            Debug.Log("Ni un brillo pelao");
            return false;
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
        //explotionSound.Play();
        //Debug.Log("BOOOM");
        if (hasExploted == false)
        {
            explotionParticleSystem.Play();
            hasExploted = true;
        }

        negativeFeedBack.SetActive(true);
    }
}