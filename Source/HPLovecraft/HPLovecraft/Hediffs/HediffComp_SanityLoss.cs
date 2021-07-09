﻿using Cthulhu;
using Verse;

namespace HPLovecraft
{
    public class HediffComp_SanityLoss : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (Pawn?.RaceProps != null)
            {
                if (Pawn.RaceProps.IsMechanoid)
                {
                    MakeSane();
                }
            }

            if (!Utility.IsCosmicHorrorsLoaded())
            {
                return;
            }

            if (Pawn != null && Pawn.GetType().ToString() == "CosmicHorrorPawn")
            {
                MakeSane();
            }
        }


        public void MakeSane()
        {
            parent.Severity -= 1f;
            Pawn.health.Notify_HediffChanged(parent);
        }
    }
}