using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour
{
    public static class API_ENDPOINT {
        public const string TIME_STAMP = "https://cueyedcuq4.execute-api.ap-northeast-2.amazonaws.com/default/get-time-stamp";
    }

    public static class GAME_OBJECT_NAME {
        public const string NO_HEART_CANVAS = "No Heart Canvas";
        public const string AFTER_PURCHASE_EFFECT_CANVAS = "After Purchase Effect Canvas";
        public const string HEART_IMAGE_PARENT_OBJECT = "Heart Images";
        public const string HEART_UPDATED_COUNT_TEXT = "Heart Updated Count Text";
        public const string HEART_TIMER_TEXT = "Heart Timer Text";
        public const string HEART_TIMER_TEXT_IN_NO_HEART_CANVAS = "Heart Timer Text in No Heart Canvas";
        public const string HEART_TIMER_TEXT_IN_SHOP = "Heart Timer Text in Shop";
        public const string HEART_COUNT_TEXT = "Heart Count Text";
        public const string HEART_CONTROLLER = "HeartController";
        public const string STAGE_TEXT = "StageText";
    }

    public const string MaldivesDice = "maldivesdice";
    public const string GoldrushDice = "goldrushdice";
    public const string SmallHeart = "smallheart";
    public const string LargeHeart = "largeheart";
    public const string HeartRechargeSpeedUp = "speedupheartrecharge1";
    public const int HEART_MAX_CHARGE_COUNT = 5;
    public const int TIMESTAMP_VALID_OFFSET_SECONDS = 5;
    public const int HEART_CHARGE_SECONDS = 20 * 60;// 20 min
}
