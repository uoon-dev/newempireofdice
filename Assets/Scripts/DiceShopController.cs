using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceShopController : MonoBehaviour
{
    [SerializeField] GameObject openShopButton = null;

    void Start()
    {
        HideScreen();
    }

    public void ShowScreen()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideScreen()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        this.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
