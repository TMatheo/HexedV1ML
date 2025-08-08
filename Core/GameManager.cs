using LUXED.Interfaces;
using LUXED.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LUXED.Core
{
    internal class GameManager
    {
        private static readonly List<IHook> LoadedHooks = new List<IHook>();
        public static void CreateHooks()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                if (!typeof(IHook).IsAssignableFrom(type)) continue;

                IHook instance = (IHook)Activator.CreateInstance(type);

                try
                {
                    if (GameUtils.IsInVr())
                    {
                        if (instance is IDesktopOnly updateableModule) continue;
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"Desktop Hook:{instance.GetType().Name}...");
                        }
                        instance.Initialize();
                        LoadedHooks.Add(instance);
                    }
                    else
                    {
                        if (instance is IVROnly updateableModule) continue;
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"VR Hook:{instance.GetType().Name}...");
                        }
                        instance.Initialize();
                        LoadedHooks.Add(instance);
                    }
                }
                catch (Exception e)
                {
                    HDLogger.LogError($"Failed to Hook {instance.GetType().Name}: {e}");
                }
            }
            HDLogger.LogDebug($"Loaded {LoadedHooks.Count} Hooks");
        }
        private static readonly List<IQuickMenu> LoadedQuickMenuModules = new List<IQuickMenu>();
        public static void CreateQuickMenuModules()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;
                if (!typeof(IQuickMenu).IsAssignableFrom(type)) continue;
                IQuickMenu instance = (IQuickMenu)Activator.CreateInstance(type);
                try
                {
                    if (instance is IQuickMenu updateableModule) continue;
                    if (Main.IsDebug)
                    {
                        HDLogger.LogDebug($"QuickMenu Global Hook:{instance.GetType().Name}...");
                    }
                    instance.QMInitialize();
                    LoadedQuickMenuModules.Add(instance);
                }
                catch (Exception e)
                { 
                }
            }
        }
        private static readonly List<IGlobalModule> LoadedGlobalModules = new List<IGlobalModule>();
        public static void CreateGlobalModules()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                if (!typeof(IGlobalModule).IsAssignableFrom(type)) continue;

                IGlobalModule instance = (IGlobalModule)Activator.CreateInstance(type);

                try
                {
                    if (GameUtils.IsInVr())
                    {
                        if (instance is IDesktopOnly updateableModule) continue;
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"Desktop Global Hook:{instance.GetType().Name}...");
                        }
                        instance.Initialize();
                        LoadedGlobalModules.Add(instance);
                    }
                    else
                    {
                        if (instance is IVROnly updateableModule) continue;
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"VR Global Hook:{instance.GetType().Name}...");
                        }
                        instance.Initialize();
                        LoadedGlobalModules.Add(instance);
                    }
                }
                catch (Exception e)
                {
                    HDLogger.LogError($"Failed to Initialize {instance.GetType().Name}: {e}");
                }
            }
            HDLogger.LogDebug($"Loaded {LoadedGlobalModules.Count} Global Modules");
        }
        private static float GlobalModuleDelayMs = UnityEngine.Time.time;
        public static void UpdateGlobalModules()
        {
            bool refreshTimer = false;
            foreach (IGlobalModule instance in LoadedGlobalModules)
            {
                if (instance is IDelayModule delayedModule)
                {
                    if (UnityEngine.Time.time > GlobalModuleDelayMs)
                    {
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"Update Hook:{instance.GetType().Name}...");
                        }
                        instance.OnUpdate();
                        refreshTimer = true;
                    }
                }
                else instance.OnUpdate();
            }
            if (refreshTimer) GlobalModuleDelayMs = UnityEngine.Time.time + 0.15f;
        }
        private static readonly Dictionary<int, List<IPlayerModule>> LoadedPlayerModules = new Dictionary<int, List<IPlayerModule>>();
        public static void CreatePlayerModules(VRCPlayer player)
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (Type type in types)
            {
                if (type.IsAbstract) continue;

                if (!typeof(IPlayerModule).IsAssignableFrom(type)) continue;

                IPlayerModule instance = (IPlayerModule)Activator.CreateInstance(type);

                int Actor = player.GetPhotonPlayer().ActorID();

                try
                {
                    if (GameUtils.IsInVr())
                    {
                        if (instance is IDesktopOnly updateableModule) continue;
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"Player DSK Hook:{instance.GetType().Name}...");
                        }
                        instance.Initialize(player);

                        if (!LoadedPlayerModules.ContainsKey(Actor)) LoadedPlayerModules.Add(Actor, new List<IPlayerModule>() { instance });
                        else LoadedPlayerModules[Actor].Add(instance);
                    }
                    else
                    {
                        if (instance is IVROnly updateableModule) continue;
                        if (Main.IsDebug)
                        {
                            HDLogger.LogDebug($"Player VR Hook:{instance.GetType().Name}...");
                        }
                        instance.Initialize(player);

                        if (!LoadedPlayerModules.ContainsKey(Actor)) LoadedPlayerModules.Add(Actor, new List<IPlayerModule>() { instance });
                        else LoadedPlayerModules[Actor].Add(instance);
                    }
                }
                catch (Exception e)
                {
                    HDLogger.LogError($"Failed to Initialize {instance.GetType().Name}: {e}");
                }
            }
            //HDLogger.LogDebug($"Loaded {LoadedPlayerModules.Count} Player Modules");
        }
        public static IPlayerModule GetPlayerModuleByClass(int Actor, Type Class)
        {
            if (LoadedPlayerModules.TryGetValue(Actor, out List<IPlayerModule> Modules))
            {
                return Modules.FirstOrDefault(x => x.GetType() == Class);
            }
            return null;
        }
        public static void DestroyPlayerModules(int Actor)
        {
            if (LoadedPlayerModules.ContainsKey(Actor))
                if (Main.IsDebug)
                {
                    HDLogger.LogDebug($"Destroy Player Module:{Actor}...");
                }
            LoadedPlayerModules.Remove(Actor);
            if (PlayerModuleDelay.ContainsKey(Actor))
                if (Main.IsDebug)
                {
                    HDLogger.LogDebug($"Destroy Player Module Delay:{Actor}...");
                }
            PlayerModuleDelay.Remove(Actor);
        }
        public static void DestroyAllPlayerModules()
        {
            LoadedPlayerModules.Clear();
            PlayerModuleDelay.Clear();
        }
        private static readonly Dictionary<int, float> PlayerModuleDelay = new Dictionary<int, float>();
        public static void UpdatePlayerModules(int Actor)
        {
            bool refreshTimer = false;

            foreach (var instance in LoadedPlayerModules.Where(x => x.Value != null && x.Key == Actor))
            {
                foreach (var module in instance.Value)
                {
                    if (module is IDelayModule delayedModule)
                    {
                        if (!PlayerModuleDelay.ContainsKey(Actor))
                        {
                            if (Main.IsDebug)
                            {
                                HDLogger.LogDebug($"Update Player Add:{Actor}...");
                            }
                            PlayerModuleDelay.Add(Actor, UnityEngine.Time.time);
                        }

                        if (UnityEngine.Time.time > PlayerModuleDelay[Actor])
                        {
                            if (Main.IsDebug)
                            {
                                HDLogger.LogDebug($"Update Player Update:{module.GetType().Name}...");
                            }
                            module.OnUpdate();
                            refreshTimer = true;
                        }
                    }
                    else module.OnUpdate();
                }
            }
            if (refreshTimer) PlayerModuleDelay[Actor] = UnityEngine.Time.time + 0.15f;
        }
    }
}
