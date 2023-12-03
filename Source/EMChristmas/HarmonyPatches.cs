//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Reflection;

//using RimWorld;
//using Verse;
//using HarmonyLib;

//namespace EMChristmas
//{
//    [StaticConstructorOnStartup]
//    public class SnowPatch
//    {
//        public static void Patch()
//        {
//            var harmony = new Harmony("Argon.ExpandedMaterials.Xmas");
//            harmony.PatchAll(Assembly.GetExecutingAssembly());
//        }
//    }

//    [HarmonyPatch(typeof(JobDriver_ClearSnow))]
//    [HarmonyPatch(nameof(JobDriver_ClearSnow.TryMakePreToilReservations))]

//}
