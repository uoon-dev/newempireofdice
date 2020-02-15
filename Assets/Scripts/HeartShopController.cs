using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeartShopController : MonoBehaviour
{
    [SerializeField] GameObject HeartText;
    [SerializeField] GameObject HeartButton;
    [SerializeField] Sprite defaultPurchaseButtonImage;
    [SerializeField] Sprite loadingButtonImage;

    LevelLoader levelLoader;
    UIController UIController;
    IAPManager iAPManager;
    NewHeartController newHeartController;

    void Awake()
    {
        Initialize();
        SetSpeedUpText();
    }

    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        UIController = FindObjectOfType<UIController>();
        iAPManager = FindObjectOfType<IAPManager>();
        newHeartController = FindObjectOfType<NewHeartController>();
    }    

    public void SetSpeedUpText()
    {
        Text HeartRechargeSpeedText = HeartText.GetComponent<Text>();
        Image HeartRechargeSpeedImage = HeartButton.GetComponent<Image>();
        Button HeartRechargeSpeedButton = HeartButton.GetComponent<Button>();

        if (HeartRechargeSpeedText != null) {
            int heartRechargeSpeed = PlayerPrefs.GetInt("HeartRechargeSpeed");
            if (heartRechargeSpeed == 2 || IAPManager.Instance.HadPurchased(Constants.HeartRechargeSpeedUp))
            {
                HeartRechargeSpeedText.text = "구매함";
                HeartRechargeSpeedText.color = new Color32(0, 0, 0, 100);
                if (levelLoader.GetCurrentSceneName() == Constants.SCENE_NAME.MAP_SYSTEM)
                {
                    HeartRechargeSpeedText.fontSize = 20;
                } 
                else
                {
                    HeartRechargeSpeedText.fontSize = 10;
                }
                
                HeartRechargeSpeedImage.color = new Color32(255, 255, 255, 100);
                HeartRechargeSpeedButton.interactable = false;
            }
        }   
    }

    public void ToggleHeartShopCanvas(bool isShow) {
        var body = this.gameObject.transform.GetChild(0);

        if (isShow) {
            this.gameObject.GetComponent<Image>().raycastTarget = true;
 
            UIController.ToggleNoHeartCanvas(false);

            if(levelLoader.GetCurrentSceneName() == "Map System") {
                this.gameObject.transform.DOMoveY(0, 0.25f);
                this.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
                this.gameObject.GetComponent<CanvasGroup>().interactable = true;
                this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;                
                return;
            }
            body.transform.DOMoveY(Screen.height/2, 0.25f);
            return;
        }
        this.gameObject.GetComponent<Image>().raycastTarget = false;
        if(levelLoader.GetCurrentSceneName() == "Map System") {
            this.gameObject.transform.DOMoveY(-4, 0.25f);
            this.gameObject.GetComponent<CanvasGroup>().DOFade(0, 0.01f);
            this.gameObject.GetComponent<CanvasGroup>().interactable = false;
            this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void HandleClick(string targetProductId)
    {
        // if (IAPManager.Instance.HadPurchased(targetProductId))
        // {
        //     Debug.Log("이미 구매한 상품입니다.");
        //     return;
        // }

        TogglePurchaseButton(true, targetProductId);
        IAPManager.Instance.Purchase(targetProductId);
    }

    public void TogglePurchaseButton(bool isLoading, string targetProductId)
    {
        GameObject purchaseButton = null;
        Transform closeButton = this.transform.Find("Body").transform.Find("Header").transform.Find("Close Button");
        Transform priceText = null;
        int heartRechargeSpeedPurchased = PlayerPrefs.GetInt("HeartRechargeSpeed");

        switch (targetProductId) {
            case Constants.SmallHeart: {
                purchaseButton = GameObject.Find("Small Heart Purchase Button");
                priceText = GameObject.Find("Small Heart").transform.Find("Price");
                break;
            }
            case Constants.LargeHeart: {
                purchaseButton = GameObject.Find("Large Heart Purchase Button");
                priceText = GameObject.Find("Large Heart").transform.Find("Price");
                break;
            }
            case Constants.HeartRechargeSpeedUp: {
                purchaseButton = GameObject.Find("HeartRechargeSpeedButton");
                priceText = GameObject.Find("Heart Recharge Speed").transform.Find("Price");
                break;
            }
        }

        if (isLoading) 
        {
            purchaseButton.GetComponent<Image>().sprite = loadingButtonImage;
            priceText.GetComponent<Text>().text = "";
            purchaseButton.GetComponent<Button>().interactable = false;
        }
        else 
        {
            purchaseButton.GetComponent<Image>().sprite = defaultPurchaseButtonImage;
            purchaseButton.GetComponent<Button>().interactable = true;
            // iAPManager.SetPricesInShop();
            if (targetProductId == Constants.HeartRechargeSpeedUp)
            {
                if (heartRechargeSpeedPurchased != 2)
                {
                    priceText.GetComponent<Text>().text = iAPManager.GetPrice(targetProductId);
                }
            }
            else
            {
                priceText.GetComponent<Text>().text = iAPManager.GetPrice(targetProductId);
            }
        }
    }
}
