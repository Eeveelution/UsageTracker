﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UsageTracker.Common;

List<TrackedProcess> trackedProcesses = new();

//Make sure File exists
if(!File.Exists("processes"))
    File.WriteAllText("processes", "");

string[] processes = File.ReadAllLines("processes");

if(processes.Length == 0)
    Environment.Exit(-1);

foreach (string process in processes) {
    TrackedProcess trackedProcess = TrackedProcess.FromProcessName(process);

    if (trackedProcess != null)
        trackedProcesses.Add(trackedProcess);
}

while (true) {
    List<Process> runningProcesses = Process.GetProcesses().ToList();

    foreach (TrackedProcess process in trackedProcesses) {
        bool isRunning = false;

        if (process.SecondsTotal % 60 == 0) {
            string secondsFile = "0";
            string processFilename = $"processes/{process.ProcessToTrack}";

            if (File.Exists(processFilename))
                secondsFile = File.ReadAllText(processFilename);

            int seconds = int.Parse(secondsFile);
            seconds += process.SecondsTracked;

            process.SecondsTracked = 0;

            File.WriteAllText(processFilename, seconds.ToString());
        }

        if (runningProcesses.Any(p => p.ProcessName == process.ProcessToTrack)) {
            isRunning = true;
            process.IsRunning = true;

            process.SecondsTotal   += 1;
            process.SecondsTracked += 1;
        }

        if (!isRunning) {
            process.IsRunning = false;
        }

        Thread.Sleep(1000);
    }
}