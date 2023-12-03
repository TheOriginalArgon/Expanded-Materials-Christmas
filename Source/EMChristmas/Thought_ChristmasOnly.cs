using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace EMChristmas
{
    internal class Thought_ChristmasOnly : Thought_Situational
    {
        public override float MoodOffset()
        {
            if (DateTime.Now.Month != 12)
            {
                return 0f;
            }
            return 3f;
        }

    }

    internal class Thought_ChristmasFood : Thought_Memory
    {
        public override float MoodOffset()
        {
            if (DateTime.Now.Month != 12)
            {
                return def.stages[0].baseMoodEffect + 0f;
            }
            return def.stages[0].baseMoodEffect + 2f;
        }
    }

    public class ThoughtWorker_WearingSantaHat : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                if (wornApparel[i].def == DefDatabase<ThingDef>.GetNamed("EM_SantaHat"))
                {
                    return true;
                }
            }
            return ThoughtState.Inactive;
        }
    }

}
