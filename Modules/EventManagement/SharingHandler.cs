using UnityEngine;

namespace LUXED.Modules.EventManagement
{
    internal class SharingHandler
    {
        public static void DeletePortals()
        {
            foreach (PortalInternal portal in Resources.FindObjectsOfTypeAll<PortalInternal>())
            {
                if (portal.gameObject.activeInHierarchy) UnityEngine.Object.Destroy(portal.gameObject);
            }
        }
    }
}
