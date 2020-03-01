using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.Xml;

namespace Yodo1Ads
{
    public class Yodo1PostProcess
    {
        [PostProcessBuild()]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget == BuildTarget.iOS)
            {
#if UNITY_IOS
                Yodo1AdSettings settings = Yodo1AdSettingsSave.Load();
                if (CheckConfiguration_iOS(settings))
                {
                    UpdateIOSPlist(pathToBuiltProject, settings);
                    UpdateIOSProject(pathToBuiltProject);
                }
#endif
            }
            if (buildTarget == BuildTarget.Android)
            {
#if UNITY_ANDROID
                Yodo1AdSettings settings = Yodo1AdSettingsSave.Load();
                if (CheckConfiguration_Android(settings))
                {
                    ValidateManifest(settings);
                }
#endif
            }
        }

        #region iOS Content

        public static bool CheckConfiguration_iOS(Yodo1AdSettings settings)
        {
            if (settings == null)
            {
                string message = "MAS iOS settings is null, please check the configuration.";
                Debug.LogError("[Yodo1 Ads] " + message);
                Yodo1Utils.ShowAlert("Error", message, "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(settings.iOSSettings.AppKey))
            {
                string message = "MAS iOS AppKey is null, please check the configuration.";
                Debug.LogError("[Yodo1 Ads] " + message);
                Yodo1Utils.ShowAlert("Error", message, "Ok");
                return false;
            }

            if (settings.iOSSettings.GlobalRegion && string.IsNullOrEmpty(settings.iOSSettings.AdmobAppID))
            {
                string message = "MAS iOS AdMob App ID is null, please check the configuration.";
                Debug.LogError("[Yodo1 Ads] " + message);
                Yodo1Utils.ShowAlert("Error", message, "Ok");
                return false;
            }
            return true;
        }

#if UNITY_IOS

        private static void UpdateIOSPlist(string path, Yodo1AdSettings settings)
        {
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            //Get Root
            PlistElementDict rootDict = plist.root;
            PlistElementDict transportSecurity = rootDict.CreateDict("NSAppTransportSecurity");
            transportSecurity.SetBoolean("NSAllowsArbitraryLoads", true);

            //Set AppLovinSdkKey
            rootDict.SetString("AppLovinSdkKey", Yodo1EditorConstants.DEFAULT_APPLOVIN_SDK_KEY);

            //Set AdMob APP Id
            if(settings.iOSSettings.GlobalRegion)
            {
                rootDict.SetString("GADApplicationIdentifier", settings.iOSSettings.AdmobAppID);
            }

            PlistElementString privacy = (PlistElementString)rootDict["NSLocationAlwaysUsageDescription"];
            if (privacy == null)
            {
                rootDict.SetString("NSLocationAlwaysUsageDescription", "Some ad content may require access to the location for an interactive ad experience.");
            }

            PlistElementString privacy1 = (PlistElementString)rootDict["NSLocationWhenInUseUsageDescription"];
            if (privacy1 == null)
            {
                rootDict.SetString("NSLocationWhenInUseUsageDescription", "Some ad content may require access to the location for an interactive ad experience.");
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static void UpdateIOSProject(string path)
        {
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);

            string target = proj.TargetGuidByName("Unity-iPhone");

            proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC -lxml2");

            //Add Frameworks
            proj.AddFrameworkToProject(target, "CoreFoundation.framework", false);
            proj.AddFrameworkToProject(target, "AdSupport.framework", false);
            proj.AddFrameworkToProject(target, "EventKit.framework", false);
            proj.AddFrameworkToProject(target, "EventKitUI.framework", false);
            proj.AddFrameworkToProject(target, "CoreData.framework", false);
            proj.AddFrameworkToProject(target, "Photos.framework", false);
            proj.AddFrameworkToProject(target, "OpenGLES.framework", false);
            proj.AddFrameworkToProject(target, "UIKit.framework", false);
            proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);
            proj.AddFrameworkToProject(target, "QuartzCore.framework", false);
            proj.AddFrameworkToProject(target, "MobileCoreServices.framework", false);
            proj.AddFrameworkToProject(target, "ImageIO.framework", false);
            proj.AddFrameworkToProject(target, "Foundation.framework", false);
            proj.AddFrameworkToProject(target, "CoreGraphics.framework", false);
            proj.AddFrameworkToProject(target, "CFNetwork.framework", false);
            proj.AddFrameworkToProject(target, "AudioToolbox.framework", false);
            proj.AddFrameworkToProject(target, "AssetsLibrary.framework", false);
            proj.AddFrameworkToProject(target, "StoreKit.framework", false);
            proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
            proj.AddFrameworkToProject(target, "CoreText.framework", false);
            proj.AddFrameworkToProject(target, "MessageUI.framework", false);
            proj.AddFrameworkToProject(target, "CoreLocation.framework", false);
            proj.AddFrameworkToProject(target, "AddressBook.framework", false);
            proj.AddFrameworkToProject(target, "Accounts.framework", false);
            proj.AddFrameworkToProject(target, "Social.framework", false);
            proj.AddFrameworkToProject(target, "MediaPlayer.framework", false);
            proj.AddFrameworkToProject(target, "AVFoundation.framework", false);
            proj.AddFrameworkToProject(target, "CoreMedia.framework", false);
            proj.AddFrameworkToProject(target, "CoreMotion.framework", false);
            proj.AddFrameworkToProject(target, "WebKit.framework", false);
            proj.AddFrameworkToProject(target, "GameController.framework", false);
            proj.AddFrameworkToProject(target, "WatchConnectivity.framework", true);
            proj.AddFrameworkToProject(target, "GLKit.framework", true);

            string tbdPath = "/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS.sdk/usr/lib/";

            if (!Directory.Exists(tbdPath))
            {
                //Add dylibs
                proj.AddFrameworkToProject(target, "libsqlite3.dylib", true);
                proj.AddFrameworkToProject(target, "libz.dylib", true);
                proj.AddFrameworkToProject(target, "libz.1.2.5.dylib", true);
                proj.AddFrameworkToProject(target, "libresolv.dylib", true);
                proj.AddFrameworkToProject(target, "libicucore.dylib", true);
                proj.AddFrameworkToProject(target, "libresolv.9.dylib", true);
                proj.AddFrameworkToProject(target, "libc++.dylib", true);
            }
            else
            {
                //add tbds
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.tbd", "Frameworks/libsqlite3.tbd", PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.1.2.5.tbd", "Frameworks/libz.1.2.5.tbd", PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libresolv.tbd", "Frameworks/libresolv.tbd", PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libicucore.tbd", "Frameworks/libicucore.tbd", PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libresolv.9.tbd", "Frameworks/libresolv.9.tbd", PBXSourceTree.Sdk));
                proj.AddFileToBuild(target, proj.AddFile("usr/lib/libc++.tbd", "Frameworks/libc++.tbd", PBXSourceTree.Sdk));
            }

            // rewrite to file
            File.WriteAllText(projPath, proj.WriteToString());
        }

#endif

        #endregion

        #region Android Content

        public static bool CheckConfiguration_Android(Yodo1AdSettings settings)
        {
            if (settings == null)
            {
                string message = "MAS Android settings is null, please check the configuration.";
                Debug.LogError("[Yodo1 Ads] " + message);
                Yodo1Utils.ShowAlert("Error", message, "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(settings.androidSettings.AppKey))
            {
                string message = "MAS Android AppKey is null, please check the configuration.";
                Debug.LogError("[Yodo1 Ads] " + message);
                Yodo1Utils.ShowAlert("Error", message, "Ok");
                return false;
            }

            if (settings.androidSettings.GooglePlayStore && string.IsNullOrEmpty(settings.androidSettings.AdmobAppID))
            {
                string message = "MAS Android AdMob App ID is null, please check the configuration.";
                Debug.LogError("[Yodo1 Ads] " + message);
                Yodo1Utils.ShowAlert("Error", message, "Ok");
                return false;
            }
            return true;
        }

        static void GenerateManifest(Yodo1AdSettings settings)
        {
            var outputFile = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
            if (!File.Exists(outputFile))
            {
                var inputFile = Path.Combine(EditorApplication.applicationContentsPath, "PlaybackEngines/androidplayer/AndroidManifest.xml");
                if (!File.Exists(inputFile))
                {
                    inputFile = Path.Combine(EditorApplication.applicationContentsPath, "PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml");
                }
                if (!File.Exists(inputFile))
                {
                    string s = EditorApplication.applicationPath;
                    int index = s.LastIndexOf("/");
                    s = s.Substring(0, index + 1);
                    inputFile = Path.Combine(s, "PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml");
                }
                File.Copy(inputFile, outputFile);
            }
            ValidateManifest(settings);
        }

        public static bool ValidateManifest(Yodo1AdSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError("[Yodo1 Ads] Validate manifest failed. Yodo1 ad settings is not exsit.");
                return false;
            }

            var androidPluginPath = Path.Combine(Application.dataPath, "Plugins/Android/");
            var manifestFile = androidPluginPath + "AndroidManifest.xml";
            if (!File.Exists(manifestFile))
            {
                GenerateManifest(settings);
                return true;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(manifestFile);

            if (doc == null)
            {
                Debug.LogError("[Yodo1 Ads] Couldn't load " + manifestFile);
                return false;
            }

            XmlNode manNode = FindChildNode(doc, "manifest");
            string ns = manNode.GetNamespaceOfPrefix("android");

            XmlNode app = FindChildNode(manNode, "application");

            if (app == null)
            {
                Debug.LogError("[Yodo1 Ads] Error parsing " + manifestFile + ", tag for application not found.");
                return false;
            }

            ////Enable hardware acceleration for video play
            //XmlElement elem = (XmlElement)app;

            //Add AdMob App ID
            if (settings.androidSettings.GooglePlayStore)
            {
                string admobAppIdValue = settings.androidSettings.AdmobAppID;
                if (string.IsNullOrEmpty(admobAppIdValue))
                {
                    Debug.LogError("[Yodo1 Ads] MAS Android AdMob App ID is null, please check the configuration.");
                    return false;
                }
                string admobAppIdName = "com.google.android.gms.ads.APPLICATION_ID";
                XmlNode metaNode = FindChildNodeWithAttribute(app, "meta-data", "android:name", admobAppIdName);
                if (metaNode == null)
                {
                    metaNode = (XmlElement)doc.CreateNode(XmlNodeType.Element, "meta-data", null);
                    app.AppendChild(metaNode);
                }

                XmlElement metaElement = (XmlElement)metaNode;
                metaElement.SetAttribute("name", ns, admobAppIdName);
                metaElement.SetAttribute("value", ns, admobAppIdValue);
                metaElement.GetNamespaceOfPrefix("android");
            }

            doc.Save(manifestFile);
            return true;
        }

        static XmlNode FindChildNode(XmlNode parent, string name)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name))
                {
                    return curr;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        static XmlNode FindChildNodeWithAttribute(XmlNode parent, string name, string attribute, string value)
        {
            XmlNode curr = parent.FirstChild;
            while (curr != null)
            {
                if (curr.Name.Equals(name) && curr.Attributes.GetNamedItem(attribute) != null && curr.Attributes[attribute].Value.Equals(value))
                {
                    return curr;
                }
                curr = curr.NextSibling;
            }
            return null;
        }

        #endregion

    }
}