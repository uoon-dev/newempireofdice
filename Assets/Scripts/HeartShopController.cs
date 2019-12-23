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
                    HeartRechargeSpeedText.fontSize = 12;
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
        if(LevelLoader.GetCurrentSceneName() == "Map System") {
            this.gameObject.transform.DOMoveY(-3, 0.25f);
            return;
        }
        body.transform.DOMoveY(-Screen.height/2, 0.25f);
        return;
    }

    public void HandleClick(string targetProductId)
    {
        if (IAPManager.Instance.HadPurchased(targetProductId))
        {
            Debug.Log("이미 구매한 상품입니다.");
            return;
        }
        // Image purchaseButtonImage = null;
        // switch (targetProductId) {
        //     case Constants.SmallHeart: {
        //         purchaseButtonImage = GameObject.Find("Small Heart Purchase Button Image").GetComponent<Image>();
        //         break;
        //     }
        //     case Constants.LargeHeart: {
        //         purchaseButtonImage = GameObject.Find("Large Heart Purchase Button Image").GetComponent<Image>();
        //         break;
        //     }
        //     case Constants.HeartRechargeSpeedUp: {
        //         purchaseButtonImage = GameObject.Find("HeartRechargeSpeedButton").GetComponent<Image>();
        //         break;
        //     }
        // }
        // purchaseButtonImage.sprite = loadingButtonImage;
        IAPManager.Instance.Purchase(targetProductId);
    }

    // public void 

}
