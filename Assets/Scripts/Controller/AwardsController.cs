using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AwardsController : MonoBehaviour
{
    [Header("Rewards")]
    [SerializeField] private ItemAwardSO itemAwards;
    [SerializeField] private Image imageReward;
    [SerializeField] private Text subtitle;
    [SerializeField] private Text descriptionWithAwards;
    [SerializeField] private Text descriptionNotAwards;
    [SerializeField] private string sceneToRedirectUser;

    public void GetRandomAward()
    {

        // TEMPORARY LOGGIN
        // ------------------------------------------------
        LoggedUser.LogInUser("2222222", "test");
        // ------------------------------------------------

        descriptionNotAwards.gameObject.SetActive(false);
        descriptionWithAwards.gameObject.SetActive(false);
        
        // 1. Get the possible items that user could win.
        List<WheelModel> possibleWheelsRewards = InventoryRawModel.GetPossibleWheelsRewards(LoggedUser.UserCode);
        List<ChassisModel> possibleChassisRewards = InventoryRawModel.GetPossibleChassisRewards(LoggedUser.UserCode);


        if (possibleChassisRewards.Count == 0 && possibleWheelsRewards.Count == 0)
        {
            Debug.Log("NO PUEDE GANAR RECOMPENSAS");
            subtitle.text = "Has obtenido todas las piezas";
            descriptionNotAwards.gameObject.SetActive(true);
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
        if (userAward.GetType() == typeof(ChassisModel))
        {
            // Casting the variable item
            ChassisModel chassisAward = (ChassisModel)userAward;


            // Search into Scriptable Object the chassis won.
            AwardInfo awardInfo = itemAwards.awards.Find(item =>item.awardType == ItemType.CHASSIS && item.awardId == chassisAward.chassis_id
            );


            // Change the sprite for the chassis won.
            imageReward.sprite = awardInfo.awardSprite;
            // Insert the new item in user inventory into DB.
            InventoryUser.InsertChassisAwardToInventory(LoggedUser.UserCode, awardInfo.awardId);
        }
        else
        {
            WheelModel wheelAward = (WheelModel)userAward;

            // Search into Scriptable Object the chassis won.
            AwardInfo awardInfo = itemAwards.awards.Find(item => item.awardType == ItemType.WHEEL && item.awardId == wheelAward.wheel_id
            );

            // Change the sprite for the wheel won.
            Debug.Log($"{awardInfo.awardName} - {awardInfo.awardSprite}");
            imageReward.sprite = awardInfo.awardSprite;
            // Insert the new item in user inventory into DB.
            InventoryUser.InsertWheelAwardToInventory(LoggedUser.UserCode, awardInfo.awardId);
        }
        descriptionWithAwards.gameObject.SetActive(true);
    }

    public void RedirectUser()
    {
        SceneManager.LoadScene(sceneToRedirectUser);
    }
}
