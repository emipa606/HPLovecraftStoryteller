using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using RimWorld.Planet;

namespace HPLovecraft
{
    public class IncidentWorker_Cats : IncidentWorker
    {

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Settings.DebugString("== Enter IncidentWorker_Cats == ");
            float rand = Rand.Value;
            _ = new GlobalTargetInfo();
            GlobalTargetInfo target;
            string flavorDesc;
            if (rand < 0.25f)
            {
                DeadCat(parms, out flavorDesc, out target);
            }
            else if (rand < 0.5f)
            {
                //Non-hostile Period is the first three seasons of omens.
                if (GenDate.DaysPassed > 45) WildCats(parms, out flavorDesc, out target);
                else
                {
                    StrayCat(parms, out flavorDesc, out target);
                }
            }
            else if (rand < 0.75f)
            {
                StrayCat(parms, out flavorDesc, out target);
            }
            else
            {
                AffectionateCat(parms, out flavorDesc, out target);
            }
            Find.LetterStack.ReceiveLetter(def.label.CapitalizeFirst(), flavorDesc, DefDatabase<LetterDef>.GetNamed("ROM_Omen"), target);
            return true;
        }


        /*
         * Affectionate Cat
         *
         * A single cat joins the colony.
         *
         */
        public void AffectionateCat(IncidentParms parms, out string flavorDesc, out GlobalTargetInfo target)
        {
            Settings.DebugString("Affectionate Cat");
            RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 loc, (Map)parms.target, CellFinder.EdgeRoadChance_Animal, false, null);
            Pawn newThing = PawnGenerator.GeneratePawn(HPLDefOf.HPLovecraft_CatKind_Black, null);
            Thing cat = GenSpawn.Spawn(newThing, loc, (Map)parms.target);
            InteractionWorker_RecruitAttempt.DoRecruit(cat.Map.mapPawns.FreeColonists.FirstOrDefault(), (Pawn)cat, 1f, true);
            flavorDesc = "ROM_OmenCatDesc4".Translate();
            target = new GlobalTargetInfo(cat);
        }

        /*
        * Stray Cat
        *
        * A single cat wanders onto the colony's border.
        * 
        */
        public void StrayCat(IncidentParms parms, out string flavorDesc, out GlobalTargetInfo target)
        {
            Settings.DebugString("Stray Cat");
            RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 loc, (Map)parms.target, CellFinder.EdgeRoadChance_Animal, false, null);
            Pawn newThing = PawnGenerator.GeneratePawn(HPLDefOf.HPLovecraft_CatKind_Black, null);
            Thing cat = GenSpawn.Spawn(newThing, loc, (Map)parms.target);
            flavorDesc = "ROM_OmenCatDesc3".Translate();
            target = new GlobalTargetInfo(cat);
        }

        /*
         * Wild Cats
         *
         * Multiple manhunter cats wander onto the colony's border.
         * 
         */
        public void WildCats(IncidentParms parms, out string flavorDesc, out GlobalTargetInfo target)
        {
            Settings.DebugString("Wild Cats");
            GlobalTargetInfo? newTarget = null;
            RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 loc, (Map)parms.target, CellFinder.EdgeRoadChance_Animal, false, null);
            var numberOfCats = (GenDate.YearsPassed > 0) ? GenDate.YearsPassed : 1;
            List<Thing> cats = new List<Thing>();
            for (int i = 0; i < numberOfCats; i++)
            {
                Pawn newCat = PawnGenerator.GeneratePawn(HPLDefOf.HPLovecraft_CatKind_Black, null);
                cats.Add(GenSpawn.Spawn(newCat, loc, (Map)parms.target));
            }
            foreach (Pawn single in cats)
            {
                single.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
                if (newTarget == null) newTarget = new GlobalTargetInfo(single);
            }
            target = newTarget.Value;
            flavorDesc = "ROM_OmenCatDesc2".Translate();

        }

        public void DeadCat(IncidentParms parms, out string flavorDesc, out GlobalTargetInfo target)
        {

            Settings.DebugString("Dead Cat");
            //Dead cat
            RCellFinder.TryFindRandomPawnEntryCell(out IntVec3 loc, (Map)parms.target, CellFinder.EdgeRoadChance_Animal, false, null);
            Pawn newThing = PawnGenerator.GeneratePawn(HPLDefOf.HPLovecraft_CatKind_Black, null);
            Thing cat = GenSpawn.Spawn(newThing, loc, (Map)parms.target);
            ((Pawn)cat).Kill(null);
            flavorDesc = "ROM_OmenCatDesc1".Translate();
            target = new GlobalTargetInfo(loc, (Map)parms.target);
        }
    }
}
