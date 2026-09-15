using System;using BepInEx;using HarmonyLib;
[BepInPlugin("gkugfk3.httpfree", "HttpFree", "1.0.0")]
public class NetLinkHTTP : BaseUnityPlugin
{
    private void Awake()
    {
        Harmony harmony = new Harmony("gkugfk3.httpfree");
        harmony.PatchAll();
        Logger.LogInfo("[gkugfk3] httpfree patch loaded");
    }
}
[HarmonyPatch]
public static class NetLinkAgentPatch
{
    static System.Reflection.MethodBase TargetMethod()
    {
        Type type = AccessTools.TypeByName("NetLinkAgent");

        if (type == null)
            return null;

        return AccessTools.Method(type, "IsUrlValid");
    }
    static bool Prefix(string url, ref bool __result)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            __result = false;
            return false;
        }
        Uri uri = new Uri(url);
        __result =
            (uri.Scheme == Uri.UriSchemeHttps ||
             uri.Scheme == Uri.UriSchemeHttp) &&
            !uri.IsFile;
        return false;
    }
}