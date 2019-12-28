using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageController
{
    public static class STORAGE_KEY
    {
        public const string BLOCKS_BY_LEVEL_TYPE = "blockByLevelType";
        public const string BLOCKS_BY_LEVEL_TEXT = "blockByLevelText";
        public const string BACKGROUND_INDEX_BY_LEVEL = "backgroundIndexByLevel";
    }

    public static class STORAGE_TYPE
    {
        public class BlocksData
        {
        }
    }

    public static bool IsKeyExists(string key) => ES3.KeyExists(key);

    public static bool IsBlocksSaved(int level)
    {
        return IsKeyExists(STORAGE_KEY.BLOCKS_BY_LEVEL_TYPE + level);
    }
    public static bool IsBackgroundImageIndexSaved(int level) {
        return IsKeyExists(STORAGE_KEY.BACKGROUND_INDEX_BY_LEVEL + level);
    }
    public static void SaveBlocksType(int level, List<string> blockTypes)
    {
                Debug.Log("saved type called");
        ES3.Save<List<string>>(STORAGE_KEY.BLOCKS_BY_LEVEL_TYPE + level, blockTypes);
    }


    public static void SaveBlocksText(int level, List<string> blockTexts)
    {

        ES3.Save<List<string>>(STORAGE_KEY.BLOCKS_BY_LEVEL_TEXT + level, blockTexts);
    }

    public static void SaveBackgroundImageIndex(int level, int index)
    {
        ES3.Save<int>(STORAGE_KEY.BACKGROUND_INDEX_BY_LEVEL + level, index);
    }

    public static List<string> LoadBlocksType(int level)
    {
        return ES3.Load<List<string>>(STORAGE_KEY.BLOCKS_BY_LEVEL_TYPE + level);
    }

    public static List<string> LoadBlocksText(int level)
    {
        return ES3.Load<List<string>>(STORAGE_KEY.BLOCKS_BY_LEVEL_TEXT + level);
    }

    public static int LoadBackgroundImageIndex(int level)
    {
        return ES3.Load<int>(STORAGE_KEY.BACKGROUND_INDEX_BY_LEVEL + level);
    }

    

}
