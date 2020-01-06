using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public static class HTTP_METHOD {
    public const string GET = "GET";
}

public static class HTTPResponse
{
    [Serializable]
    public class TimeStampResponse
    {
        public Int32 timestamp;
    }
}


public class HTTPRequestController
{
    public static HttpClient httpClient = new HttpClient();


    public static async Task<string> GetResponseAsync(string url)
    {
        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
        httpResponse.EnsureSuccessStatusCode();
        string responseBody = await httpResponse.Content.ReadAsStringAsync();
        return responseBody;
    }
}
