using UnityEngine;
using System.Collections;
using System;

public class Constants : MonoBehaviour
{
    public const string MaldivesDice = "maldivesdice";
    public const string GoldrushDice = "goldrushdice";
    public const string SmallHeart = "smallheart";
    public const string LargeHeart = "largeheart";
    public const string HeartRechargeSpeedUp = "speedupheartrecharge1";
    public const int TIMESTAMP_VALID_OFFSET_SECONDS = 5;

    public static class API_ENDPOINT {
        public const string TIME_STAMP = "https://cueyedcuq4.execute-api.ap-northeast-2.amazonaws.com/default/get-time-stamp";
    }

    public static class HTTPResponse
    {
        [Serializable]
        public class TimeStampResponse {
            public Int32 timestamp;
        }
    }    
}
