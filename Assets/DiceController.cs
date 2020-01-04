using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        Dice[] dices = FindObjectsOfType<Dice>();
        int destroyedDiceCount = 0;

        foreach (Dice dice in dices)
        {
            if (dice.IsDestroyed() == true) destroyedDiceCount++;
        }

        return destroyedDiceCount;
    }    
}
