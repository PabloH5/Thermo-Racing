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
    [SerializeField] private ItemAwardSO itemAwards;
    [SerializeField] private Image imageReward;

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
        
        GetRandomAward();

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

            GetRandomAward();

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

    private void GetRandomAward()
    {
        // TEMPORARY LOGGIN
        // ------------------------------------------------
        LoggedUser.LogInUser("2222222", "test");
        // ------------------------------------------------
 

        // 1. Get the possible items that user could win.
        List<WheelModel> possibleWheelsRewards = InventoryRawModel.GetPossibleWheelsRewards(LoggedUser.UserCode);
        List<ChassisModel> possibleChassisRewards = InventoryRawModel.GetPossibleChassisRewards(LoggedUser.UserCode);


        if(possibleChassisRewards.Count == 0 && possibleWheelsRewards.Count == 0) {
            Debug.Log("NO PUEDE GANAR RECOMPENSAS");
            return;
        }

        // 2. Create a list for save all the possible items that can be awards.
        List<object> itemList = new List<object>();

        
        itemList.AddRange(possibleWheelsRewards.Cast<object>());
        itemList.AddRange(possibleChassisRewards.Cast<object>());

        // 3. Order the items in a random way.
        System.Random random = new System.Random();
        itemList = itemList.OrderBy(x => random.Next()).ToList();

        // 4. Obtain the user award
        var userAward = itemList.First();
       
        // 5. Verify if the award will be "CHASSIS" or "WHEEL"
        if(userAward.GetType() ==typeof( ChassisModel))
        {
            // Casting the variable item
            ChassisModel chassisAward = (ChassisModel)userAward;

            // Search into Scriptable Object the chassis won.
            AwardInfo awardInfo = itemAwards.awards.Find(item => item.awardType == ItemType.CHASSIS && item.awardId == chassisAward.chassis_id);

            // Change the sprite for the chassis won.
            imageReward.sprite = awardInfo.awardSprite;
            // Insert the new item in user inventory into DB.
            InventoryUser.InsertChassisAwardToInventory(LoggedUser.UserCode, awardInfo.awardId);
        }
        else
        {
            WheelModel wheelAward = (WheelModel)userAward;
            // Search into Scriptable Object the chassis won.
            AwardInfo awardInfo = itemAwards.awards.Find(item => item.awardType == ItemType.WHEEL && item.awardId == wheelAward.wheel_id);

            // Change the sprite for the wheel won.
            imageReward.sprite = awardInfo.awardSprite;
            // Insert the new item in user inventory into DB.
            InventoryUser.InsertWheelAwardToInventory(LoggedUser.UserCode,awardInfo.awardId);
        }
    }
}