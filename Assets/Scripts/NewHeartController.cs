using System;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

/* 
    Todo 
    다른 씬에도 NewHeartController, UIController 넣어주기 (완료)
    하트 잘 사용되는지 테스트하기 (완료)
    하트 상점에서 구입 잘 되는지 테스트하기 (완료)
    하트 타이머 잘 작동되는지 테스트하기 (완료)
    클리어 조건 완료시 하트 보상 잘되는지 테스트하기 (완료)
    10x 스테이지 클리어시 하트 풀 충전 되는지 테스트하기 (완료)
    하트 충전속도 증가 로직 구현하기 (완료)
    광고 리워드로 하트 잘 얻을 수 있는지 테스트하기

    bug
    처음 앱을 실행했을 때 하트가 0개이다. (solved)
    타이머 충전 중간에 인터넷을 꺼도 타이머 상태가 오프라인으로 변하지 않는다. (solved)
    타이머 충전 중간에 인터넷을 끄고 앱을 껐다가 다시 켜도 오프라인 표시가 나타나지 않는다. (solved)
    앱을 pause 했다가 다시 돌아가보면 하트 하나가 더 충전되어 있다. (solved)

    * 인터넷을 중간에 다시 연결해도 타이머가 동작하지 않는다. -> 플레이를 해야 동작하는데 10분으로 리셋된다. *
*/

public class NewHeartController : MonoBehaviour
{
    public static NewHeartController Instance; 
    public static Timer timer;
    private const int timerInterval = 500;
    private int heartRechargeSpeed = 1;
    private int heartAmount;
    public static bool isHeartAmountUpdated = false;
    private int heartTargetTimeStamp;
    private bool isNetworkConnected;
    private bool IsDeviceTimeValid = false;
    public static UIController UIController;
    public static LevelLoader levelLoader;
    public static HeartShopController heartShopController;
    // public bool IsDeviceTimeValidTest = false;
    // public int targetDeltaCountTest = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);        
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("HeartAmount")) 
        {
            heartAmount = PlayerPrefs.GetInt("HeartAmount");
        }
        else 
        {
            heartAmount = Constants.HEART_MAX_CHARGE_COUNT;
        }

        if (PlayerPrefs.HasKey("HeartRechargeSpeed") || IAPManager.Instance.HadPurchased(Constants.HeartRechargeSpeedUp))
        {
            heartRechargeSpeed = 2;
        } 
        else 
        {
            heartRechargeSpeed = 1;
        }

        heartTargetTimeStamp = PlayerPrefs.GetInt("HeartTargetTimeStamp");
        UIController = FindObjectOfType<UIController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        heartShopController = FindObjectOfType<HeartShopController>();

        InitializeTimer();

        int heartCharteRemainSecond = GetHeartTargetTimeStamp() - Utils.GetTimeStamp();
    }

    private void InitializeHeartBar() {
        UIController.HandleHeartBarUI();
        UIController.HandleHeartBarInEffectUI();
    }

    private void InitializeTimer() {
        if (timer == null) {
            timer = new System.Timers.Timer(timerInterval);
            timer.AutoReset = false;
            timer.Elapsed += Tick;
        }
    }

    public void StartTimer() {
        timer.Start();
    }

    private void StopTimer() {
        if (timer.Enabled) 
        {
            timer.Stop();
        }
    }

    private async void Tick(object sender, ElapsedEventArgs e) {
        int currentTimeStamp = Utils.GetTimeStamp();
        if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
        {
            bool IsDeviceTimeValid = true;
            if (currentTimeStamp >= heartTargetTimeStamp)
            {
                IsDeviceTimeValid = await Utils.IsDeviceTimeValid();
                // IsDeviceTimeValidTest = IsDeviceTimeValid;
                if (IsDeviceTimeValid)
                {
                    int targetDeltaCount = (currentTimeStamp - heartTargetTimeStamp) / (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
                    // targetDeltaCountTest = targetDeltaCount;
                    heartAmount += targetDeltaCount + 1;
                    if (heartAmount > Constants.HEART_MAX_CHARGE_COUNT)
                    {
                        heartAmount = Constants.HEART_MAX_CHARGE_COUNT;
                    }
                    heartTargetTimeStamp = heartTargetTimeStamp + (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed) * (targetDeltaCount + 1) - 1;
                    isHeartAmountUpdated = true;
                }
            }
            if (IsDeviceTimeValid)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }
        }
        else
        {
            StopTimer();
        }
        
    }

    private void Start()
    {
        if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT) {
            StartTimer();
        }
    }

    private void Update()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();

        if (isNetworkConnected && !newIsNetworkConnected) {
            StopTimer();
        }

        if (!isNetworkConnected && newIsNetworkConnected) {
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
            {
                StartTimer();
            }
        }
        
        isNetworkConnected = newIsNetworkConnected;
    }


    private void OnDestroy()
    {
        SaveHeartAmount(heartAmount);
        SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    }

    private void OnApplicationQuit()
    {
        SaveHeartAmount(heartAmount);
        SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    }

    // private void OnApplicationFocus(bool focus)
    // {
    //     if (focus)
    //     {
    //         if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
    //         {
    //             StartTimer();
    //         }
    //     }
    //     else {
    //         StopTimer();
    //         SaveHeartAmount(heartAmount);
    //         SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    //     }
    // }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            StopTimer();
            SaveHeartAmount(heartAmount);
            SaveHeartTargetTimeStamp(heartTargetTimeStamp);
        }
        else
        {
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
            {
                StartTimer();
            }
        }
    }

    private void SaveHeartTargetTimeStamp(int timestamp) {
        PlayerPrefs.SetInt("HeartTargetTimeStamp", timestamp);
    }

    public bool CanUseHeart()
    {
        if (GetHeartAmount() <= 0) {
            UIController.ToggleNoHeartCanvas(true);
            return false;
        }
        SubtractHeartAmount(1);
        return true;
    }    


    public void OnClickUseHeart()
    {
        if (heartAmount <= 0)
        {
            UIController.ToggleNoHeartCanvas(true);
        } else {
            SubtractHeartAmount(1);
        }
    }    

    private void SaveHeartAmount(int targetHeartAmount) {
        PlayerPrefs.SetInt("HeartAmount", targetHeartAmount);
        PlayerPrefs.Save();
    }

    public void AddHeartAmount(int addedCount) {
        heartAmount += addedCount;
        // if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
        // {        
        //     UIController.HandleHeartBarUI();
        // }
        // UIController.HandleHeartBarInEffectUI();
        SaveHeartAmount(heartAmount);
    }

    public void SubtractHeartAmount(int subtractedCount) {
        heartAmount -= subtractedCount;
        // if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
        // {
        //     UIController.HandleHeartBarUI();
        // }
        if (!timer.Enabled && heartAmount < Constants.HEART_MAX_CHARGE_COUNT) {
            heartTargetTimeStamp = Utils.GetTimeStamp() + (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
            StartTimer();
        }
    }

    public int GetHeartAmount()
    {
        return heartAmount;
    }

    public int GetHeartTargetTimeStamp()
    {
        return heartTargetTimeStamp;
    }

    public void UpgradeHeartRechargeSpeed(int speed)
    {
        heartRechargeSpeed = speed;
        PlayerPrefs.SetInt("HeartRechargeSpeed", speed);

        int heartCharteRemainSecond = GetHeartTargetTimeStamp() - Utils.GetTimeStamp();
        if (heartCharteRemainSecond > Constants.HEART_CHARGE_SECONDS / speed)
        {
            StopTimer();
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT) 
            {
                int currentTimeStamp = Utils.GetTimeStamp();
                int targetDeltaCount = (currentTimeStamp - heartTargetTimeStamp) / (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
                heartTargetTimeStamp = currentTimeStamp + (Constants.HEART_CHARGE_SECONDS / heartRechargeSpeed);
                StartTimer();
            }
        }
    }

    // public bool GetIsDeviceTimeValidTest() {
    //     return IsDeviceTimeValidTest;
    // }

    // public int GetTargetDeltaCountTest() {
    //     return targetDeltaCountTest;
    // }    
}
