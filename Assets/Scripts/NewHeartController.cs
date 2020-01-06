using System;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

/* 
    Todo 
    다른 씬에도 NewHeartController, UIController 넣어주기
    하트 잘 사용되는지 테스트하기
    하트 상점에서 구입 잘 되는지 테스트하기 
    광고 리워드로 하트 잘 얻을 수 있는지 테스트하기
    하트 타이머 잘 작동되는지 테스트하기
    클리어 조건 완료시 하트 보상 잘되는지 테스트하기
    10x 스테이지 클리어시 하트 풀 충전 되는지 테스트하기
*/

public class NewHeartController : MonoBehaviour
{
    public static NewHeartController Instance; 
    private static Timer timer;
    private const int timerInterval = 500;
    private static int heartAmount;
    private static int heartTargetTimeStamp;
    private static bool isNetworkConnected;
    UIController UIController;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }
        // else if (Instance != this) {
        //     Destroy(gameObject);
        // }
        // DontDestroyOnLoad(gameObject);
    }

    private void Initialize()
    {
        if (PlayerPrefs.HasKey("HeartAmount")) {
            heartAmount = PlayerPrefs.GetInt("HeartAmount");
        } else {
            heartAmount = Constants.HEART_MAX_CHARGE_COUNT;
        }

        heartTargetTimeStamp = PlayerPrefs.GetInt("HeartTargetTimeStamp");
        UIController = FindObjectOfType<UIController>();
        InitializeHeartBar();
        InitializeTimer();
    }

    private void InitializeHeartBar() {
        UIController.HandleHeartBarUI();
    }

    private void InitializeTimer() {
        if (timer == null) {
            timer = new Timer(timerInterval);
            timer.AutoReset = true;
            timer.Elapsed += Tick;
        }
    }

    private void StartTimer() {
        timer.Start();
    }

    private void StopTimer() {
        timer.Stop();
    }

    private async void Tick(object sender, ElapsedEventArgs e) {
        int currentTimeStamp = Utils.GetTimeStamp();
        if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
        {
            if (currentTimeStamp >= heartTargetTimeStamp)
            {
                bool IsDeviceTimeValid = await Utils.IsDeviceTimeValid();
                if (IsDeviceTimeValid)
                {
                    int targetDeltaCount = (currentTimeStamp - heartTargetTimeStamp) / Constants.HEART_CHARGE_SECONDS;
                    heartAmount += targetDeltaCount + 1;
                    if (heartAmount > Constants.HEART_MAX_CHARGE_COUNT)
                    {
                        heartAmount = Constants.HEART_MAX_CHARGE_COUNT;
                    }
                    UIController.HandleHeartBarUI();
                    heartTargetTimeStamp = heartTargetTimeStamp + Constants.HEART_CHARGE_SECONDS * (targetDeltaCount + 1) - 1;
                }
                else
                {
                    StopTimer();
                }
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
        StopTimer();
        SaveHeartAmount(heartAmount);
        SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    }

    private void OnApplicationQuit()
    {
        StopTimer();
        SaveHeartAmount(heartAmount);
        SaveHeartTargetTimeStamp(heartTargetTimeStamp);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
            {
                StartTimer();
            }
        }
        else {
            StopTimer();
            SaveHeartAmount(heartAmount);
            SaveHeartTargetTimeStamp(heartTargetTimeStamp);
        }
        
    }

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
    }

    public void AddHeartAmount(int addedCount) {
        heartAmount += addedCount;
        UIController.HandleHeartBarUI();

        SaveHeartAmount(heartAmount);
    }

    public void SubtractHeartAmount(int subtractedCount) {
        heartAmount -= subtractedCount;
        UIController.HandleHeartBarUI();
        
        if (!timer.Enabled && heartAmount< Constants.HEART_MAX_CHARGE_COUNT) {
            heartTargetTimeStamp = Utils.GetTimeStamp() + Constants.HEART_CHARGE_SECONDS;
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
}
