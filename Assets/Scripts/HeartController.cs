using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartController : MonoBehaviour
{
    private int m_HeartAmount = 0;
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    private const int MAX_HEART = 5;
    private int heartRechargeInterval = 60 * 20;
    private int heartRechargeIntervalUpdated = 0;
    private Coroutine m_RechargeTimerCoroutine = null;
    private int m_RechargeRemainTime = 0;
    public static int heartRechargeSpeed = 1;

    [SerializeField]
    public GameObject heartState = null;
    [SerializeField]
    public GameObject heartTotalCount = null;
    [SerializeField]
    public GameObject heartTotalCountAfterPurchase = null;
    [SerializeField]
    public GameObject heartImagesObject = null;
    [SerializeField]
    public Sprite[] heartImages = null;
    [SerializeField]
    public GameObject noHeartCanvasTimer = null;
    [SerializeField]
    public GameObject heartShopTimer = null;
    [SerializeField]
    public GameObject heartBarTimer = null;
    [SerializeField]
    public GameObject noHeartCanvas = null;
    LevelLoader levelLoader;


    // Start is called before the first frame update
    private void Awake() 
    {
        Init();
        LoadRechargeSpeedInfo();
        LoadHeartInfo();
        LoadAppQuitTime();

        if (m_HeartAmount < MAX_HEART) {
            SetRechargeScheduler();
        }
    }
    public void OnApplicationPause(bool value) 
    {
        // Debug.Log("OnApplicationFocus");
        if (value)
        {
            SaveHeartInfo();
            SaveAppQuitTime();
        // Debug.Log($"m_HeartAmount {m_HeartAmount}");
        }
        else
        {
            LoadRechargeSpeedInfo();
            LoadHeartInfo();
            LoadAppQuitTime();
            if (m_HeartAmount < MAX_HEART) {
                SetRechargeScheduler();
            }
        }
    }

    private void OnDestroy() {
        SaveHeartInfo();
        SaveAppQuitTime();        
    }

    private void OnApplicationQuit() {
        // Debug.Log("GoodsRechargeTester: OnApplicationQuit()");
        SaveHeartInfo();
        SaveAppQuitTime();
    }

    public void OnClickUseHeart()
    {
        // Debug.Log("OnClickUseHeart");
        UseHeart();
    }

    public void Init() 
    {
        m_HeartAmount = 0;
        m_RechargeRemainTime = 0;
        m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
        if (heartBarTimer != null) {
            heartBarTimer.SetActive(false);
        }
        levelLoader = FindObjectOfType<LevelLoader>();
    }
    public bool LoadRechargeSpeedInfo() {
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("HeartRechargeSpeed"))
            {
                // PlayerPrefs.DeleteKey("HeartRechargeSpeed");
                heartRechargeSpeed = PlayerPrefs.GetInt("HeartRechargeSpeed");
                heartRechargeIntervalUpdated = heartRechargeInterval / heartRechargeSpeed;
            }
            result = true;
        }
        catch (System.Exception e)
        {
            // Debug.LogError("LoadRechargeSpeedInfo Failed (" + e.Message + ")");
        }

        return result;
    }

    public bool LoadHeartInfo()
    {
        // Debug.Log("LoadHeartInfo");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("HeartAmount"))
            {
                // Debug.Log("PlayerPrefs has key : HeartAmount");
                m_HeartAmount = PlayerPrefs.GetInt("HeartAmount");
                if (m_HeartAmount <= 0)
                {
                    m_HeartAmount = 0;
                }
            }
            else if (m_HeartAmount >= MAX_HEART)
            {
                m_HeartAmount = MAX_HEART;
            }
            if (heartImagesObject != null) {
                SetHeartBar();
            }
            if (heartState != null) {
                heartState.GetComponent<Text>().text = m_HeartAmount.ToString();
            }
            // SaveHeartInfo();
            // Debug.Log("Loaded HeartAmount : " + m_HeartAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            // Debug.LogError("LoadHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }

    public void SetHeartBar() 
    {
        int heartAmount = m_HeartAmount;
        for (int i = 0; i < heartImagesObject.transform.childCount; i++) {
            var heartImageObject = heartImagesObject.transform.GetChild(heartImagesObject.transform.childCount - i - 1);
            var heartImage = heartImageObject.GetComponent<Image>();

            heartImage.sprite = heartImages[1];                        
            if (heartAmount <= 0) {
                heartImage.sprite = heartImages[2];
            } else {
                if (i == 0 && heartAmount > MAX_HEART) {
                    heartImage.sprite = heartImages[0];
                    heartTotalCount.SetActive(true);
                    heartTotalCount.GetComponent<Text>().text = m_HeartAmount.ToString();
                    heartTotalCountAfterPurchase.SetActive(true);
                    heartTotalCountAfterPurchase.GetComponent<Text>().text = m_HeartAmount.ToString();
                }
            }

            if (m_HeartAmount <= MAX_HEART) {
                heartTotalCount.SetActive(false);
                heartTotalCountAfterPurchase.SetActive(false);
            }
            heartAmount--;
        }
    }

    public bool SaveHeartInfo()
    {
        // Debug.Log("SaveHeartInfo");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("HeartAmount", m_HeartAmount);
            PlayerPrefs.Save();
            // Debug.Log("Saved HeartAmount : " + m_HeartAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            // Debug.LogError("SaveHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool LoadAppQuitTime()
    {
        // Debug.Log("LoadAppQuitTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                // Debug.Log("PlayerPrefs has key : AppQuitTime");
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));
            }
            // Debug.Log(string.Format("Loaded AppQuitTime : {0}", m_AppQuitTime.ToString()));
            
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveAppQuitTime()
    {
        // Debug.Log("SaveAppQuitTime");
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();
            var rechargeRemainTime = m_RechargeRemainTime;
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.SetInt("RechargeRemainTime", rechargeRemainTime);
            PlayerPrefs.Save();
            // Debug.Log("Saved AppQuitTime : " + DateTime.Now.ToLocalTime().ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public async void SetRechargeScheduler(Action onFinish = null)
    {
        bool IsDeviceTimeValid = await Utils.IsDeviceTimeValid();

        int timeDifferenceInSec = 0;
        if (IsDeviceTimeValid) {
            timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);
        }
        int rechargeRemainTime = PlayerPrefs.GetInt("RechargeRemainTime");
        // Debug.Log("TimeDifference In Sec :" + timeDifferenceInSec + "s");
        int remainTime = rechargeRemainTime > timeDifferenceInSec ? 
                rechargeRemainTime - timeDifferenceInSec : 
                heartRechargeInterval - (timeDifferenceInSec - rechargeRemainTime) % heartRechargeInterval;
        int heartToAdd = (timeDifferenceInSec + heartRechargeInterval - rechargeRemainTime) / heartRechargeInterval;
        // Debug.Log("Heart to add : " + heartToAdd);
        // Debug.Log("RemainTime : " + remainTime);

        m_HeartAmount += heartToAdd;
        if (m_HeartAmount >= MAX_HEART)
        {
            m_HeartAmount = MAX_HEART;
        }
        else
        {
            if (m_RechargeTimerCoroutine != null)
            {
                StopCoroutine(m_RechargeTimerCoroutine);
            }
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }

        // Debug.Log("HeartAmount : " + m_HeartAmount);
    }

    public void ToggleNoHeartCanvas(bool isShow) {
        noHeartCanvas.SetActive(isShow);
        var body = noHeartCanvas.transform.GetChild(0);

        if (isShow) {
            var heartShopController = FindObjectOfType<HeartShopController>();
            if (heartShopController != null)
                heartShopController.ToggleHeartShopCanvas(false);

            var startController = FindObjectOfType<StartController>();
            if (startController != null)
                startController.HideScreen();

            if(levelLoader.GetCurrentSceneName() == "Map System") {
                noHeartCanvas.transform.DOMoveY(0, 0.25f);
                return;
            }
            body.transform.DOMoveY(Screen.height/2 - 20, 0.25f);
            return;
        }
        if(levelLoader.GetCurrentSceneName() == "Map System") {
            noHeartCanvas.transform.DOMoveY(-3, 0.25f);
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void UseHeart(Action onFinish = null)
    {
        if (m_HeartAmount <= 0)
        {
            ToggleNoHeartCanvas(true);
            return;
        }
        m_HeartAmount--;
        if (heartImagesObject != null) {
            SetHeartBar();
        }
        if (heartState != null) {
            heartState.GetComponent<Text>().text = m_HeartAmount.ToString();
        }
        // SaveHeartInfo();

        if (m_RechargeTimerCoroutine == null)
        {
            if (m_HeartAmount < MAX_HEART) {
                m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(heartRechargeInterval));
            }
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }

    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        // Debug.Log("DoRechargeTimer");
        if (remainTime <= 0)
        {
            m_RechargeRemainTime = heartRechargeInterval;
        }
        else
        {
            m_RechargeRemainTime = remainTime;
            if (heartRechargeSpeed > 1 && remainTime >= heartRechargeIntervalUpdated)
                m_RechargeRemainTime = heartRechargeIntervalUpdated;
        }

        // Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
        while (m_RechargeRemainTime > 0)
        {
            int minutes = Mathf.FloorToInt(m_RechargeRemainTime / 60f);
            int seconds = Mathf.FloorToInt(Mathf.Floor(m_RechargeRemainTime - minutes * 60f));
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            noHeartCanvasTimer.GetComponent<Text>().text = niceTime;
            heartShopTimer.GetComponent<Text>().text = niceTime;
            if (heartBarTimer != null) {
                heartBarTimer.SetActive(true);
                heartBarTimer.GetComponent<Text>().text = niceTime;
            }

            // Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
            m_RechargeRemainTime -= 1;
            yield return new WaitForSeconds(1f);
        }

        m_HeartAmount++;
        if (m_HeartAmount >= MAX_HEART)
        {
            if (m_HeartAmount != MAX_HEART) {
                m_HeartAmount = m_HeartAmount - 1;
            }
            m_RechargeRemainTime = 0;
            if (heartBarTimer != null)
                heartBarTimer.SetActive(false);
            // Debug.Log("HeartAmount reached max amount");
            m_RechargeTimerCoroutine = null;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(heartRechargeInterval, onFinish));
            // Debug.Log("HeartAmount : " + m_HeartAmount);
        }
        if (heartImagesObject != null) {
            SetHeartBar();
        }

        if (heartState != null) 
        {
            heartState.GetComponent<Text>().text = m_HeartAmount.ToString();
        }
        // SaveHeartInfo();
    }

    public int GetHeartAmount() 
    {
        return m_HeartAmount;
    }

    public void SetHeartAmount(int amount)
    {
        m_HeartAmount = amount;
        PlayerPrefs.SetInt("HeartAmount", m_HeartAmount);

        if (m_HeartAmount >= MAX_HEART)
        {
            m_RechargeRemainTime = 0;
            // Debug.Log("HeartAmount reached max amount");
            if (m_RechargeTimerCoroutine != null) {
                StopCoroutine(m_RechargeTimerCoroutine);
                string niceTime = string.Format("{0:0}:{1:00}", 0, 0);

                noHeartCanvasTimer.GetComponent<Text>().text = niceTime;
                heartShopTimer.GetComponent<Text>().text = niceTime;
                if (heartBarTimer != null) {
                    heartBarTimer.SetActive(false);
                    heartBarTimer.GetComponent<Text>().text = niceTime;
                }
                
                m_RechargeTimerCoroutine = null;
            }
        }

        if (heartImagesObject != null) {
            SetHeartBar();
        }        

        if (heartState != null) 
        {
            heartState.GetComponent<Text>().text = m_HeartAmount.ToString();
        }
    }

    public void UpgradeHeartRechargeSpeed(int speed) {
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }

        heartRechargeSpeed = speed;
        heartRechargeIntervalUpdated = heartRechargeInterval / speed;
        PlayerPrefs.SetInt("HeartRechargeSpeed", speed);
        
        if (m_HeartAmount < MAX_HEART) {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(m_RechargeRemainTime));
        }

        FindObjectOfType<HeartShopController>().SetSpeedUpText();
    }
}
