using UnityEngine;
using System.Collections;
using System;

public class Constants : MonoBehaviour
{
    public static class API_ENDPOINT {
        public const string TIME_STAMP = "https://cueyedcuq4.execute-api.ap-northeast-2.amazonaws.com/default/get-time-stamp";
    }

    public static class SCENE_NAME {
        public const string MAP_SYSTEM = "Map System";
        public const string LEVEL = "Level";
        public const string LEVEL1 = "Level 1";
        public const string TUTORIAL = "Tutorial";
    }

    public static class STAGE_REWARD_TYPE {
        public const string HEART_REWARD_FULL = "heartRewardFull";
        public const string HEART_REWARD_ADD = "heartRewardADD";
    }

    public static class GAME_OBJECT_NAME {
        public const string BODY = "Body";
        public const string PANEL_SETTING = "PanelSetting";
        public const string NO_HEART_CANVAS = "No Heart Canvas";
        public const string AFTER_PURCHASE_EFFECT_CANVAS = "After Purchase Effect Canvas";
        public const string HEART_BAR_OBJECT = "Heart Bar";
        public const string HEART_IMAGE_PARENT_OBJECT = "Heart Images";
        public const string HEART_IMAGE_PARENT_OBJECT_IN_EFFECT = "Heart Images In Effect";
        public const string HEART_UPDATED_COUNT_TEXT = "Heart Updated Count Text";
        public const string HEART_TIMER_TEXT = "Heart Timer Text";
        public const string HEART_TIMER_TEXT_IN_NO_HEART_CANVAS = "Heart Timer Text in No Heart Canvas";
        public const string HEART_TIMER_TEXT_IN_SHOP = "Heart Timer Text in Shop";
        public const string HEART_COUNT_TEXT = "Heart Count Text";
        public const string HEART_CONTROLLER = "HeartController";
        public const string STAGE_TEXT = "StageText";
        public const string START_BUTTON = "Start Button";
        public const string RETRY_BUTTON = "Retry Button";
        public const string NEXT_LEVEL_BUTTON = "Next Level Button";
        public const string DICES = "Dices";
        public const string NUMBER_TEXT = "Number Text";
        public const string IMAGE = "Image";
    }

    public static class TUTORIAL {
        public static class GAME_OBJECT_NAME {
            public const string TUTORIAL_GUIDE_CANVAS = "Tutorial Guide Canvas";
            public const string INTRO_CANVAS = "Intro Canvas";
            public const string MAIN_DIALOGUE_CONTAINER = "Main Dialogue Container";
            public const string SUB_DIALOGUE_CONTAINER = "Sub Dialogue Container";
            public const string TEXT_BOX = "Text Box";
            public const string GUIDER_IMAGE = "Guider Image";
            public const string SUPER_TEXT = "Super Text";
            public const string OVAL = "Oval";
            public const string ATTACK_GAGE = "Attack Gage";
            public const string LEFT_AREA = "Left Area";
            public const string RIGHT_AREA = "Right Area";
            public const string OUTLINE = "Outline";
            public const string OUTLINE_DICE = "Outline Dice";
            public const string OUTLINE_CIRCLE = "Outline Circle";
            public const string OUTLINE_RECT = "Outline Rect";
            public const string OUTLINE_FULL_RECT = "Outline Full Rect";
            public const string BLOCKS = "Blocks";
            public const string TOAST = "Toast";
            public const string ARROW = "Arrow";
            public const string TURN = "Turn";
            public const string DESC = "Description";
            public const string GUIDE_ITEM = "Guide Item";
            public const string BACKGROUND_IMAGE = "Background Image";
            public const string COST_IMAGE = "Cost Image";
            public const string MONEY_TEXT = "Money Text";
            public const string MONEY_AREA = "Money Area";
            public const string COST_ICON = "Cost Icon";
            public const string COST_TEXT = "Cost Text";
            public const string ATTACK_POWER_TEXT = "Attack Power Text";
            public const string ATTACK_POWER_IMAGE = "Attack Power Image";
        }
    }

    public static class TYPE {
        public const string LAST_BLOCK = "lastBlock";
        public const string FIRST_BLOCK = "firstBlock";
        public const string MIDDLE_BLOCK = "middleBlock";
        public const string LEFT_MIDDLE_BLOCK = "leftMiddleBlock";
        public const string BOTTOM_MIDDLE_BLOCK = "bottomMiddleBlock";
    }    

    public const string MaldivesDice = "maldivesdice";
    public const string GoldrushDice = "goldrushdice";
    public const string SmallHeart = "smallheart";
    public const string LargeHeart = "largeheart";
    public const string HeartRechargeSpeedUp = "speedupheartrecharge1";
    public const int HEART_MAX_CHARGE_COUNT = 5;
    public const int TIMESTAMP_VALID_OFFSET_SECONDS = 5;
    // public const int HEART_CHARGE_SECONDS = 20;
    public const int HEART_CHARGE_SECONDS = 20 * 60;// 20 min
}
