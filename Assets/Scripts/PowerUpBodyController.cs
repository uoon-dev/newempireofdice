using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBodyController : MonoBehaviour
{
    [SerializeField] GameObject pointArea = null;
    [SerializeField] GameObject Dices = null;
    int powerUpGage = 0;
    int cost = 2;

    private void OnMouseDown()
    {
        var resetDiceButton = FindObjectOfType<ResetDiceController>();
        if (resetDiceButton.GetCurrentMoney() >= cost)
        {
            resetDiceButton.SpendCurrentMoney(cost);
            Dice[] dices = Dices.GetComponentsInChildren<Dice>();
            foreach (Dice dice in dices)
            {
                dice.PowerUpDice(powerUpGage);
            }
        }
    }

    public void AddPowerUpGage()
    {
        powerUpGage++;
        Text[] pointTexts = pointArea.GetComponentsInChildren<Text>();
        foreach (Text pointText in pointTexts)
        {
            pointText.text = "+" + (int.Parse(pointText.text.Substring(1, 1)) + 1).ToString();
        }
    }
}
