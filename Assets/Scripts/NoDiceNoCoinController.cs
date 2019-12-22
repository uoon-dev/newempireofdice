using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDiceNoCoinController : MonoBehaviour
{
    void Start()
    {
        HideScreen();
    }

    public void ToggleScreen()
    {
        var dices = FindObjectsOfType<Dice>();
        var resetDiceController = FindObjectOfType<ResetDiceController>();
        int destroyedDiceCount = 0;

        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed())
            {
                destroyedDiceCount++;
            }
        }

        if (destroyedDiceCount == 6 && resetDiceController.GetCurrentMoney() < resetDiceController.GetCost())
        {
            ShowScreen();
        } else 
        {
            HideScreen();
        }
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
