using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityModManagerNet.UnityModManager;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.ModLogic;
using WotrSandbox.Config;
using static WotrSandbox.Main;

namespace WotrSandbox.Infrastructure
{
    public class ModContextTTTBase : ModContextBase
    {
        public AddedContent AddedContent;

        public ModContextTTTBase(ModEntry ModEntry) : base(ModEntry)
        {
#if DEBUG
            Debug = true;
#endif
            LoadAllSettings();
        }

        public override void LoadAllSettings()
        {
            LoadSettings("AddedContent.json", "WotrSandbox.Config", ref AddedContent);
            LoadBlueprints("WotrSandbox.Config", this);
            LoadLocalization("WotrSandbox.Localization");
        }

        public override void AfterBlueprintCachePatches()
        {
            base.AfterBlueprintCachePatches();
            if (Debug)
            {
                Blueprints.RemoveUnused();
                SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }

        public override void SaveAllSettings()
        {
            base.SaveAllSettings();
            SaveSettings("AddedContent.json", AddedContent);
        }
    }
}
