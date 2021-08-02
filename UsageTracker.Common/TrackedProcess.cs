using System.IO;

namespace UsageTracker.Common {
    public class TrackedProcess {
        public string ProcessToTrack;
        public string Alias;
        public int    SecondsTracked = 1;
        public int    SecondsTotal   = 1;
        public bool   IsRunning;
        public bool   Track = true;

        public static TrackedProcess FromProcessName(string processName) {
            TrackedProcess process = new();

            string[] processInformation = File.ReadAllLines($"processes/{processName}");

            if (processInformation.Length == 2) {
                process.SecondsTotal = int.Parse(processInformation[0]);
                process.Alias        = processInformation[1] != "" ? processInformation[1] : processName;
            } else return null;

            return process;
        }
    }
}
