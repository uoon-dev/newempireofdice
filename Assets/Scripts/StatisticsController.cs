using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsController : MonoBehaviour
{
    [SerializeField] Text factor01 = null;
    [SerializeField] Text factor02 = null;
    [SerializeField] Text factor03 = null;
    [SerializeField] GameObject star01 = null;
    [SerializeField] GameObject star02 = null;
    [SerializeField] GameObject star03 = null;
    [SerializeField] GameObject buttons = null;
    float clearedBlockCount = 0;
    float ddackCount = 0;
    float turnCount = 0;
    int currentLevelNumber = 0;
    int savedLevelStarCount = 0;
    int getStarCount = 0;
    int levelCleared = 0;
    LevelLoader levelLoader;
    NewHeartController newHeartController;
    AfterPurchaseEffectController afterPurchaseEffectController;
    ResetDiceController resetDiceController;


    void Start()
    {
        Initialize();
        factor01.text = "0%";
        factor02.text = "0";
        factor03.text = "0";

        // star display image
        star01.transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
        star02.transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
        star03.transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;

        // star display text
        star01.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = false;
        star02.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = false;
        star03.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = false;

        // button display
        buttons.transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
        buttons.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = false;

        // for test
        // FindObjectOfType<LevelController>().WinLastBlock();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        newHeartController = FindObjectOfType<NewHeartController>();
        afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();

        currentLevelNumber = levelLoader.GetCurrentLevelNumber();
        savedLevelStarCount = PlayerPrefs.GetInt("LevelStar");
        levelCleared = PlayerPrefs.GetInt($"Level {currentLevelNumber}");
    }

    IEnumerator HandleRewardUI()
    {
        GameObject star01Image = star01.transform.GetChild(0).gameObject;
        GameObject star01Text = star01.transform.GetChild(1).gameObject;
        GameObject star02Image = star02.transform.GetChild(0).gameObject;
        GameObject star02Text = star02.transform.GetChild(1).gameObject;
        GameObject star03Image = star03.transform.GetChild(0).gameObject;
        GameObject star03Text = star03.transform.GetChild(1).gameObject;

        bool inTurnLimit = resetDiceController.GetTurnCount() <= 30;

        SetStarCount();
        SetRewardHeart();

        star01Image.GetComponent<Animator>().enabled = true;
        star01Text.GetComponent<Animator>().enabled = true;        

        if (clearedBlockCount == ddackCount)
        {
            yield return new WaitForSeconds(0.7f);
            star02Image.GetComponent<Animator>().enabled = true;
            star02Text.GetComponent<Animator>().enabled = true;
            star02Text.GetComponent<Text>().text = "딱뎀 100%";

            if (inTurnLimit)
            {
                yield return new WaitForSeconds(0.7f);
                star03Image.GetComponent<Animator>().enabled = true;
                star03Text.GetComponent<Animator>().enabled = true;

                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL)
                {
                    bool isHeartFullReward = false;
                    if (currentLevelNumber % 10 == 0 && levelCleared != 1)
                    {
                        isHeartFullReward = true;
                        yield return new WaitForSeconds(0.7f);
                        afterPurchaseEffectController.ShowScreen("3");
                    }
                    if (savedLevelStarCount < 3)
                    {
                        yield return new WaitForSeconds(isHeartFullReward ? 2f : 0.7f);
                        afterPurchaseEffectController.ShowScreen("2");
                    }
                }
            }
        } 
        else 
        {
            yield return new WaitForSeconds(0.7f);
            if (inTurnLimit) 
            {
                star02Image.GetComponent<Animator>().enabled = true;
                star02Text.GetComponent<Animator>().enabled = true;
                star02Text.GetComponent<Text>().text = "30턴 안에 클리어!";
            }
        }

        if (levelCleared != 1 && getStarCount < 3)
        {
            if (currentLevelNumber % 10 == 0)
            {
                Debug.Log("afterPurchaseEffectController.ShowScreen('3')");
                yield return new WaitForSeconds(0.7f);
                afterPurchaseEffectController.ShowScreen("3");
            }
        }        

        PlayerPrefs.SetInt($"Level {currentLevelNumber}", 1);
        PlayerPrefs.SetInt($"LevelStar {currentLevelNumber}", getStarCount);

        yield return new WaitForSeconds(getStarCount > 1 ? 1.5f : 0.7f);
        buttons.transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
        buttons.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = true;
        
        buttons.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = true;
        buttons.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = true;
    }

    public void SetStarCount()
    {
        bool inTurnLimit = resetDiceController.GetTurnCount() <= 30;
        getStarCount = 1;

        if (clearedBlockCount == ddackCount) 
        { 
            getStarCount = 2;
            if (inTurnLimit)
            {
                getStarCount = 3;
            }
        } else 
        {
            if (inTurnLimit)
            {
                getStarCount = 2;
            }
        }
    }

    public void SetRewardHeart()
    {
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.LEVEL) {
            if (currentLevelNumber % 10 == 0 && newHeartController.GetHeartAmount() < Constants.HEART_MAX_CHARGE_COUNT && levelCleared != 1) {
                newHeartController.AddHeartAmount(Constants.HEART_MAX_CHARGE_COUNT - newHeartController.GetHeartAmount());
            }
            if (getStarCount == 3 && savedLevelStarCount < 3)
            {
                newHeartController.AddHeartAmount(2);
            }
        }
    }

    public void UpdateFactor01()
    {
        clearedBlockCount++;
        ddackCount++;
        factor01.text = Mathf.Floor((ddackCount/clearedBlockCount)*100).ToString() + "%";
        factor02.text = clearedBlockCount.ToString();
    }

    public void UpdateFactor02()
    {
        clearedBlockCount++;
        factor01.text = Mathf.Floor((ddackCount / clearedBlockCount) * 100).ToString() + "%";
        factor02.text = clearedBlockCount.ToString();
    }

    public void UpdateFactor03()
    {
        turnCount++;
        factor03.text = (turnCount + 1).ToString();
    }

    public void UpdateStarsStatisticDisplay()
    {
        StartCoroutine(HandleRewardUI());
    }
}
