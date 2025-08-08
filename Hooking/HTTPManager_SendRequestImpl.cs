using BestHTTP;
using HarmonyLib;
using LUXED.Core;
using LUXED.Interfaces;
using LUXED.Wrappers;
using System.Linq;
using System.Reflection;

namespace LUXED.Hooking
{
    internal class HTTPManager_SendRequestImpl : IHook
    {
        public void Initialize()
        {
            EasyHooking.LuxedInstance.Patch(typeof(HTTPManager).GetMethods().LastOrDefault((MethodInfo x) => x.Name.Equals("SendRequestImpl")), new HarmonyMethod(AccessTools.Method(typeof(HTTPManager_SendRequestImpl), "Hook", null, null)), null, null, null, null);
        }
        private static void Hook(HTTPRequest request)
        {
            if (request != null)
            {
                if (InternalSettings.APILog) HDLogger.LogApi(request);

                if (request.Uri != null && request.Uri.ToString().Contains("amplitude.com")) return;
            }
        }
    }
}
