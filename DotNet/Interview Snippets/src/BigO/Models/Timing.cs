using System;
using System.Collections.Generic;
using System.Linq;

namespace BigO.Models
{
    public class Timing
    {
        public Dictionary<string, List<TimingInternal>> InternalTimers { get; } = new Dictionary<string, List<TimingInternal>>();

        public void AddUpdateDataStore(string action, string listType, int recordCount, double ticks, double milliSeconds)
        {
            var key = $"{action}-{listType}";

            if (InternalTimers.ContainsKey(key))
            {
                if (InternalTimers[key].Any(a => a.RecordCount == recordCount))
                {
                    InternalTimers[key].FirstOrDefault(a => a.RecordCount == recordCount).AddEntry(recordCount, ticks, milliSeconds);
                    return;
                }
                var timingInternal2 = new TimingInternal();
                timingInternal2.AddEntry(recordCount, ticks, milliSeconds);
                InternalTimers[key].Add(timingInternal2);
                return;
            }

            var timingInternal = new TimingInternal();
            timingInternal.AddEntry(recordCount, ticks, milliSeconds);
            if (!InternalTimers.ContainsKey(key))
            {
                InternalTimers.Add(key, new List<TimingInternal>());
            }
            InternalTimers[key].Add(timingInternal);
        }

        public IEnumerable<string> DisplayResults()
        {
            var results = new List<string>();
            var load1 = InternalTimers["Load-ListLinq"];
            var load2 = InternalTimers["Load-DictionaryLinq"];
            var load3 = InternalTimers["Load-Trie"];
            var read1 = InternalTimers["Read-ListLinq"];
            var read2 = InternalTimers["Read-DictionaryLinq"];
            var read3 = InternalTimers["Read-List"];
            var read4 = InternalTimers["Read-Dictionary"];
            var read5 = InternalTimers["Read-Trie"];

            results.Add("");
            results.Add($"Record Count|Load-ListLinq (s)|Load-DictionaryLinq (s)|Load-Trie (s)");
            for (int i = 0; i < load1.Count; i++)
            {
                results.Add($"{load1[i].RecordCount}|{load1[i].MilliSeconds}|{load2[i].MilliSeconds}|{load3[i].MilliSeconds}");
            }

            results.Add("");
            results.Add($"Record Count|Read-ListLinq (s)|Read-List (s)|Read-DictionaryLinq (s)|Read-Dictionary (s)|Read-Trie (s)");
            for (int i = 0; i < read1.Count; i++)
            {
                results.Add($"{read1[i].RecordCount}|{read1[i].MilliSeconds}|{read3[i].MilliSeconds}|{read2[i].MilliSeconds}|{read4[i].MilliSeconds}|{read5[i].MilliSeconds}");
            }

            return results.ToArray();
        }
    }


    public class TimingInternal
    {
        public TimingInternal()
        {
            TimingPoints = new List<TimingPoint>();
        }

        public void AddEntry(int recordCount, double ticks, double milliSeconds)
        {
            TimingPoints.Add(new TimingPoint
            {
                RecordCount = recordCount,
                Ticks = ticks,
                MilliSeconds = milliSeconds,
                Seconds = milliSeconds / 1000
            });
        }

        public List<TimingPoint> TimingPoints { get; private set; }

        public double RecordCount => TimingPoints.Average(a => a.RecordCount);
        public double Ticks => TimingPoints.Average(a => a.Ticks);
        public double MilliSeconds => TimingPoints.Average(a => a.MilliSeconds);
        public double Seconds => TimingPoints.Average(a => a.Seconds);
        public double PerSec => RecordCount / Seconds;
        public double PerTick => RecordCount / Ticks;
        public string FormattedPerSecCsv => $"{RecordCount}|{Seconds:0.000000}|{PerSec:0.0000}";
        public string FormattedPerTickCsv => $"{RecordCount}|{Ticks:0.000000}|{PerTick:0.0000}";
    }

    public class TimingPoint
    {
        public int RecordCount { get; set; }
        public double Ticks { get; set; }
        public double MilliSeconds { get; set; }
        public double Seconds { get; set; }
    }
}
