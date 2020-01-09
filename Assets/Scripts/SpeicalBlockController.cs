using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public static class Helper
{
    public static object GetPropertyValue(this object T, string PropName)
    {
        if (T == null) return null;
        return T.GetType().GetProperty(PropName) == null ? null : T.GetType().GetProperty(PropName).GetValue(T, null);
    }

}

public class SpeicalBlockController : MonoBehaviour
{
    public static int mineCount = 0;
    public static int dungeonCount = 0;
    public static int armyCount = 0;
    public static int wizardCount = 0;
    public static int relicsCount = 0;
    public static int horizontalCount = 0;
    public static int verticalCount = 0;
    public static int bombCount = 0;
    public static int lastBlockCount = 0;
    private static int createdMineCount = 0;
    private static int createdArmyCount = 0;
    private static int createdWizardCount = 0;
    private static int createdRelicsCount = 0;
    private static int createdDungeonCount = 0;
    private static int createdHorizontalCount = 0;
    private static int createdVerticalCount = 0;
    private static int createdBombCount = 0;
    static List<int> blockNumberList = null;

    static string[] blocksType = { "광산", "던전", "용병", "기병대", "공습", "폭탄", "마법사", "유물" };
    LevelLoader levelLoader;

    void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }   


    public void SetSpeicialBlocks(object speicalBlocks,bool placeSpecialBlock = true)
    {
        if (levelLoader.GetCurrentSceneName() != "Level") return;

        if (speicalBlocks != null) 
        {
            dungeonCount = (int)speicalBlocks.GetPropertyValue("dungeon");
            mineCount = (int)speicalBlocks.GetPropertyValue("mine");
            armyCount = (int)speicalBlocks.GetPropertyValue("army");
            horizontalCount = (int)speicalBlocks.GetPropertyValue("horizontal");
            verticalCount = (int)speicalBlocks.GetPropertyValue("vertical");
            bombCount = (int)speicalBlocks.GetPropertyValue("bomb");
            wizardCount = (int)speicalBlocks.GetPropertyValue("wizard");
            relicsCount = (int)speicalBlocks.GetPropertyValue("relics");
            lastBlockCount = (int)speicalBlocks.GetPropertyValue("lastblock");
        }

        var blocks = FindObjectsOfType<Block>();

        if (blocks.Length > 0&& placeSpecialBlock)
        {
            int[] blockNumbers = new int[blocks.Length];
            for (int i = 0; i < blockNumbers.Length; i++)
            {
                blockNumbers[i] = i;
            }

            blockNumberList = new List<int>(blockNumbers);

            PlaceSpecialBlockRandomly(mineCount, createdMineCount, blocks, 0);
            PlaceSpecialBlockRandomly(dungeonCount, createdDungeonCount, blocks, 1);
            PlaceSpecialBlockRandomly(armyCount, createdArmyCount, blocks, 2);
            PlaceSpecialBlockRandomly(horizontalCount, createdHorizontalCount, blocks, 3);
            PlaceSpecialBlockRandomly(verticalCount, createdVerticalCount, blocks, 4);
            PlaceSpecialBlockRandomly(bombCount, createdBombCount, blocks, 5);
            PlaceSpecialBlockRandomly(wizardCount, createdWizardCount, blocks, 6);
            PlaceSpecialBlockRandomly(relicsCount, createdRelicsCount, blocks, 7);
        }

        foreach (Block block in blocks)
        {
            block.SetTooltipInfo();
        }
    }

    public static void PlaceSpecialBlockRandomly(int speicalBlockCount, int createdSpeicalBlockCount, Block[] blocks, int typeIndex)
    {
        if (createdSpeicalBlockCount < blockNumberList.Count)
        {
            if (speicalBlockCount > blockNumberList.Count - 2)
            {
                speicalBlockCount = blockNumberList.Count - 2;
            }

            while (createdSpeicalBlockCount < speicalBlockCount)
            {
                int randomNum = Random.Range(0, blockNumberList.Count);
                Block block = blocks[blockNumberList[randomNum]];
                int maxSize = (int)Math.Floor(Math.Sqrt(blocks.Length));
                if (!(block.GetPosX() == 1 && block.GetPosY() == 1) &&
                    !(block.GetPosX() == maxSize && block.GetPosY() == maxSize))
                {
                    block.SetBlockType(blocksType[typeIndex]);
                    createdSpeicalBlockCount++;
                    blockNumberList.RemoveAt(randomNum);
                }
            }
        }
    }

    public void IncreaseLastBlockGage()
    {
        GameObject lastBlock = BlockController.lastBlock;
        Text lastBlockText = lastBlock.GetComponentInChildren<Text>();
        string lastblockNumber = lastBlockText.text;
        lastblockNumber = String.Empty != lastblockNumber ? (int.Parse(lastblockNumber) + 1).ToString() : "";
        lastBlockText.text = lastblockNumber;
    }

    public void IncreaseDiceNumber(int powerUpGage)
    {
        var resetDiceButton = FindObjectOfType<ResetDiceController>();
        var Dices = GameObject.Find("Dices");
        Dice[] dices = Dices.GetComponentsInChildren<Dice>();

        foreach (Dice dice in dices)
        {
            if (powerUpGage == 1)
            {
                dice.PowerUpDice(powerUpGage);
            }
            else
            {
                dice.PowerUpDice(dice.GetCurrentNumber());
            }
        }
    }

    //public static void SetSpeicialBlocks(object speicalBlocks) {
    //    dungeonCount = (int)speicalBlocks.GetPropertyValue("dungeon");
    //    mineCount = (int)speicalBlocks.GetPropertyValue("mine");
    //    armyCount = (int)speicalBlocks.GetPropertyValue("army");
    //    horizontalCount = (int)speicalBlocks.GetPropertyValue("horizontal");
    //    verticalCount = (int)speicalBlocks.GetPropertyValue("vertical");
    //    bombCount = (int)speicalBlocks.GetPropertyValue("bomb");
    //    wizardCount = (int)speicalBlocks.GetPropertyValue("wizard");
    //    relicsCount = (int)speicalBlocks.GetPropertyValue("relics");
    //    lastBlockCount = (int)speicalBlocks.GetPropertyValue("lastblock");
    //}

    public int GetLastBlockNumber()
    {
        return lastBlockCount;
    }
}
