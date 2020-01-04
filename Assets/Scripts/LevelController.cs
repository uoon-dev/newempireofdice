using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelController : MonoBehaviour
{
    [SerializeField] float waitToSecond = 0.1f;
    [SerializeField] GameObject winLabel = null;
    [SerializeField] GameObject loseLabel = null;
    [SerializeField] GameObject buttonsInLoseScreen = null;
    private GameObject stageIntro = null;
    private GameObject stageTextObject = null;


    void Start()
    {
        FindObjects();
        winLabel.SetActive(false);
        loseLabel.SetActive(false);

        if (stageIntro != null)
            AnimateStageIntro();
    }

    public void FindObjects()
    {
        stageIntro = GameObject.Find("Stage Intro");
        stageTextObject = GameObject.Find("Stage Number");
    }


    public void AnimateStageIntro()
    {
        int levelNumber = LevelLoader.GetCurrentLevelNumber();
        stageTextObject.GetComponent<Text>().text = $"Stage {levelNumber.ToString()}";
        stageIntro.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.2f).SetDelay(0.5f).OnComplete(() => {
            stageIntro.transform.DOScale(new Vector3(0.4f, 0.4f, 0.4f), 0.25f);
            stageIntro.GetComponent<CanvasGroup>().DOFade(0, 0.25f);
        });
    }

    public void WinLastBlock()
    {
        StartCoroutine(HandleWinCondition());
    }

    IEnumerator HandleWinCondition()
    {
        yield return new WaitForSeconds(waitToSecond);
        FindObjectOfType<UIAlignController>().UpdateBackgroundImage();
        winLabel.SetActive(true);
        winLabel.GetComponent<Canvas>().sortingOrder = 13;
        if (BackGroundSoundController.instance != null)
            BackGroundSoundController.instance.StopPlay(BackGroundSoundController.BGM_NAME.GAME_BGM);
        if (EffectSoundController.instance != null)
        EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.FINISH_ONE_ROUND);


        int currentLevelNumber = LevelLoader.GetCurrentLevelNumber();
        int levelCleared = PlayerPrefs.GetInt($"Level {currentLevelNumber}");
        

        if (levelCleared == 0)
        {
            if (currentLevelNumber % 10 == 0) {
                var heartController = FindObjectOfType<HeartController>();
                if (heartController.GetHeartAmount() < 5) {
                    heartController.SetHeartAmount(5);
                }
            }
        }

        FindObjectOfType<StatisticsController>().UpdateStarsStatisticDisplay();

        int restrictedMapCount = FindObjectOfType<MapController>().GetRestrictedMapCount();
        if (currentLevelNumber % restrictedMapCount == 0)
        {
            PlayerPrefs.SetInt($"LevelCycled", 1);
        }        
    }

    public void HandleLoseCondition()
    {
        loseLabel.SetActive(true);
    }
}
