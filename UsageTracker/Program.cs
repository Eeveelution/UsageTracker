using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UsageTracker.Common;

List<TrackedProcess> trackedProcesses = new();

void RefreshProcessList() {
    //Make sure File exists
    if(!File.Exists("processes"))
        File.WriteAllText("processes", "");

    string[] processes = File.ReadAllLines("processes");

    if(processes.Length == 0)
        Environment.Exit(-1);

    foreach (string process in processes) {
        TrackedProcess trackedProcess = TrackedProcess.FromProcessName(process);

        if (trackedProcess != null && !trackedProcesses.Any(p => p.ProcessToTrack == process))
            trackedProcesses.Add(trackedProcess);
    }
}


while (true) {
    RefreshProcessList();

    List<Process> runningProcesses = Process.GetProcesses().ToList();

    foreach (TrackedProcess process in trackedProcesses) {
        bool isRunning = false;

        if (process.SecondsTotal % 60 == 0) {
            string processFilename = $"processInfo/{process.ProcessToTrack}";

            process.SecondsTotal   += process.SecondsTracked;
            process.SecondsTracked = 0;

            process.Save();
        }

        if (runningProcesses.Any(p => p.ProcessName == process.ProcessToTrack)) {
            isRunning = true;
            process.IsRunning = true;

            process.SecondsTracked += 1;
            process.SecondsTotal += 1;
        }

        if (!isRunning) {
            process.IsRunning = false;
        }
    }

    Thread.Sleep(1000);
}
