using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
//using UnityEngine.Advertisements;

public static class AD_REWARD_TYPE
{
    public const string GET_ALL_DICES = "getAllDices";
    public const string LOAD_CLICKED_MAP = "LoadClickedMap";
    public const string LOAD_LEVEL_SCENE = "LoadLevelScene";
}

public class AdsController : MonoBehaviour
{
    private static bool isIntitialized;
    private string adsAppleId = "3259036";
    private string adsAndroidId = "3259037";
    private static string rewardType = "";
    private int targetLevel = 0;
    NewHeartController newHeartController;
    UIController UIController;
    NoDiceNoCoinController noDiceNoCoinController;
    ResetDiceController resetDiceController;
    MapController mapController;
    LevelLoader levelLoader;

    
    void Start()
    {
        Initialize();
        if(isIntitialized) return; 
        isIntitialized = true;
        InitializeAds();        
    }

    public void Initialize()
    {
        newHeartController = FindObjectOfType<NewHeartController>();
        UIController = FindObjectOfType<UIController>();
        noDiceNoCoinController = FindObjectOfType<NoDiceNoCoinController>();
        resetDiceController = FindObjectOfType<ResetDiceController>();
        mapController = FindObjectOfType<MapController>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void InitializeAds()
    {
        Yodo1U3dAds.InitializeSdk();
        SetListners();
    }

   
    public void SetListners()
    {
        SetRewaredAdsListner();
        SetInterstitialAdsListner();
    }

    public void SetInterstitialAdsListner() {
        Yodo1U3dSDK.setInterstitialAdDelegate((Yodo1U3dConstants.AdEvent adEvent, string error) => {
            Debug.Log("InterstitialAdDelegate:" + adEvent + "\n" + error);
            switch (adEvent)
            {
                case Yodo1U3dConstants.AdEvent.AdEventClick:
                    AnalyticsEvent.AdStart(false);
                    Debug.Log("Interstital ad has been clicked.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventClose:
                    //LevelLoader.LoadClickedMap(targetLevel);
                    Debug.Log("Interstital ad has been closed.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowSuccess:
                    //LevelLoader.LoadClickedMap(targetLevel);
                    AnalyticsEvent.AdComplete(false);
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowFail:
                    Debug.Log("Interstital ad has been show failed, the error message:" + error);
                    break;
            }
        });
    }

    public void SetRewaredAdsListner()
    {
        Yodo1U3dSDK.setRewardVideoDelegate((Yodo1U3dConstants.AdEvent adEvent, string error) =>
        {
            Debug.Log("RewardVideoDelegate:" + adEvent + "\n" + error);
            switch (adEvent)
            {
                case Yodo1U3dConstants.AdEvent.AdEventClick:
                    AnalyticsEvent.AdStart(true);
                    Debug.Log("Rewarded video ad has been clicked.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventClose:
                    Debug.Log("Rewarded video ad has been closed.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowSuccess:
                    AnalyticsEvent.AdComplete(true);
                    OnRewaredVideoSuccess();
                    Debug.Log("Rewarded video ad has shown successful.");
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventShowFail:
                    Debug.Log("Rewarded video ad show failed, the error message:" + error);
                    break;
                case Yodo1U3dConstants.AdEvent.AdEventFinish:
                    Debug.Log("Rewarded video ad has been played finish, give rewards to the player.");
                    break;
            }
        });
    }

    public void PlayAds(string reward)
    {
        if (Yodo1U3dAds.VideoIsReady()) {   
            rewardType = reward;
            Yodo1U3dAds.ShowVideo();
        }
    }

    public void PlayInterstitialAdsWithLevel(string reward, int level) {
        if (Yodo1U3dAds.InterstitialIsReady())
        {
            targetLevel = level;
            Yodo1U3dAds.ShowInterstitial();
        }
    }

    public void PlayInterstitialAds(string reward) {
        if (Yodo1U3dAds.InterstitialIsReady()) {
            Yodo1U3dAds.ShowInterstitial();
        }
    }
    private void OnRewaredVideoSuccess()
    {
        Initialize();
        // 주사위 굴리기
        if (rewardType == AD_REWARD_TYPE.GET_ALL_DICES)
        {
            noDiceNoCoinController.HideScreen();
            resetDiceController.AbleResetDiceButton();
            resetDiceController.ResetDices();
        }
        // 게임 시작하기
        else
        {
            newHeartController.AddHeartAmount(1);
        }

        switch(rewardType) {
            case AD_REWARD_TYPE.LOAD_CLICKED_MAP: {
                mapController.OnClickMap();
                break;
            }
            case AD_REWARD_TYPE.LOAD_LEVEL_SCENE: {
                if (levelLoader.GetIsGoingToNextLevel()) {
                    levelLoader.LoadNextLevel();
                    levelLoader.SetIsGoingToNextLevel(false);
                    return;
                } 
                levelLoader.LoadCurrentScene();
                break;
            }
        }
    }
}
    

