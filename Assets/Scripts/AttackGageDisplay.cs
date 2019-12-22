using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackGageDisplay : MonoBehaviour
{
    int gageNum = 0;
    Text gageText; 

    // Start is called before the first frame update
    void Start()
    {
        gageText = GetComponent<Text>();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        gageText.text = gageNum.ToString();
    }

    public void AddGageNum(int amount)
    {
        gageNum += amount;
        UpdateDisplay();
    }

    public void SpendGageNum(int amount)
    {
        if (gageNum >= amount)
        {
            gageNum -= amount;
            UpdateDisplay();
        }
    }

    public string GetAttackGage()
    {
        return gageText.text;
    }

    public void ResetAttackGage()
    {
        gageNum = 0;
        UpdateDisplay();
    }

    public void SumAttackGage()
    {
        gageNum = 0;
        Dice[] dices = FindObjectsOfType<Dice>();
        foreach (Dice dice in dices) 
        {
            if (dice.CheckIsClicked() == true) gageNum += dice.GetCurrentNumber();
        }
        UpdateDisplay();
    }
}
