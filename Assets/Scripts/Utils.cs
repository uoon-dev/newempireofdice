
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    public static int GetTimeStamp() {
        return (System.Int32)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
    }

    public static async Task<int> GetLiveTimeStampAsync() {
        var response = await HTTPRequestController.GetResponseAsync(Constants.API_ENDPOINT.TIME_STAMP);
        return JsonUtility.FromJson<HTTPResponse.TimeStampResponse>(response).timestamp;
    }

    public static bool IsNetworkConnected() => Application.internetReachability != NetworkReachability.NotReachable;

    public static async Task<bool> IsDeviceTimeValid(){
        try {
            int localTimestamp = GetTimeStamp();
            int liveTimestamp = await GetLiveTimeStampAsync();
            bool isDeviceTimeValid = Mathf.Abs(liveTimestamp - localTimestamp) < Constants.TIMESTAMP_VALID_OFFSET_SECONDS;
            return isDeviceTimeValid;

        } catch (System.Exception e) {
            return false;
        }
    }

    public static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
