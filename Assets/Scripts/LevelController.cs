using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Analytics;

public class LevelController : MonoBehaviour
{
    [SerializeField] float waitToSecond = 0.1f;
    [SerializeField] GameObject winLabel = null;
    [SerializeField] GameObject loseLabel = null;
    [SerializeField] GameObject buttonsInLoseScreen = null;
    private GameObject stageIntro = null;
    private GameObject stageTextObject = null;
    LevelLoader levelLoader;
    NewHeartController newHeartController;


    void Start()
    {
        Initialize();
        winLabel.SetActive(false);
        loseLabel.SetActive(false);

        if (stageIntro != null)
            AnimateStageIntro();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        newHeartController = FindObjectOfType<NewHeartController>();
        stageIntro = GameObject.Find("Stage Intro");
        stageTextObject = GameObject.Find("Stage Number");        
    }


    public void AnimateStageIntro()
    {
        int levelNumber = levelLoader.GetCurrentLevelNumber();
        stageTextObject.GetComponent<Text>().text = $"Stage {levelNumber.ToString()}";

        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.TUTORIAL)
        {
            stageIntro.SetActive(false);    
        }
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
        var UIController = FindObjectOfType<UIAlignController>();
        UIController.UpdateBackgroundImage();
        UIController.UpdateBlocksNumberColor();

        winLabel.SetActive(true);
        winLabel.GetComponent<Canvas>().sortingOrder = 13;
        
        if (BackGroundSoundController.instance != null)
            BackGroundSoundController.instance.StopPlay(BackGroundSoundController.BGM_NAME.GAME_BGM);
        if (EffectSoundController.instance != null)
        EffectSoundController.instance.PlaySoundByName(EffectSoundController.SOUND_NAME.FINISH_ONE_ROUND);

        int currentLevelNumber = levelLoader.GetCurrentLevelNumber();

        FindObjectOfType<StatisticsController>().UpdateStarsStatisticDisplay();

        int restrictedMapCount = FindObjectOfType<MapController>().GetRestrictedMapCount();
        if (currentLevelNumber % restrictedMapCount == 0)
        {
            PlayerPrefs.SetInt($"LevelCycled", 1);
        }

        if (currentLevelNumber == 1 || currentLevelNumber == 0)
        {
            AnalyticsEvent.TutorialComplete();
        }
    }

    public void HandleLoseCondition()
    {
        int currentLevelNumber = levelLoader.GetCurrentLevelNumber();
        AnalyticsEvent.LevelFail(currentLevelNumber);
        loseLabel.SetActive(true);
    }
}
