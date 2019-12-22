#if UNITY_ANDROID
using UnityEngine;
using UnityEditor.Android;
using System.IO;
using System.Xml;

namespace Yodo1Ads
{
#if UNITY_2018_1_OR_NEWER
    public class Yodo1PostGenerateGradleAndroidProject: IPostGenerateGradleAndroidProject
    {
        public int callbackOrder
        {
            get { return 100; }
        }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Yodo1ValidateGradle(path);
        }

        static void Yodo1ValidateGradle(string path)
        {
            var gradlePath = Path.Combine(path, "build.gradle");
            Debug.LogFormat("[Yodo1 Ads] Updating gradle for Play Instant: {0}", gradlePath);
            WriteBelow(gradlePath, "defaultConfig {", "\t\tmultiDexEnabled true");
        }

        static bool WriteBelow(string filePath, string below, string text)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text_all = streamReader.ReadToEnd();
            streamReader.Close();

            int beginIndex = text_all.LastIndexOf(below);
            if (beginIndex == -1)
            {
                Debug.LogError("[Yodo1 Ads] Error parsing " + filePath + ", tag for " + below + " not found.");
                return false;
            }

            if (text_all.IndexOf(text) == -1)
            {
                int endIndex = beginIndex + below.Length;

                text_all = text_all.Substring(0, endIndex) + "\n" + text + /*"\n" +*/ text_all.Substring(endIndex);

                StreamWriter streamWriter = new StreamWriter(filePath);
                streamWriter.Write(text_all);
                streamWriter.Close();
                return true;
            }
            return false;
        }
    }
#endif
}
#endif