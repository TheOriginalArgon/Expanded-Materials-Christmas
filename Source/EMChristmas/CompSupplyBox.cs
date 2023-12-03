using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace EMChristmas
{

    public class CompProperties_SupplyBox : CompProperties
    {
        public List<ThingDef> containedSupplies;
        public List<int> amountMin;
        public List<int> amountMax;
        public List<int> chances;
        public bool isLocked;

        public CompProperties_SupplyBox()
        {
            compClass = typeof(CompSupplyBox);
        }

        public CompProperties_SupplyBox(Type CompClass) : base(CompClass)
        {
            compClass = CompClass;
        }

    }

    public class CompSupplyBox : ThingComp
    {
        public CompProperties_SupplyBox Props => (CompProperties_SupplyBox)props;

        //public bool isLocked => Props.isLocked;
        public bool isLocked;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                isLocked = Props.isLocked;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref isLocked, "isLocked", Props.isLocked);
        }

    }

    public class CompUseEffect_SpawnSupplies : CompUseEffect
    {
        Random random = new Random();
        private List<ThingDef> containedSupplies => parent.GetComp<CompSupplyBox>().Props.containedSupplies;
        private List<int> amountMin => parent.GetComp<CompSupplyBox>().Props.amountMin;
        private List<int> amountMax => parent.GetComp<CompSupplyBox>().Props.amountMax;
        private List<int> chances => parent.GetComp<CompSupplyBox>().Props.chances;
        private bool isLocked => parent.GetComp<CompSupplyBox>().Props.isLocked;

        public override void DoEffect(Pawn usedBy)
        {
            if (!parent.GetComp<CompSupplyBox>().isLocked)
            {
                base.DoEffect(usedBy);
                foreach (ThingDef thing in containedSupplies)
                {
                    if (Rand.Range(0, 100) <= chances[containedSupplies.IndexOf(thing)])
                    {
                        Thing itemToSpawn = ThingMaker.MakeThing(thing, thing.defName == "EM_SantaHat" ? ThingDefOf.Cloth : null);
                        itemToSpawn.stackCount = random.Next(amountMin[containedSupplies.IndexOf(thing)], amountMax[containedSupplies.IndexOf(thing)]);
                        GenPlace.TryPlaceThing(itemToSpawn, parent.Position, parent.Map, ThingPlaceMode.Near);
                    }
                    //itemToSpawn.stackCount = amount[containedSupplies.IndexOf(thing)];
                }
            }
        }

        public override bool CanBeUsedBy(Pawn p, out string failReason)
        {
            if (DateTime.Now.Month != 12)
            {
                failReason = "Can't open outside of Christmas time.";
                return false;
            }
            failReason = null;
            return true;
        }
    }

}
