using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    [SerializeField]    
    public Sprite[] heartImageSprites = null;
    GameObject heartImageParentObject;
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

        heartImageParentObject = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_IMAGE_PARENT_OBJECT);
        noHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.NO_HEART_CANVAS);
        afterPurchaseEffectCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.AFTER_PURCHASE_EFFECT_CANVAS);
        if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM) 
        {
            Debug.Log(levelLoader.GetCurrentSceneName());
            heartTimerText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT).GetComponent<Text>();
        }
        heartTimerTextInNoHeartCanvas = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT_IN_NO_HEART_CANVAS).GetComponent<Text>();
        heartTimerTextInShop = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_TIMER_TEXT_IN_SHOP).GetComponent<Text>();
        heartCountText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_COUNT_TEXT).GetComponent<Text>();
        heartUpdatedCountText = GameObject.Find(Constants.GAME_OBJECT_NAME.HEART_UPDATED_COUNT_TEXT).GetComponent<Text>();
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
                heartTimerText.text = remainTime;
                heartTimerTextInNoHeartCanvas.text = remainTime;
                heartTimerTextInShop.text = remainTime;
            }
        }
        else {
            // heartTimerText.text = Texts.OFFLINE;
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
                    heartUpdatedCountText.gameObject.SetActive(true);
                    heartUpdatedCountText.text = heartAmount.ToString();
                }
            }

            if (heartAmount <= Constants.HEART_MAX_CHARGE_COUNT) {
                heartCountText.text = string.Empty;
                heartUpdatedCountText.gameObject.SetActive(false);
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
