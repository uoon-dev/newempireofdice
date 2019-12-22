#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("21hDx1oiUmKmT9GDYvN6/VED1xCcMG6XhZpkxmhqGT3eFK8712vOIg+9Ph0PMjk2Fbl3ucgyPj4+Oj883+tMeWHStgvSG/Ea+/ctAd4A2ZF7q6x1kq5Ma396PSbTC26NKXutHb0+MD8PvT41Pb0+Pj+uaVzXcMz7OjWtffgEwYPHXfp4CdPaHw/1wJRjhVs4RIkD6wgVoYxykoYz4b/fTH4XUdK0cjIWYXqeFZhraYzucSJ3TF2JYM0sB8bQG93KNfurTZCzmp80p7dYNVNE0kgc/nzZvw1IS8DRN26KFKOGexVurDLAY1cfvnCVRXBPZ6jQNAapbBCB1f5UF3NThmJt5jqcBymV6UhFUGqKMdgyqXnLcvmt8/sWPY5+M/O4CD08Pj8+");
        private static int[] order = new int[] { 11,5,11,7,6,5,11,10,10,11,10,11,12,13,14 };
        private static int key = 63;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
