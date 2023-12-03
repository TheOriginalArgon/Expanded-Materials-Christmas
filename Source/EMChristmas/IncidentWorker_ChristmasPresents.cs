using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EMChristmas
{
    class IncidentWorker_ChristmasPresents : IncidentWorker
    {
        List<ThingDef> smallGiftDefs = null; //This goes in the class level, not inside the method
        List<ThingDef> largeGiftDefs = null; //This goes in the class level, not inside the method

        bool ChristmasUnlocked = LoadedModManager.GetMod<EMChristmas>().GetSettings<EMChristmas_ModSettings>().ChristmasUnlocked;

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            if (((DateTime.Now.Month > 10 && DateTime.Now.Month <= 12) || DateTime.Now.Month == 1) || ChristmasUnlocked)
            {
                Map map = (Map)parms.target;
                if (smallGiftDefs == null) //Only load them if we haven't already
                    smallGiftDefs = DefDatabase<ThingDef>.AllDefsListForReading.Where(d => d.defName.StartsWith("EM_GiftSmall")).ToList();
                if (largeGiftDefs == null) //Only load them if we haven't already
                    largeGiftDefs = DefDatabase<ThingDef>.AllDefsListForReading.Where(d => d.defName.StartsWith("EM_GiftLarge")).ToList();
                var pawn = map.PlayerPawnsForStoryteller.RandomElement(); //Pick someone to gift at.
                var giftMultiplier = Math.Max((int)map.PlayerWealthForStoryteller / 65000, 1);
                List<Thing> giftThings = new List<Thing>();
                var smallGiftCount = Rand.Range(2, 6) * giftMultiplier;
                var largeGiftCount = Rand.Range(1, 4) * giftMultiplier;
                for (int i = 0; i < smallGiftCount; i++)
                {
                    giftThings.Add(ThingMaker.MakeThing(smallGiftDefs.RandomElement()));
                }
                for (int i = 0; i < largeGiftCount; i++)
                {
                    giftThings.Add(ThingMaker.MakeThing(largeGiftDefs.RandomElement()));
                }
                var pawnBed = (from b in map.listerBuildings.allBuildingsColonist where b.def.IsBed && b.TryGetComp<CompAssignableToPawn>().AssignedPawns.Contains(pawn) select b).FirstOrDefault();
                foreach (Thing thing in giftThings)
                {
                    GenSpawn.Spawn(thing, pawnBed == null ? pawn.Position : pawnBed.Position, map);
                }
                //DropPodUtility.DropThingsNear(pawn.Position, map, giftThings);
                TaggedString title = def.letterLabel.Formatted(pawn.LabelShort, pawn.Named("PAWN")).CapitalizeFirst();
                TaggedString text = def.letterText.Formatted(pawn.NameShortColored, pawn.Named("PAWN")).AdjustedFor(pawn).CapitalizeFirst();
                SendStandardLetter(title, text, def.letterDef, parms, pawn);
                //SendStandardLetter("LetterLabelChristmasPresents".Translate(), "ChristmasPresents".Translate(new NamedArgument(pawn, "PAWN")), LetterDefOf.PositiveEvent, parms, new TargetInfo(pawn.Position, map));
                return true;
            }
            else
                return false;
        }
    }
}
