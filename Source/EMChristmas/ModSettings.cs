using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using UnityEngine;
using RimWorld;

namespace EMChristmas
{
    public class EMChristmas_ModSettings : ModSettings
    {
        public bool ChristmasUnlocked;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref ChristmasUnlocked, "christmasUnlocked");
            base.ExposeData();
        }

        public void DoWindowContents(Rect wrect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            Color color = GUI.color;
            listing_Standard.Begin(wrect);
            GUI.color = color;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            listing_Standard.Gap(12f);
            listing_Standard.Label("EMChristmas.ChristmasDate".Translate(), -1f, null);
            listing_Standard.CheckboxLabeled("EMChristmas.Unlocked".Translate(), ref ChristmasUnlocked, 12f);
            listing_Standard.End();
            EMChristmas.settings.Write();
        }
    }

    public class EMChristmas : Mod
    {
        public static EMChristmas_ModSettings settings;
        public EMChristmas(ModContentPack content) : base(content)
        {
            settings = GetSettings<EMChristmas_ModSettings>();
        }

        public override string SettingsCategory()
        {
            return "EMChristmas.SettingsCategory".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }
}
