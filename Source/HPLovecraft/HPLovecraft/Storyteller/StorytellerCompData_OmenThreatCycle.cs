using System.Collections.Generic;
using Cthulhu;
using RimWorld;
using Verse;

namespace HPLovecraft
{
    public static class Lovecraft
    {
        private static StorytellerData data;

        public static List<IncidentDef> Omens = new List<IncidentDef>
        {
            HPLDefOf.HPLovecraft_CatsIncident,
            HPLDefOf.HPLovecraft_ParanoiaIncident,
            HPLDefOf.HPLovecraft_CrowsIncident,
            HPLDefOf.HPLovecraft_MysteryIncident
        };

        public static StorytellerData Data
        {
            get
            {
                if (data == null)
                {
                    data = new StorytellerData();
                }

                return data;
            }
        }
    }

    public class StorytellerData : IExposable
    {
        public int cosmicHorrorEvents;
        public bool lastEventWasOmen;
        public int standardEvents;

        public StorytellerData()
        {
            Settings.DebugString("Lovecraft :: Storyteller Data Written");
        }

        public bool NeedCosmicHorrorEvent => Utility.IsCosmicHorrorsLoaded() && standardEvents != 0 &&
                                             cosmicHorrorEvents != 0 && standardEvents > 2 &&
                                             standardEvents / 3 > cosmicHorrorEvents;

        public void ExposeData()
        {
            Scribe_Values.Look(ref standardEvents, "standardEvents");
            Scribe_Values.Look(ref cosmicHorrorEvents, "cosmicHorrorEvents");
            Scribe_Values.Look(ref lastEventWasOmen, "lastEventWasOmen");
        }

        public void RecordIncident(IncidentDef def)
        {
            if (Utility.CosmicHorrorIncidents().Contains(def))
            {
                cosmicHorrorEvents++;
                return;
            }

            standardEvents++;
        }
    }
}