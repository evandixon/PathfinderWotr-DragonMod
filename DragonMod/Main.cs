using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityModManagerNet;
using DragonMod.Infrastructure;

namespace DragonMod
{
    public class Main
    {
        public static ModContextTTTBase IsekaiContext;
        public static bool Enabled;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            IsekaiContext = new ModContextTTTBase(modEntry);
            try
            {
                var harmony = new Harmony(modEntry.Info.Id);
                IsekaiContext.ModEntry.OnSaveGUI = OnSaveGUI;
                IsekaiContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
                harmony.PatchAll();
                PostPatchInitializer.Initialize(IsekaiContext);
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

            IsekaiContext.Logger.Log(msg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void LogDebug(string msg)
        {
            IsekaiContext.Logger.Log(msg);
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            IsekaiContext.SaveAllSettings();
        }
    }
}
