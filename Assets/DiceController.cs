using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public int GetClickedDiceCount()
    {
        var dices = FindObjectsOfType<Dice>();
        int clickedDiceCount = 0;

        foreach (Dice dice in dices)
        {
            if (dice.CheckIsClicked())
            {
                clickedDiceCount++;
            }
        }

        return clickedDiceCount;
    }

    public void DestroyDices()
    {
        var dices = FindObjectsOfType<Dice>();

        foreach (Dice dice in dices)
        {
            if (dice.CheckIsClicked())
            {
                dice.DestoryDice();
            }
        }        
    }

    public int GetDestroyedDiceCount()
    {
        var dices = FindObjectsOfType<Dice>();
        int destroyedDiceCount = 0;

        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed() == true) destroyedDiceCount++;
        }

        return destroyedDiceCount;
    }

    public int GetDiceNumberRandomly()
    {
        var dices = FindObjectsOfType<Dice>();

        List<int> diceNumbers = new List<int>();
        foreach (Dice dice in dices)
        {
            if (!dice.IsDestroyed())
            {
                diceNumbers.Add(dice.GetCurrentNumber());
            }
        }

        int randomDiceNumber = diceNumbers[Random.Range(0, 6)];

        return randomDiceNumber;
    }
}
