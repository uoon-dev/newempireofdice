using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;



public class IAPManager : MonoBehaviour, IStoreListener
{
    [SerializeField] GameObject MaldivesButton = null;
    [SerializeField] GameObject GoldrushButton = null;

    private const string IOSMaldivesDiceId = "maldivesdice";
    private const string AndroidMaldivesDiceId = "maldivesdice";

    private const string IOSGoldrushDiceId = "goldrushdice";
    private const string AndroidGoldrushDiceId = "goldrushdice";
    private const string IOSSmallHeartId = "smallheart";
    private const string AndroidSmallHeartId = "smallheart";
    private const string IOSLargeHeartId = "largeheart";
    private const string AndroidLargeHeartId = "largeheart";
    private const string IOSHeartRechargeSpeedUpId = "speedupheartrecharge1";
    private const string AndroidHeartRechargeSpeedUpId = "speedupheartrecharge1";

    private static IAPManager mInstance;

    public static IAPManager Instance
    {
        get
        {
            if (mInstance != null) return mInstance;

            mInstance = FindObjectOfType<IAPManager>();

            if (mInstance == null) mInstance = new GameObject("IAP Manager").AddComponent<IAPManager>();
            return mInstance;
        }
    }

    private IStoreController storeController; // 구매 과정을 제어하는 함수를 제공
    private IExtensionProvider storeExtensionProvider; // 여러 플랫폼을 위한 확장 처리를 제공

    public bool IsInitialized => storeController != null && storeExtensionProvider != null;

    void Awake()
    {
        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
            return;
        }
        InitUnityIAP();
    }

    void InitUnityIAP()
    {
        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(
            Constants.MaldivesDice, ProductType.NonConsumable,
            new IDs()
            {
                { IOSMaldivesDiceId, AppleAppStore.Name },
                { AndroidMaldivesDiceId, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.GoldrushDice, ProductType.NonConsumable,
            new IDs()
            {
                { IOSGoldrushDiceId, AppleAppStore.Name },
                { AndroidGoldrushDiceId, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.SmallHeart, ProductType.Consumable,
            new IDs()
            {
                { IOSSmallHeartId, AppleAppStore.Name },
                { AndroidSmallHeartId, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.LargeHeart, ProductType.Consumable,
            new IDs()
            {
                { IOSLargeHeartId, AppleAppStore.Name },
                { AndroidLargeHeartId, GooglePlay.Name },
            }
            );
        builder.AddProduct(
            Constants.HeartRechargeSpeedUp, ProductType.NonConsumable,
            new IDs()
            {
                { IOSHeartRechargeSpeedUpId, AppleAppStore.Name },
                { AndroidHeartRechargeSpeedUpId, GooglePlay.Name },
            }
            );

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) 
    {
        Debug.Log("유니티 IAP 초기화 성공");
        storeController = controller;
        storeExtensionProvider = extensions;
        SetPricesInShop();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 IAP 초기화 실패 { error }");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");
        var heartShopController = FindObjectOfType<HeartShopController>();
        heartShopController.TogglePurchaseButton(false, args.purchasedProduct.definition.id);

        switch(args.purchasedProduct.definition.id)
        {
            case Constants.MaldivesDice:
                {
                    Debug.Log("몰디브 주사위 획득...");

                    MaldivesButton.GetComponent<ButtonController>().HidePurchaseButtonGroup();
                    MaldivesButton.GetComponent<ButtonController>().ShowUseButtonGroup();
                    PlayerPrefs.SetInt($"purchased-{Constants.MaldivesDice}", 1);
                    break;
                }
            case Constants.GoldrushDice:
                {
                    Debug.Log("골드러시 주사위 획득...");

                    GoldrushButton.GetComponent<ButtonController>().HidePurchaseButtonGroup();
                    GoldrushButton.GetComponent<ButtonController>().ShowUseButtonGroup();
                    PlayerPrefs.SetInt($"purchased-{Constants.GoldrushDice}", 1);
                    break;
                }
            case Constants.SmallHeart:
                {
                    Debug.Log("하트 구매...");

                    var heartController = FindObjectOfType<HeartController>();
                    heartController.SetHeartAmount(heartController.GetHeartAmount() + 15);

                    var afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
                    afterPurchaseEffectController.ShowScreen("0");
                    break;
                }
            case Constants.LargeHeart:
                {
                    Debug.Log("하트 많이 구매...");

                    var heartController = FindObjectOfType<HeartController>();
                    heartController.SetHeartAmount(heartController.GetHeartAmount() + 75);

                    var afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
                    afterPurchaseEffectController.ShowScreen("0");
                    break;
                }
            case Constants.HeartRechargeSpeedUp:
                {
                    Debug.Log("하트 충전 속도 업...");

                    var heartController = FindObjectOfType<HeartController>();
                    heartController.UpgradeHeartRechargeSpeed(2);

                    var afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
                    afterPurchaseEffectController.ShowScreen("1");
                    break;
                }
            default: break;
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        var heartShopController = FindObjectOfType<HeartShopController>();
        heartShopController.TogglePurchaseButton(false, product.definition.id);        
        Debug.LogWarning($"구매 실패 - {product.definition.id}, {reason}");
    }

    public void Purchase(string productId)
    {
        if (!IsInitialized) return;

        var product = storeController.products.WithID(productId);

        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"구매 시도 - {product.definition.id}");
            if (product.definition.id == Constants.HeartRechargeSpeedUp && RestorePurchase())
            {
                var heartController = FindObjectOfType<HeartController>();
                heartController.UpgradeHeartRechargeSpeed(2);

                var afterPurchaseEffectController = FindObjectOfType<AfterPurchaseEffectController>();
                afterPurchaseEffectController.ShowScreen("1");
            }
            else {
                storeController.InitiatePurchase(product);
            }
        }
        else
        {
            Debug.Log($"구매 시도 불가 - {productId}");
        }
    }

    public bool RestorePurchase()
    {
        if (!IsInitialized) return false;

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            bool isRestored = false;
            var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();
            appleExt.RestoreTransactions(
                result => { isRestored = true; }
                );
            return isRestored;
        }
        return false;
    }

    public Boolean HadPurchased(string productId)
    {
        if (!IsInitialized) return false;

        var product = storeController.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;
    }

    public void SetPricesInShop()
    {
        var heartShopCanvas = GameObject.Find("Heart Shop Canvas");
        if (heartShopCanvas == null) return;

        var smallHeartPrice = heartShopCanvas.transform.Find("Body").transform.Find("Left").transform.Find("Small Heart").transform.Find("Price");
        var largeHeartPrice = heartShopCanvas.transform.Find("Body").transform.Find("Left").transform.Find("Large Heart").transform.Find("Price");
        var heartRechargeSpeed = heartShopCanvas.transform.Find("Body").transform.Find("Right").transform.Find("Heart Recharge Speed").transform.Find("Price");
        smallHeartPrice.GetComponent<Text>().text = GetPrice(IOSSmallHeartId);
        largeHeartPrice.GetComponent<Text>().text = GetPrice(IOSLargeHeartId);

        int heartRechargeSpeedPurchased = PlayerPrefs.GetInt("HeartRechargeSpeed");
        if (heartRechargeSpeedPurchased != 2) 
        {
            heartRechargeSpeed.GetComponent<Text>().text = GetPrice(IOSHeartRechargeSpeedUpId);
        }
    }

    public string GetPrice(string productId)
    {
        if (!IsInitialized) return "";
        return storeController.products.WithID(productId).metadata.localizedPriceString;
    }    
}
