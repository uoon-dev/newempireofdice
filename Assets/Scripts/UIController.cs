using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField]    
    public Sprite[] heartImageSprites = null;
    GameObject heartImageParentObject;
    GameObject heartImageParentObjectInEffect;
    GameObject noHeartCanvas;
    GameObject afterPurchaseEffectCanvas;
    HeartShopController heartShopController;
    StartController startController;
    NewHeartController newHeartController;
    LevelLoader levelLoader;
    IAPManager iAPManager;
    Text heartTimerText;
    Text heartTimerTextInNoHeartCanvas;
    Text heartTimerTextInShop;
    Text heartShopTimer;
    Text heartCountText;
    Text heartUpdatedCountText;
    Text timerTitle;
    Text titleInHeartShop;
    Text iapInitTest;
    private int prevHeartAmount = -1;
    private bool isNetworkConnected;
    private bool isIAPInitialized;
    private bool isSetSpeedUp = false;



    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();
        isIAPInitialized = iAPManager.isIAPInitialized();

        if (isNetworkConnected && !newIsNetworkConnected) 
        {
            DeactivePurchaseButtonInHeartShop();
            Debug.Log("DeactivePurchaseButtonInHeartShop");
        }

        if (!isNetworkConnected && newIsNetworkConnected && isIAPInitialized) 
        {
            ActivePurchaseButtonInHeartShop();
            Debug.Log("ActivePurchaseButtonInHeartShop");
        }

        if (isIAPInitialized && !isSetSpeedUp 
            && IAPManager.Instance.HadPurchased(Constants.HeartRechargeSpeedUp))
        {
            newHeartController.UpgradeHeartRechargeSpeed(2);
            heartShopController.SetSpeedUpText();
            isSetSpeedUp = true;
        }

        isNetworkConnected = newIsNetworkConnected;

        UpdateTimerText();
        
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
        {
            HandleHeartBarUI();
        }
        HandleHeartBarInEffectUI();

        int heartAmount = newHeartController.GetHeartAmount();
        if (heartAmount > 0)
        {
            ToggleNoHeartCanvas(false);
        }
    }

    private void Initialize()
    {
        bool newIsNetworkConnected = Utils.IsNetworkConnected();

        heartShopController = FindObjectOfType<HeartShopController>();
        startController = FindObjectOfType<StartController>();
        newHeartController = FindObjectOfType<NewHeartController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        iAPManager = FindObjectOfType<IAPManager>();
        
        afterPurchaseEffectCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.AFTER_PURCHASE_EFFECT_CANVAS);
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
        {
            heartImageParentObject = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_IMAGE_PARENT_OBJECT);
            heartTimerText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT).GetComponent<Text>();
            heartCountText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_COUNT_TEXT).GetComponent<Text>();
        }
        heartImageParentObjectInEffect = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_IMAGE_PARENT_OBJECT_IN_EFFECT);
        heartUpdatedCountText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_UPDATED_COUNT_TEXT).GetComponent<Text>();
        heartTimerTextInNoHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT_IN_NO_HEART_CANVAS).GetComponent<Text>();
        heartTimerTextInShop = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT_IN_SHOP).GetComponent<Text>();
        timerTitle = GameObject.Find(Constants.GAME_OBJECT_NAME.TIMER_TITLE_IN_SHOP).GetComponent<Text>();
        titleInHeartShop = GameObject.Find(Constants.GAME_OBJECT_NAME.TITLE_IN_SHOP).GetComponent<Text>();

        if (newIsNetworkConnected && isIAPInitialized)
        {
            ActivePurchaseButtonInHeartShop();
        }
        else {
            DeactivePurchaseButtonInHeartShop();
        }
    }

    // public void TimeTest()
    // {
    //     GameObject.Find("IsDeviceTimeValid Test").GetComponent<Text>().text = newHeartController.GetIsDeviceTimeValidTest().ToString();
    //     GameObject.Find("targetDeltaCount Test").GetComponent<Text>().text = newHeartController.GetTargetDeltaCountTest().ToString();
    // }

    public void UpdateTimerText()
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int heartCharteRemainSecond = heartAmount < Constants.HEART_MAX_CHARGE_COUNT ? 
            newHeartController.GetHeartTargetTimeStamp() - Utils.GetTimeStamp() : 0;

        if (Utils.IsNetworkConnected())
        {
            string remainTime = string.Format("{0:0}:{1:00}", heartCharteRemainSecond / 60, heartCharteRemainSecond % 60);
            // Debug.Log(remainTime);

            // if (heartCharteRemainSecond < 0)
            // {
            //     newHeartController.StartTimer();
            //     // Invoke("TimeTest", 0.5f);
            // }

            if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
            {
                heartTimerText.text = (heartAmount < Constants.HEART_MAX_CHARGE_COUNT && heartCharteRemainSecond > 0) ? remainTime : "";
                heartTimerText.fontSize = 32;
                heartTimerText.color = new Color32(0, 0, 0, 255);
            }
            else
            {
                heartTimerTextInNoHeartCanvas.fontSize = 14;
                heartTimerTextInShop.fontSize = 14;
            }

            if (heartCharteRemainSecond > 0)
            {
                heartTimerTextInNoHeartCanvas.text = remainTime;
                heartTimerTextInShop.text = remainTime;
                timerTitle.text = "다음하트";

                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 27);
                }
                else
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 30);
                }
            }
            else
            {
                heartTimerTextInNoHeartCanvas.text = "";
                heartTimerTextInShop.text = "";
                timerTitle.text = "";
                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 24);
                }
                else
                {
                    titleInHeartShop.transform.localPosition = new Vector2(titleInHeartShop.transform.localPosition.x, 27);
                }
            }
        }
        else
        {
            if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
            {
                heartTimerText.text = "오프라인";
                heartTimerText.fontSize = 24;
                heartTimerText.color = new Color32(193, 193, 193, 255);
            }
            else
            {
                heartTimerTextInNoHeartCanvas.fontSize = 12;
                heartTimerTextInShop.fontSize = 12;
            }            
            heartTimerTextInNoHeartCanvas.text = "오프라인";
            heartTimerTextInShop.text = "오프라인";
        }
    }

    public void ActivePurchaseButtonInHeartShop()
    {
        heartShopController.TogglePurchaseButton(false, Constants.SmallHeart);
        heartShopController.TogglePurchaseButton(false, Constants.LargeHeart);

        int heartRechargeSpeedPurchased = PlayerPrefs.GetInt("HeartRechargeSpeed");
        if (heartRechargeSpeedPurchased != 2) 
        {
            heartShopController.TogglePurchaseButton(false, Constants.HeartRechargeSpeedUp);
        }
    }

    public void DeactivePurchaseButtonInHeartShop()
    {
        heartShopController.TogglePurchaseButton(true, Constants.SmallHeart);
        heartShopController.TogglePurchaseButton(true, Constants.LargeHeart);

        int heartRechargeSpeedPurchased = PlayerPrefs.GetInt("HeartRechargeSpeed");
        if (heartRechargeSpeedPurchased != 2) 
        {
            heartShopController.TogglePurchaseButton(true, Constants.HeartRechargeSpeedUp);
        }
    }

    public void HandleHeartBarUI() 
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int copiedHeartAmount = heartAmount;

        // 하트 개수가 달라질 때만 heart bar 업데이트 하기
        if (prevHeartAmount == heartAmount) return;

        Debug.Log("heart updated");
        prevHeartAmount = heartAmount;

        if (heartImageParentObject != null)
        {
            for (int i = 0; i < heartImageParentObject.transform.childCount; i++) {
                var heartImageObject = heartImageParentObject.transform.GetChild(heartImageParentObject.transform.childCount - i - 1);
                var heartImage = heartImageObject.GetComponent<Image>();

                heartImage.sprite = heartImageSprites[1];                        
                if (copiedHeartAmount <= 0) {
                    heartImage.sprite = heartImageSprites[2];
                } else {
                    if (i == 0 && copiedHeartAmount > Constants.HEART_MAX_CHARGE_COUNT) {
                        heartImage.sprite = heartImageSprites[0];
                        heartCountText.text = heartAmount.ToString();
                    }
                }

                if (heartAmount <= Constants.HEART_MAX_CHARGE_COUNT) {
                    heartCountText.text = string.Empty;
                }
                copiedHeartAmount--;
            }
        }

    }

    public void HandleHeartBarInEffectUI() 
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int copiedHeartAmount = heartAmount;

        // 하트 개수가 달라질 때만 heart bar 업데이트 하기
        if (prevHeartAmount != heartAmount)
        {
            prevHeartAmount = heartAmount;
            return; 
        }

        if (heartImageParentObjectInEffect != null)
        {
            for (int i = 0; i < heartImageParentObjectInEffect.transform.childCount; i++) {
                var heartImageObject = heartImageParentObjectInEffect.transform.GetChild(heartImageParentObjectInEffect.transform.childCount - i - 1);
                var heartImage = heartImageObject.GetComponent<Image>();

                heartImage.sprite = heartImageSprites[1];                        
                if (copiedHeartAmount <= 0) {
                    heartImage.sprite = heartImageSprites[2];
                } else {
                    if (i == 0 && copiedHeartAmount > Constants.HEART_MAX_CHARGE_COUNT) {
                        heartImage.sprite = heartImageSprites[0];
                        heartUpdatedCountText.text = heartAmount.ToString();
                    }
                }

                if (heartAmount <= Constants.HEART_MAX_CHARGE_COUNT) {
                    heartUpdatedCountText.text = string.Empty;
                }
                copiedHeartAmount--;
            }
        }
    }

    public void ToggleNoHeartCanvas(bool isShow) {
        noHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.NO_HEART_CANVAS);
        startController = FindObjectOfType<StartController>();
        var body = noHeartCanvas.transform.GetChild(0);

        if (isShow) {
            if (heartShopController != null)
                heartShopController.ToggleHeartShopCanvas(false);

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
}
