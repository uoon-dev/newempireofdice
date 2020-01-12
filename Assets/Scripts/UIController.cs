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
    Text heartTimerText;
    Text heartTimerTextInNoHeartCanvas;
    Text heartTimerTextInShop;
    Text heartShopTimer;
    Text heartCountText;
    Text heartUpdatedCountText;

    private void Initialize()
    {
        heartShopController = FindObjectOfType<HeartShopController>();
        startController = FindObjectOfType<StartController>();
        newHeartController = FindObjectOfType<NewHeartController>();
        levelLoader = FindObjectOfType<LevelLoader>();

        noHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.NO_HEART_CANVAS);
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
    }

    private void Awake()
    {
        Initialize();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int heartCharteRemainSecond = newHeartController.GetHeartTargetTimeStamp() - Utils.GetTimeStamp();
        // heartCountText.text = heartAmount + "";
        if (Utils.IsNetworkConnected())
        {
            if (heartAmount < Constants.HEART_MAX_CHARGE_COUNT)
            {
                string remainTime = string.Format("{0:0}:{1:00}", heartCharteRemainSecond / 60, heartCharteRemainSecond % 60);
                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
                {
                    heartTimerText.text = remainTime;
                    heartTimerText.fontSize = 32;
                    heartTimerText.color = new Color32(0, 0, 0, 255);
                }
                heartTimerTextInNoHeartCanvas.text = remainTime;
                heartTimerTextInShop.text = remainTime;
            } else 
            {
                string remainTime = string.Format("{0:0}:{1:00}", 0, 0);
                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
                {
                    heartTimerText.text = remainTime;
                    heartTimerText.fontSize = 32;
                    heartTimerText.color = new Color32(0, 0, 0, 255);                    
                }
                heartTimerTextInNoHeartCanvas.text = remainTime;
                heartTimerTextInShop.text = remainTime;
            }
        }
        else {
            if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
            {            
                heartTimerText.text = "오프라인";
                heartTimerText.fontSize = 24;
                heartTimerText.color = new Color32(193, 193, 193, 255);
            }
        }
    }

    public void HandleHeartBarUI() 
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int copiedHeartAmount = heartAmount;

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

    public void HandleHeartBarInEffectUI() 
    {
        int heartAmount = newHeartController.GetHeartAmount();
        int copiedHeartAmount = heartAmount;

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

    public void ToggleNoHeartCanvas(bool isShow) {
        // noHeartCanvas.SetActive(isShow);
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
