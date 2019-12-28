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
    void Start()
    {
        SetSpeedUpText();
    }

    public void SetSpeedUpText()
    {
        Text HeartRechargeSpeedText = HeartText.GetComponent<Text>();
        Image HeartRechargeSpeedImage = HeartButton.GetComponent<Image>();
        Button HeartRechargeSpeedButton = HeartButton.GetComponent<Button>();

        if (HeartRechargeSpeedText != null) {
            int heartRechargeSpeed = PlayerPrefs.GetInt("HeartRechargeSpeed");
            Debug.Log("heartChargeSpeed : "+heartRechargeSpeed);
            if (heartRechargeSpeed == 2)
            {
                HeartRechargeSpeedText.text = "구매함";
                HeartRechargeSpeedText.color = new Color32(0, 0, 0, 100);
                if (LevelLoader.GetCurrentSceneName() == "Map System")
                {
                    HeartRechargeSpeedText.fontSize = 20;
                }else{
                    HeartRechargeSpeedText.fontSize = 10;
                }
                
                HeartRechargeSpeedImage.color = new Color32(255, 255, 255, 100);
                HeartRechargeSpeedButton.interactable = false;
            }
        }
        
    }

    public void ToggleHeartShopCanvas(bool isShow) {
        this.gameObject.SetActive(isShow);
        var body = this.gameObject.transform.GetChild(0);

        if (isShow) {
            this.gameObject.GetComponent<Image>().raycastTarget = true;
            TogglePurchaseButton(false, Constants.SmallHeart);
            TogglePurchaseButton(false, Constants.LargeHeart);

            int heartRechargeSpeedPurchased = PlayerPrefs.GetInt("HeartRechargeSpeed");
            if (heartRechargeSpeedPurchased != 2) 
            {
                TogglePurchaseButton(false, Constants.HeartRechargeSpeedUp);
            }
 
            var heartController = FindObjectOfType<HeartController>();
            if (heartController != null)
                heartController.ToggleNoHeartCanvas(false);

            if(LevelLoader.GetCurrentSceneName() == "Map System") {
                this.gameObject.transform.DOMoveY(0, 0.25f);
                return;
            }
            body.transform.DOMoveY(Screen.height/2, 0.25f);
            return;
        }
        this.gameObject.GetComponent<Image>().raycastTarget = false;
        if(LevelLoader.GetCurrentSceneName() == "Map System") {
            this.gameObject.transform.DOMoveY(-3, 0.25f);
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

    public void TogglePurchaseButton(bool isLoadinng, string targetProductId)
    {
        GameObject purchaseButton = null;
        Transform closeButton = this.transform.Find("Body").transform.Find("Header").transform.Find("Close Button");
        Transform priceText = null;

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

        if (isLoadinng) 
        {
            purchaseButton.GetComponent<Image>().sprite = loadingButtonImage;
            priceText.GetComponent<Text>().text = "";
            purchaseButton.GetComponent<Button>().interactable = false;
            closeButton.GetComponent<Button>().interactable = false;
        } 
        else 
        {
            purchaseButton.GetComponent<Image>().sprite = defaultPurchaseButtonImage;
            purchaseButton.GetComponent<Button>().interactable = true;
            closeButton.GetComponent<Button>().interactable = true;
            FindObjectOfType<IAPManager>().SetPricesInShop();
        }
    }
}
