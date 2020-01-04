using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public string targetProductId;
    [SerializeField] GameObject PurchaseButtonGroup = null;
    [SerializeField] GameObject UseButtonGroup = null;

    void Start()
    {
        // ButtonController는 상점의 각 주사위마다 가지고 있다.
        if (PlayerPrefs.GetInt($"purchased-{targetProductId}") == 1)
        {
            // 이미 샀던 아이템이라면 구매하기 버튼을 안 보여주게 한다.
            HidePurchaseButtonGroup();

            if (PlayerPrefs.GetInt($"used-{targetProductId}") == 1)
            {
                // 이미 샀던 아이템이고 장착까지 했다면 
                UseDice();
            }
            else
            {
                NotUseDice();
            }
        }
        else
        {
            HideUseButtonGroup();
        }
    }

    public void HandleClick()
    {
        if (IAPManager.Instance.HadPurchased(targetProductId))
        {
            Debug.Log("이미 구매한 상품입니다.");
            return;
        }

        IAPManager.Instance.Purchase(targetProductId);
    }

    public void HidePurchaseButtonGroup()
    {
        PurchaseButtonGroup.SetActive(false);
    }

    public void ShowUseButtonGroup()
    {
        UseButtonGroup.SetActive(true);
        UseButtonGroup.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void HideUseButtonGroup()
    {
        UseButtonGroup.SetActive(false);
    }

    public void UseDice(string currentDiceName) {
        var buttonControllers = FindObjectsOfType<ButtonController>();
        var startController = FindObjectOfType<StartController>();

        // 다른 주사위들은 다 사용하지 않는 걸로 표시해주기
        foreach (ButtonController buttonController in buttonControllers)
        {
            if (buttonController.targetProductId != currentDiceName) {
                buttonController.NotUseDice();
            } else {
                PlayerPrefs.SetInt($"used-{currentDiceName}", 1);
                buttonController.UseButtonGroup.transform.GetChild(0).gameObject.SetActive(false);
                buttonController.UseButtonGroup.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        // 현재 주사위만 사용하는 걸로 표시해주기
    }

    public void UseDice()
    {
        var buttonControllers = FindObjectsOfType<ButtonController>();
        var startController = FindObjectOfType<StartController>();

        // 다른 주사위들은 다 사용하지 않는 걸로 표시해주기
        foreach (ButtonController buttonController in buttonControllers)
        {
            buttonController.NotUseDice();
        }

        // 현재 주사위만 사용하는 걸로 표시해주기
        PlayerPrefs.SetInt($"used-{targetProductId}", 1);
        
        // 장착가능 버튼
        UseButtonGroup.transform.GetChild(0).gameObject.SetActive(false);
        // 장착완료 버튼
        UseButtonGroup.transform.GetChild(1).gameObject.SetActive(true);        

        // 현재 주사위를 StartController Display에 반영하기
        startController.SetCurrentDiceDisplay();
    }

    public void NotUseDice()
    {
        PlayerPrefs.SetInt($"used-{targetProductId}", 0);

        UseButtonGroup.transform.GetChild(0).gameObject.SetActive(true);
        UseButtonGroup.transform.GetChild(1).gameObject.SetActive(false);
    }
}
