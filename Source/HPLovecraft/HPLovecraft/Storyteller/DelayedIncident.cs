using RimWorld;
using Verse;

namespace HPLovecraft
{
    public class DelayedIncident : IExposable
    {
        public int ticksUntilIncident = -1;
        public IncidentDef incident = null;
        public IncidentParms incidentParms = null;
        public Map map = null;
        public bool didIt = false;

        public DelayedIncident()
        {

        }

        public DelayedIncident(DelayedIncident copy)
        {
            this.ticksUntilIncident = copy.ticksUntilIncident;
            this.incident = copy.incident;
            this.incidentParms = copy.incidentParms;
            this.map = copy.map;
        }

        public DelayedIncident(Map newMap, IncidentDef newIncident, IncidentParms newParms, int newTicks)
        {
            map = newMap;
            incident = newIncident;
            incidentParms = newParms;
            ticksUntilIncident = newTicks;
        }

        public void Tick()
        {
            if (!didIt && Find.TickManager.TicksGame % 1000 == 0) Log.Message(Find.TickManager.TicksGame + " " + ticksUntilIncident);
            if (Find.TickManager.TicksGame % 100 == 0 &&
                Find.TickManager.TicksGame > ticksUntilIncident &&
                map.GetComponent<MapComponent_OmenIncidentTracker>() is MapComponent_OmenIncidentTracker tracker)
            {
                    //Log.Message("IncidentTriggered");
                    tracker.TriggerIncident(this);
            }
        }

        public void ExposeData()
        {
            Scribe_References.Look(ref this.map, "map");
            Scribe_Defs.Look(ref this.incident, "incident");
            Scribe_Deep.Look(ref this.incidentParms, "incidentParms", new object[0]);
            Scribe_Values.Look(ref this.ticksUntilIncident, "ticksUntilIncident", -1);
            Scribe_Values.Look(ref this.didIt, "didIt", false);
        }
    }
}
