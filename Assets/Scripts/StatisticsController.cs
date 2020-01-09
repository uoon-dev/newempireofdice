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
    LevelLoader levelLoader;

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
    }    

    IEnumerator HandleStarsAnimation()
    {
        GameObject star01Image = star01.transform.GetChild(0).gameObject;
        GameObject star01Text = star01.transform.GetChild(1).gameObject;
        GameObject star02Image = star02.transform.GetChild(0).gameObject;
        GameObject star02Text = star02.transform.GetChild(1).gameObject;
        GameObject star03Image = star03.transform.GetChild(0).gameObject;
        GameObject star03Text = star03.transform.GetChild(1).gameObject;
        var resetDiceController = FindObjectOfType<ResetDiceController>();

        int getStarCount = 0;
        int currentLevelNumber = levelLoader.GetCurrentLevelNumber();
        bool inTurnLimit = resetDiceController.GetTurnCount() <= 30;

        star01Image.GetComponent<Animator>().enabled = true;
        star01Text.GetComponent<Animator>().enabled = true;

        getStarCount = 1;

        yield return new WaitForSeconds(0.7f);
        if (clearedBlockCount == ddackCount)
        {
            getStarCount = 2;
            star02Image.GetComponent<Animator>().enabled = true;
            star02Text.GetComponent<Animator>().enabled = true;
            star02Text.GetComponent<Text>().text = "딱뎀 100%";

            if (inTurnLimit) 
            {
                getStarCount = 3;
                yield return new WaitForSeconds(0.7f);
                star03Image.GetComponent<Animator>().enabled = true;
                star03Text.GetComponent<Animator>().enabled = true;

                int levelCleared = PlayerPrefs.GetInt($"Level {currentLevelNumber}");
                int savedLevelStartCount = PlayerPrefs.GetInt($"LevelStar {currentLevelNumber}", 0);
                var HeartController = FindObjectOfType<HeartController>();

                if (levelLoader.GetCurrentSceneName() == "Level" && ((levelCleared != 1) || savedLevelStartCount<3)) {
                    HeartController.SetHeartAmount(HeartController.GetHeartAmount() + 2);
                    var afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
                    afterPurchaseEffectController.ShowScreen("2");
                }
                
            }
        } else {
            if (inTurnLimit) {
                getStarCount = 2;
                star02Image.GetComponent<Animator>().enabled = true;
                star02Text.GetComponent<Animator>().enabled = true;
                star02Text.GetComponent<Text>().text = "30턴 안에 클리어!";
            }
        }

        PlayerPrefs.SetInt($"Level {currentLevelNumber}", 1);
        PlayerPrefs.SetInt($"LevelStar {currentLevelNumber}", getStarCount);

        yield return new WaitForSeconds(1.5f);
        buttons.transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
        buttons.transform.GetChild(1).gameObject.GetComponent<Animator>().enabled = true;
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
        StartCoroutine(HandleStarsAnimation());
    }
}
