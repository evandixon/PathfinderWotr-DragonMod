using DragonMod.Infrastructure;
using HarmonyLib;
using System;
using System.IO;
using System.Runtime.Remoting.Contexts;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace DragonMod
{
    public class Main
    {
        public static ModContextTTTBase DragonModContext;
        public static bool Enabled;

        private static object logLock = new object();

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            DragonModContext = new ModContextTTTBase(modEntry);
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                DragonModContext.ModEntry.OnSaveGUI = OnSaveGUI;
                DragonModContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
                harmony.PatchAll();
                PostPatchInitializer.Initialize(DragonModContext);
                return true;
            }
            catch (Exception e)
            {
                Log(e.ToString());
                throw e;
            }
        }

        public static void Log(string msg)
        {

            lock (logLock)
            {
                var path = @"C:\Users\evanl\AppData\LocalLow\Owlcat Games\Pathfinder Wrath Of The Righteous\dragon.txt";
                File.AppendAllText(path, DateTime.Now.ToString() + " - " + msg + "\n");
            }

            DragonModContext.Logger.Log(msg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg)
        {

            lock (logLock)
            {
                var path = @"C:\Users\evanl\AppData\LocalLow\Owlcat Games\Pathfinder Wrath Of The Righteous\dragon.txt";
                File.AppendAllText(path, DateTime.Now.ToString() + " (Debug) - " + msg + "\n");
            }
            DragonModContext.Logger.Log(msg);
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            DragonModContext.SaveAllSettings();
        }
    }
}
